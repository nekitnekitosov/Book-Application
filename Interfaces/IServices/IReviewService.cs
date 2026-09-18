using Book.Models;

namespace Book
{
    public interface IReviewService
    {
        Task<Review> AddReview(int bookId, int userId, ReviewRequest reviewRequest);
        Task<bool> RemoveReview(int reviewId, int userId, string userRole);
        Task<List<GetReviewResponse>> GetTopBooks(int top);
    }
}