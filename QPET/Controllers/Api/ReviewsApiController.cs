using Microsoft.AspNetCore.Mvc;
using QPET.DTOs;
using QPET.Services.Interfaces;

namespace QPET.Controllers.Api
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsApiController
        : ControllerBase
    {
        private readonly IReviewService
            _reviewService;

        public ReviewsApiController(
            IReviewService reviewService)
        {
            _reviewService =
                reviewService;
        }

        [HttpGet]
        public async Task<
            ActionResult<IEnumerable<ReviewResponse>>>
            GetReviews()
        {
            var reviews =
                await _reviewService
                    .GetReviewsAsync();

            var response =
                reviews
                    .Select(
                        review =>
                            new ReviewResponse
                            {
                                ReviewId =
                                    review.ReviewId,

                                BranchId =
                                    review.BranchId,

                                BranchName =
                                    review.Branch
                                        .BranchName,

                                DisplayName =
                                    review.DisplayName,

                                Rating =
                                    review.Rating,

                                ReviewMessage =
                                    review.ReviewMessage,

                                CreatedDate =
                                    review.CreatedDate
                            }
                    )
                    .ToList();

            return Ok(response);
        }
    }
}