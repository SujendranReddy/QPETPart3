using Microsoft.AspNetCore.Mvc;
using QPET.Models;
using QPET.Services.Interfaces;

namespace QPET.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService
            _reviewService;

        private readonly IBranchService
            _branchService;

        public ReviewsController(
            IReviewService reviewService,
            IBranchService branchService)
        {
            _reviewService =
                reviewService;

            _branchService =
                branchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(
                await BuildPageModelAsync()
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(
            ReviewsPageViewModel model)
        {
            model.Review ??=
                new ReviewViewModel();

            var validBranch =
                await _branchService
                    .ExistsAsync(
                        model.Review.BranchId
                    );

            if (!validBranch)
            {
                ModelState.AddModelError(
                    "Review.BranchId",
                    "Please select a valid branch."
                );
            }

            if (!ModelState.IsValid)
            {
                model.Reviews =
                    await _reviewService
                        .GetReviewsAsync();

                model.Branches =
                    await _branchService
                        .GetBranchesAsync();

                return View(
                    "Index",
                    model
                );
            }

            var review =
                new QPET.Data.Entities.Review
                {
                    BranchId =
                        model.Review.BranchId,

                    DisplayName =
                        model.Review
                            .DisplayName
                            .Trim(),

                    EmailAddress =
                        string.IsNullOrWhiteSpace(
                            model.Review.EmailAddress
                        )
                            ? null
                            : model.Review
                                .EmailAddress
                                .Trim(),

                    Rating =
                        model.Review.Rating,

                    ReviewMessage =
                        model.Review
                            .ReviewMessage
                            .Trim()
                };

            await _reviewService
                .AddReviewAsync(review);

            TempData["ReviewMessage"] =
                "Thank you. Your review has been submitted successfully.";

            return RedirectToAction(
                nameof(Index)
            );
        }

        private async Task<ReviewsPageViewModel>
            BuildPageModelAsync()
        {
            return new ReviewsPageViewModel
            {
                Reviews =
                    await _reviewService
                        .GetReviewsAsync(),

                Branches =
                    await _branchService
                        .GetBranchesAsync(),

                Review =
                    new ReviewViewModel()
            };
        }
    }
}