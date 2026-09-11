using Book.Models;

namespace Book
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public async Task<Review> AddReview(int bookId, int userId, ReviewRequest reviewRequest)
        {
           return await _reviewRepository.AddReviewAsync(bookId, userId, reviewRequest);
        }
        public async Task<bool> RemoveReview(int reviewId)
        {
            return await _reviewRepository.RemoveReviewAsync(reviewId);
        }
    }
}