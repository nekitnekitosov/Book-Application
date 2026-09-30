using Book.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Book
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<ReviewService> _logger;
        public ReviewService(IReviewRepository reviewRepository, IMemoryCache memoryCache, ILogger<ReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _memoryCache = memoryCache;
            _logger = logger;
        }
        public async Task<List<GetReviewResponse>> GetTopBooks(int top)
        {
            if (top <= 0) throw new ValidationException("Число не может быть меньше или равно 0");

            _memoryCache.TryGetValue("top_books", out var cached);

            if (cached == null)
            {
                var books = await _reviewRepository.GetBooksAverageRatingAsync(top);

                foreach (var book in books)
                {
                    book.AverageRating = Math.Round(book.AverageRating, 1);
                }

                if(books.Count > 0)
                {
                    _memoryCache.Set("top_books", books, TimeSpan.FromMinutes(5)); // кеширование 
                    _logger.LogInformation($"Топ {top} книг был добавлен в кеш");
                }

                return books;
            }

            return (List<GetReviewResponse>)cached;
        }
        public async Task<Review> AddReview(int bookId, int userId, ReviewRequest reviewRequest)
        {
            if (reviewRequest.Rating < 1 || reviewRequest.Rating > 5) throw new ValidationException("Рейтинг должен быть от 0 до 5");

            var addReview = await _reviewRepository.AddReviewAsync(bookId, userId, reviewRequest);

            _memoryCache.Remove("top_books");
            _logger.LogInformation("Старый кеш удален");
            return addReview;
        }
        public async Task<bool> RemoveReview(int reviewId, int userId, string userRole)
        {
            var request = await _reviewRepository.FindReviewAsync(reviewId, userId);

            if (request == null) throw new NotFoundException("Отзыв не найден");

            if (request.UserId == userId || userRole == "Admin")
            {
                await _reviewRepository.RemoveReviewAsync(reviewId);
                _memoryCache.Remove("top_books");
                _logger.LogInformation("Старый кеш удален");
                return true;
            }
            else throw new ForbiddenException("Вы не можете удалить чужой отзыв");
        }
    }
}