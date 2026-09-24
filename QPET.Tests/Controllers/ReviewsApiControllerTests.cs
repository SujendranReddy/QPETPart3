using Microsoft.AspNetCore.Mvc;
using Moq;
using QPET.Controllers.Api;
using QPET.Data.Entities;
using QPET.Services.Interfaces;

namespace QPET.Tests.Controllers
{
    public class ReviewsApiControllerTests
    {
        [Fact]
        public async Task GetReview_InvalidId_ReturnsBadRequest()
        {
            var reviewService =
                new Mock<IReviewService>();

            var branchService =
                new Mock<IBranchService>();

            var controller =
                new ReviewsApiController(
                    reviewService.Object,
                    branchService.Object
                );

            var result =
                await controller.GetReview(
                    0
                );

            Assert.IsType<
                BadRequestObjectResult>(
                    result.Result
                );
        }

        [Fact]
        public async Task GetReview_MissingReview_ReturnsNotFound()
        {
            var reviewService =
                new Mock<IReviewService>();

            var branchService =
                new Mock<IBranchService>();

            reviewService
                .Setup(
                    current =>
                        current
                            .GetReviewByIdAsync(
                                999
                            )
                )
                .ReturnsAsync(
                    (Review?)null
                );

            var controller =
                new ReviewsApiController(
                    reviewService.Object,
                    branchService.Object
                );

            var result =
                await controller.GetReview(
                    999
                );

            Assert.IsType<
                NotFoundObjectResult>(
                    result.Result
                );
        }

        [Fact]
        public async Task DeleteReview_MissingReview_ReturnsNotFound()
        {
            var reviewService =
                new Mock<IReviewService>();

            var branchService =
                new Mock<IBranchService>();

            reviewService
                .Setup(
                    current =>
                        current
                            .DeleteReviewAsync(
                                999
                            )
                )
                .ReturnsAsync(false);

            var controller =
                new ReviewsApiController(
                    reviewService.Object,
                    branchService.Object
                );

            var result =
                await controller.DeleteReview(
                    999
                );

            Assert.IsType<
                NotFoundObjectResult>(
                    result
                );
        }
    }
}