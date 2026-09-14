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
            if(reviewRequest.Rating < 0 || reviewRequest.Rating > 5) throw new ValidationException("Выберите от 0 до 5");

           return await _reviewRepository.AddReviewAsync(bookId, userId, reviewRequest);
        }
        public async Task<bool> RemoveReview(int reviewId, int userId, string userRole)
        {
            var request = await _reviewRepository.FindReviewAsync(reviewId, userId);

            if(request == null) throw new NotFoundException("Отзыв не найден");

            if(request.UserId == userId) await _reviewRepository.RemoveReviewAsync(reviewId);
            if(userRole == "Admin") await _reviewRepository.RemoveReviewAsync(reviewId);
            else throw new ForbiddenException("Вы не можете удалить чужой отзыв");
            
            return true;
        }
    }
}