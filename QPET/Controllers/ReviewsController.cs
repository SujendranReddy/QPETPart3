using Microsoft.AspNetCore.Mvc;
using QPET.Models;
using QPET.Services;

namespace QPET.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly PrototypeDataService
            _dataService;


        public ReviewsController(
            PrototypeDataService dataService)
        {
            _dataService =
                dataService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View(
                BuildPageModel()
            );
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitReview(
            ReviewsPageViewModel model)
        {
            model.Review ??=
                new ReviewViewModel();


            ValidateBranch(
                model.Review.BranchId
            );


            if (!ModelState.IsValid)
            {
                model.Reviews =
                    _dataService
                        .GetReviews()
                        .OrderByDescending(
                            review =>
                                review.ReviewId
                        )
                        .ToList();


                model.Branches =
                    _dataService
                        .GetBranches();


                return View(
                    "Index",
                    model
                );
            }


            var review =
                new Review
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


            _dataService.AddReview(
                review
            );


            TempData["ReviewMessage"] =
                "Thank you. Your review has been submitted successfully.";


            return RedirectToAction(
                nameof(Index)
            );
        }


        private ReviewsPageViewModel
            BuildPageModel()
        {
            return new ReviewsPageViewModel
            {
                Reviews =
                    _dataService
                        .GetReviews()
                        .OrderByDescending(
                            review =>
                                review.ReviewId
                        )
                        .ToList(),

                Branches =
                    _dataService
                        .GetBranches(),

                Review =
                    new ReviewViewModel()
            };
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
                    "Review.BranchId",
                    "Please select a valid branch."
                );
            }
        }
    }
}