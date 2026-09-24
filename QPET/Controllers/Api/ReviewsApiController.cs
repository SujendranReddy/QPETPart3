using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QPET.Data.Entities;
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

        private readonly IBranchService
            _branchService;

        public ReviewsApiController(
            IReviewService reviewService,
            IBranchService branchService)
        {
            _reviewService =
                reviewService;

            _branchService =
                branchService;
        }

        [HttpGet]
        public async Task<
            ActionResult<IEnumerable<ReviewResponse>>>
            GetReviews()
        {
            var reviews =
                await _reviewService
                    .GetReviewsAsync();

            return Ok(
                reviews.Select(MapReview)
            );
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ReviewResponse>>
            GetReview(
                int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Review ID must be greater than zero."
                    }
                );
            }

            var review =
                await _reviewService
                    .GetReviewByIdAsync(id);

            if (review == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Review not found."
                    }
                );
            }

            return Ok(
                MapReview(review)
            );
        }

        [HttpPost]
        public async Task<ActionResult<ReviewResponse>>
            CreateReview(
                ReviewCreateRequest request)
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

            try
            {
                var review =
                    new Review
                    {
                        BranchId =
                            request.BranchId,

                        DisplayName =
                            request.DisplayName.Trim(),

                        EmailAddress =
                            string.IsNullOrWhiteSpace(
                                request.EmailAddress
                            )
                                ? null
                                : request.EmailAddress
                                    .Trim(),

                        Rating =
                            request.Rating,

                        ReviewMessage =
                            request.ReviewMessage
                                .Trim()
                    };

                var createdReview =
                    await _reviewService
                        .AddReviewAsync(review);

                var result =
                    await _reviewService
                        .GetReviewByIdAsync(
                            createdReview.ReviewId
                        );

                return CreatedAtAction(
                    nameof(GetReview),
                    new
                    {
                        id =
                            createdReview.ReviewId
                    },
                    MapReview(result!)
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

        [HttpDelete("{id:int}")]
        [Authorize(
            AuthenticationSchemes = "QPETAdmin",
            Roles = "Admin"
        )]
        public async Task<IActionResult> DeleteReview(
            int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Review ID must be greater than zero."
                    }
                );
            }

            var deleted =
                await _reviewService
                    .DeleteReviewAsync(id);

            if (!deleted)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Review not found."
                    }
                );
            }

            return NoContent();
        }

        private static ReviewResponse MapReview(
            Review review)
        {
            return new ReviewResponse
            {
                ReviewId =
                    review.ReviewId,

                BranchId =
                    review.BranchId,

                BranchName =
                    review.Branch?
                        .BranchName
                    ?? string.Empty,

                DisplayName =
                    review.DisplayName,

                Rating =
                    review.Rating,

                ReviewMessage =
                    review.ReviewMessage,

                CreatedDate =
                    review.CreatedDate
            };
        }
    }
}