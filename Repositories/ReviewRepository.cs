using Book.Models;
using Microsoft.EntityFrameworkCore;

namespace Book
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Review> AddReviewAsync(int bookId, int userId, ReviewRequest reviewRequest)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);

            if(book == null) throw new NotFoundException("Книга не найдена");
            if(await _context.Reviews.AnyAsync(r => r.BookId == bookId && r.UserId == userId)) throw new ConflictException("Вы уже оставляли отзыв");

            var newReview = new Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = reviewRequest.Rating,
                Comment = reviewRequest.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(newReview);

            await _context.SaveChangesAsync();

            return newReview;
        }
        public async Task<bool> RemoveReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(a => a.Id == reviewId);

            if(review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<Review> FindReviewAsync(int reviewId, int userId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(a => a.Id == reviewId);

            if(review == null) throw new NotFoundException("Отзыв не найден");

            return review;
        }
    }
}