namespace Book.Models
{
    public interface IReviewRepository
    {
        Task<Review> AddReviewAsync(int bookId, int userId, ReviewRequest reviewRequest);
        Task<bool> RemoveReviewAsync(int reviewId);
        Task<Review> FindReviewAsync(int reviewId, int userId);
    }
}