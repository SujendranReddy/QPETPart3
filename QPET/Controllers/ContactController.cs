using Microsoft.AspNetCore.Mvc;
using QPET.Models;
using QPET.Services;

namespace QPET.Controllers
{
    public class ContactController : Controller
    {
        private readonly PrototypeDataService
            _dataService;


        public ContactController(
            PrototypeDataService dataService)
        {
            _dataService =
                dataService;
        }


        [HttpGet]
        public IActionResult Index(
            string? product = null)
        {
            PopulateBranches();


            var model =
                new EnquiryViewModel();


            if (
                !string.IsNullOrWhiteSpace(
                    product
                ))
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
        public IActionResult SubmitEnquiry(
            EnquiryViewModel model)
        {
            ValidateBranch(
                model.BranchId
            );

            ValidateAttachments(
                model.Attachments
            );


            if (!ModelState.IsValid)
            {
                PopulateBranches();

                return View(
                    "Index",
                    model
                );
            }

            // This reuses existing customers with the same email so one customer can have many enquiries 
            var customer =
                _dataService
                    .FindCustomerByEmail(
                        model.EmailAddress.Trim()
                    );


            if (customer == null)
            {
                customer =
                    _dataService.AddCustomer(
                        new Customer
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

            // The prototype stores attachment metadata instead of  saving physical files
            var attachments =
                model.Attachments
                    .Where(
                        file =>
                            file.Length > 0
                    )
                    .Select(
                        file =>
                            new EnquiryAttachment
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
                _dataService.AddEnquiry(
                    new Enquiry
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
        public IActionResult Confirmation(
            int id)
        {
            var enquiry =
                _dataService
                    .GetEnquiryById(id);


            if (enquiry == null)
            {
                return RedirectToAction(
                    nameof(Index)
                );
            }


            PopulateBranches();


            ViewBag.EnquiryReference =
                "ENQ-" +
                enquiry.EnquiryId;


            return View(
                "Index",
                new EnquiryViewModel()
            );
        }


        private void PopulateBranches()
        {
            ViewBag.Branches =
                _dataService
                    .GetBranches();
        }


        private void ValidateBranch(
            int branchId)
        {
            var exists =
                _dataService
                    .GetBranches()
                    .Any(
                        branch =>
                            branch.BranchId ==
                            branchId
                    );


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


            foreach (
                var file in attachments)
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


                if (
                    !allowedExtensions
                        .Contains(extension))
                {
                    ModelState.AddModelError(
                        nameof(
                            EnquiryViewModel
                                .Attachments
                        ),
                        "Only PDF, JPG, JPEG and PNG files are allowed."
                    );

                    break;
                }
            }
        }
    }
}