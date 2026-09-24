using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QPET.DTOs;
using QPET.Services.Interfaces;

namespace QPET.Controllers.Api
{
    [ApiController]
    [Route("api/enquiries")]
    [Authorize(
        AuthenticationSchemes =
            "QPETAdmin"
    )]
    public class EnquiriesApiController
        : ControllerBase
    {
        private readonly IEnquiryService
            _enquiryService;

        public EnquiriesApiController(
            IEnquiryService enquiryService)
        {
            _enquiryService =
                enquiryService;
        }

        [HttpGet]
        public async Task<
            ActionResult<IEnumerable<EnquiryResponse>>>
            GetEnquiries()
        {
            var enquiries =
                await _enquiryService
                    .GetEnquiriesAsync();

            var response =
                enquiries
                    .Select(MapEnquiry)
                    .ToList();

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EnquiryResponse>>
            GetEnquiry(
                int id)
        {
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

        private static EnquiryResponse MapEnquiry(
            QPET.Data.Entities.Enquiry enquiry)
        {
            return new EnquiryResponse
            {
                EnquiryId =
                    enquiry.EnquiryId,

                CustomerName =
                    enquiry.Customer
                        .FullName,

                CustomerEmail =
                    enquiry.Customer
                        .EmailAddress,

                CustomerPhoneNumber =
                    enquiry.Customer
                        .PhoneNumber,

                BranchId =
                    enquiry.BranchId,

                BranchName =
                    enquiry.Branch
                        .BranchName,

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