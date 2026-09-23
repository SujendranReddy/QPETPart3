using Microsoft.AspNetCore.Mvc;
using QPET.Data.Entities;
using QPET.Models;
using QPET.Services.Interfaces;

namespace QPET.Controllers
{
    public class ContactController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly IEnquiryService _enquiryService;

        public ContactController(
            IBranchService branchService,
            IEnquiryService enquiryService)
        {
            _branchService = branchService;
            _enquiryService = enquiryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? product = null)
        {
            await PopulateBranchesAsync();

            var model =
                new EnquiryViewModel();

            if (!string.IsNullOrWhiteSpace(product))
            {
                model.Subject =
                    "Enquiry about " +
                    product;

                model.Message =
                    "I would like more information about " +
                    product +
                    ", including available commercial quantities and product specifications.";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitEnquiry(
            EnquiryViewModel model)
        {
            await ValidateBranchAsync(
                model.BranchId
            );

            ValidateAttachments(
                model.Attachments
            );

            if (!ModelState.IsValid)
            {
                await PopulateBranchesAsync();

                return View(
                    "Index",
                    model
                );
            }

            var customer =
                await _enquiryService
                    .FindCustomerByEmailAsync(
                        model.EmailAddress.Trim()
                    );

            if (customer == null)
            {
                customer =
                    await _enquiryService
                        .AddCustomerAsync(
                            new QPET.Data.Entities.Customer
                            {
                                FullName =
                                    model.FullName.Trim(),

                                EmailAddress =
                                    model.EmailAddress.Trim(),

                                PhoneNumber =
                                    model.PhoneNumber.Trim()
                            }
                        );
            }

            var attachments =
                model.Attachments
                    .Where(
                        file =>
                            file.Length > 0
                    )
                    .Select(
                        file =>
                            new QPET.Data.Entities.EnquiryAttachment
                            {
                                OriginalFileName =
                                    Path.GetFileName(
                                        file.FileName
                                    ),

                                StoredFileName =
                                    Path.GetFileName(
                                        file.FileName
                                    ),

                                ContentType =
                                    string.IsNullOrWhiteSpace(
                                        file.ContentType
                                    )
                                        ? "unknown"
                                        : file.ContentType
                            }
                    )
                    .ToList();

            var enquiry =
                await _enquiryService
                    .AddEnquiryAsync(
                        new QPET.Data.Entities.Enquiry
                        {
                            CustomerId =
                                customer.CustomerId,

                            BranchId =
                                model.BranchId,

                            Subject =
                                model.Subject.Trim(),

                            Message =
                                model.Message.Trim(),

                            Attachments =
                                attachments
                        }
                    );

            return RedirectToAction(
                nameof(Confirmation),
                new
                {
                    id = enquiry.EnquiryId
                }
            );
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(
            int id)
        {
            var enquiry =
                await _enquiryService
                    .GetEnquiryByIdAsync(id);

            if (enquiry == null)
            {
                return RedirectToAction(
                    nameof(Index)
                );
            }

            await PopulateBranchesAsync();

            ViewBag.EnquiryReference =
                "ENQ-" +
                enquiry.EnquiryId;

            return View(
                "Index",
                new EnquiryViewModel()
            );
        }

        private async Task PopulateBranchesAsync()
        {
            ViewBag.Branches =
                await _branchService
                    .GetBranchesAsync();
        }

        private async Task ValidateBranchAsync(
            int branchId)
        {
            var exists =
                await _branchService
                    .ExistsAsync(branchId);

            if (!exists)
            {
                ModelState.AddModelError(
                    nameof(
                        EnquiryViewModel.BranchId
                    ),
                    "Please select a valid branch."
                );
            }
        }

        private void ValidateAttachments(
            IEnumerable<IFormFile> attachments)
        {
            var allowedExtensions =
                new[]
                {
                    ".pdf",
                    ".jpg",
                    ".jpeg",
                    ".png"
                };

            foreach (var file in attachments)
            {
                if (file.Length == 0)
                {
                    continue;
                }

                var extension =
                    Path.GetExtension(
                        file.FileName
                    )
                    .ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        nameof(
                            EnquiryViewModel.Attachments
                        ),
                        "Only PDF, JPG, JPEG and PNG files are allowed."
                    );

                    break;
                }
            }
        }
    }
}