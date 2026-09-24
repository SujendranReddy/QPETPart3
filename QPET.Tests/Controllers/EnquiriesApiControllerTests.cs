using Microsoft.AspNetCore.Mvc;
using Moq;
using QPET.Controllers.Api;
using QPET.Data.Entities;
using QPET.DTOs;
using QPET.Services.Interfaces;

namespace QPET.Tests.Controllers
{
    public class EnquiriesApiControllerTests
    {
        [Fact]
        public async Task GetEnquiry_InvalidId_ReturnsBadRequest()
        {
            var enquiryService =
                new Mock<IEnquiryService>();

            var branchService =
                new Mock<IBranchService>();

            var controller =
                new EnquiriesApiController(
                    enquiryService.Object,
                    branchService.Object
                );

            var result =
                await controller.GetEnquiry(
                    0
                );

            Assert.IsType<
                BadRequestObjectResult>(
                    result.Result
                );
        }

        [Fact]
        public async Task GetEnquiry_MissingEnquiry_ReturnsNotFound()
        {
            var enquiryService =
                new Mock<IEnquiryService>();

            var branchService =
                new Mock<IBranchService>();

            enquiryService
                .Setup(
                    current =>
                        current
                            .GetEnquiryByIdAsync(
                                999
                            )
                )
                .ReturnsAsync(
                    (Enquiry?)null
                );

            var controller =
                new EnquiriesApiController(
                    enquiryService.Object,
                    branchService.Object
                );

            var result =
                await controller.GetEnquiry(
                    999
                );

            Assert.IsType<
                NotFoundObjectResult>(
                    result.Result
                );
        }

        [Fact]
        public async Task UpdateEnquiryStatus_InvalidStatus_ReturnsBadRequest()
        {
            var enquiryService =
                new Mock<IEnquiryService>();

            var branchService =
                new Mock<IBranchService>();

            enquiryService
                .Setup(
                    current =>
                        current
                            .GetEnquiryByIdAsync(
                                1
                            )
                )
                .ReturnsAsync(
                    new Enquiry
                    {
                        EnquiryId = 1,
                        CustomerId = 1,
                        BranchId = 1,
                        Subject =
                            "Test enquiry",
                        Message =
                            "Test message",
                        CreatedDate =
                            DateTime.UtcNow,
                        Status =
                            "New"
                    }
                );

            var controller =
                new EnquiriesApiController(
                    enquiryService.Object,
                    branchService.Object
                );

            var request =
                new EnquiryStatusUpdateRequest
                {
                    Status =
                        "InvalidStatus"
                };

            var result =
                await controller
                    .UpdateEnquiryStatus(
                        1,
                        request
                    );

            Assert.IsType<
                BadRequestObjectResult>(
                    result
                );
        }

        [Fact]
        public async Task UpdateEnquiryStatus_RejectedTransition_ReturnsBadRequest()
        {
            var enquiryService =
                new Mock<IEnquiryService>();

            var branchService =
                new Mock<IBranchService>();

            enquiryService
                .Setup(
                    current =>
                        current
                            .GetEnquiryByIdAsync(
                                1
                            )
                )
                .ReturnsAsync(
                    new Enquiry
                    {
                        EnquiryId = 1,
                        CustomerId = 1,
                        BranchId = 1,
                        Subject =
                            "Test enquiry",
                        Message =
                            "Test message",
                        CreatedDate =
                            DateTime.UtcNow,
                        Status =
                            "New"
                    }
                );

            enquiryService
                .Setup(
                    current =>
                        current
                            .UpdateEnquiryStatusAsync(
                                1,
                                "Resolved"
                            )
                )
                .ReturnsAsync(false);

            var controller =
                new EnquiriesApiController(
                    enquiryService.Object,
                    branchService.Object
                );

            var request =
                new EnquiryStatusUpdateRequest
                {
                    Status =
                        "Resolved"
                };

            var result =
                await controller
                    .UpdateEnquiryStatus(
                        1,
                        request
                    );

            Assert.IsType<
                BadRequestObjectResult>(
                    result
                );
        }
    }
}