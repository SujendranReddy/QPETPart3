using Microsoft.EntityFrameworkCore;
using QPET.Data;
using QPET.Data.Entities;
using QPET.Services.Interfaces;

namespace QPET.Services
{
    public class ReviewService : IReviewService
    {
        private readonly QpetDbContext _context;

        public ReviewService(
            QpetDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>>
            GetReviewsAsync()
        {
            return await _context.Reviews
                .AsNoTracking()
                .Include(
                    review =>
                        review.Branch
                )
                .OrderByDescending(
                    review =>
                        review.ReviewId
                )
                .ToListAsync();
        }
        public async Task<Review?> GetReviewByIdAsync(
    int reviewId)
        {
            if (reviewId <= 0)
            {
                return null;
            }

            return await _context.Reviews
                .AsNoTracking()
                .Include(
                    review =>
                        review.Branch
                )
                .FirstOrDefaultAsync(
                    review =>
                        review.ReviewId ==
                        reviewId
                );
        }

        public async Task<Review> AddReviewAsync(
            Review review)
        {
            var validBranch =
                await _context.Branches
                    .AnyAsync(
                        branch =>
                            branch.BranchId ==
                            review.BranchId
                    );

            if (!validBranch)
            {
                throw new ArgumentException(
                    "The selected branch is invalid."
                );
            }

            review.CreatedDate =
                DateTime.Now;

            await _context.Reviews
                .AddAsync(review);

            await _context
                .SaveChangesAsync();

            return review;
        }

        public async Task<bool> DeleteReviewAsync(
            int reviewId)
        {
            if (reviewId <= 0)
            {
                return false;
            }

            var review =
                await _context.Reviews
                    .FirstOrDefaultAsync(
                        review =>
                            review.ReviewId ==
                            reviewId
                    );

            if (review == null)
            {
                return false;
            }

            _context.Reviews.Remove(review);

            await _context
                .SaveChangesAsync();

            return true;
        }
    }
}