using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QPET.Data.Entities;
using QPET.DTOs;
using QPET.Services.Interfaces;

namespace QPET.Controllers.Api
{
    [ApiController]
    [Route("api/enquiries")]
    public class EnquiriesApiController
        : ControllerBase
    {
        private readonly IEnquiryService
            _enquiryService;

        private readonly IBranchService
            _branchService;

        public EnquiriesApiController(
            IEnquiryService enquiryService,
            IBranchService branchService)
        {
            _enquiryService =
                enquiryService;

            _branchService =
                branchService;
        }

        [HttpGet]
        [Authorize(
            AuthenticationSchemes = "QPETAdmin",
            Roles = "Admin"
        )]
        public async Task<
            ActionResult<IEnumerable<EnquiryResponse>>>
            GetEnquiries()
        {
            var enquiries =
                await _enquiryService
                    .GetEnquiriesAsync();

            return Ok(
                enquiries.Select(MapEnquiry)
            );
        }

        [HttpGet("{id:int}")]
        [Authorize(
            AuthenticationSchemes = "QPETAdmin",
            Roles = "Admin"
        )]
        public async Task<ActionResult<EnquiryResponse>>
            GetEnquiry(
                int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Enquiry ID must be greater than zero."
                    }
                );
            }

            var enquiry =
                await _enquiryService
                    .GetEnquiryByIdAsync(id);

            if (enquiry == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Enquiry not found."
                    }
                );
            }

            return Ok(
                MapEnquiry(enquiry)
            );
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<EnquiryResponse>>
            CreateEnquiry(
                EnquiryCreateRequest request)
        {
            if (
                !await _branchService
                    .ExistsAsync(
                        request.BranchId
                    ))
            {
                return BadRequest(
                    new
                    {
                        message =
                            "The selected branch is invalid."
                    }
                );
            }

            var customer =
                await _enquiryService
                    .FindCustomerByEmailAsync(
                        request.EmailAddress.Trim()
                    );

            if (customer == null)
            {
                customer =
                    await _enquiryService
                        .AddCustomerAsync(
                            new Customer
                            {
                                FullName =
                                    request.FullName.Trim(),

                                EmailAddress =
                                    request.EmailAddress.Trim(),

                                PhoneNumber =
                                    request.PhoneNumber.Trim()
                            }
                        );
            }

            try
            {
                var enquiry =
                    await _enquiryService
                        .AddEnquiryAsync(
                            new Enquiry
                            {
                                CustomerId =
                                    customer.CustomerId,

                                BranchId =
                                    request.BranchId,

                                Subject =
                                    request.Subject.Trim(),

                                Message =
                                    request.Message.Trim()
                            }
                        );

                var result =
                    await _enquiryService
                        .GetEnquiryByIdAsync(
                            enquiry.EnquiryId
                        );

                return CreatedAtAction(
                    nameof(GetEnquiry),
                    new
                    {
                        id =
                            enquiry.EnquiryId
                    },
                    MapEnquiry(result!)
                );
            }
            catch (ArgumentException exception)
            {
                return BadRequest(
                    new
                    {
                        message =
                            exception.Message
                    }
                );
            }
        }

        [HttpPatch("{id:int}/status")]
        [Authorize(
            AuthenticationSchemes = "QPETAdmin",
            Roles = "Admin"
        )]
        public async Task<IActionResult>
            UpdateEnquiryStatus(
                int id,
                EnquiryStatusUpdateRequest request)
        {
            if (id <= 0)
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Enquiry ID must be greater than zero."
                    }
                );
            }

            var enquiry =
                await _enquiryService
                    .GetEnquiryByIdAsync(id);

            if (enquiry == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Enquiry not found."
                    }
                );
            }

            var allowedStatuses =
                new[]
                {
                    "New",
                    "InProgress",
                    "Resolved",
                    "Closed"
                };

            if (
                !allowedStatuses.Contains(
                    request.Status
                ))
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Status must be New, InProgress, Resolved or Closed."
                    }
                );
            }

            var updated =
                await _enquiryService
                    .UpdateEnquiryStatusAsync(
                        id,
                        request.Status
                    );

            if (!updated)
            {
                return BadRequest(
                    new
                    {
                        message =
                            $"The enquiry cannot move from {enquiry.Status} to {request.Status}."
                    }
                );
            }

            return NoContent();
        }

        private static EnquiryResponse MapEnquiry(
            Enquiry enquiry)
        {
            return new EnquiryResponse
            {
                EnquiryId =
                    enquiry.EnquiryId,

                CustomerName =
                    enquiry.Customer?
                        .FullName
                    ?? string.Empty,

                CustomerEmail =
                    enquiry.Customer?
                        .EmailAddress
                    ?? string.Empty,

                CustomerPhoneNumber =
                    enquiry.Customer?
                        .PhoneNumber
                    ?? string.Empty,

                BranchId =
                    enquiry.BranchId,

                BranchName =
                    enquiry.Branch?
                        .BranchName
                    ?? string.Empty,

                Subject =
                    enquiry.Subject,

                Message =
                    enquiry.Message,

                CreatedDate =
                    enquiry.CreatedDate,

                Status =
                    enquiry.Status,

                Attachments =
                    enquiry.Attachments
                        .Select(
                            attachment =>
                                new EnquiryAttachmentResponse
                                {
                                    AttachmentId =
                                        attachment
                                            .AttachmentId,

                                    OriginalFileName =
                                        attachment
                                            .OriginalFileName,

                                    ContentType =
                                        attachment
                                            .ContentType
                                }
                        )
                        .ToList()
            };
        }
    }
}