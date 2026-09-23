using QPET.Data.Entities;

namespace QPET.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<Review>> GetReviewsAsync();

        Task<Review> AddReviewAsync(
            Review review);

        Task<bool> DeleteReviewAsync(
            int reviewId);
    }
}