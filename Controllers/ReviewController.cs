using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Book
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [Authorize]
        [HttpGet("top")]
        public async Task<IActionResult> GetTopBooks(int top)
        {
            var books = await _reviewService.GetTopBooks(top);

            if(books == null) return NotFound("Список книг пуст");

            return Ok(books);
        }
        [Authorize]
        [HttpPost("review/{bookId}")]
        public async Task<IActionResult> AddReview(int bookId, [FromBody] ReviewRequest reviewRequest)
        {
            var request = await _reviewService.AddReview(bookId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value), reviewRequest);
            
            if(request == null) return BadRequest("Ошибка");

            return Ok(new {Status = "Успешно добавили отзыв", Rating = reviewRequest.Rating, Comment = reviewRequest.Comment});
        }
        [HttpDelete("review/{reviewId}")]
        public async Task<IActionResult> RemoveReview (int reviewId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "User";

            if(userId == null) return Unauthorized(new {error = "ID пользователя не найдено в токене"});

            var request = await _reviewService.RemoveReview(reviewId, int.Parse(userId.Value), userRole);

            if(request == false) return BadRequest("Не получилось удалить отзыв");

            return Ok(request);
        }
    }
}