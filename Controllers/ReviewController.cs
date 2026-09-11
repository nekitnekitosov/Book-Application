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
        [HttpPost("{bookId}/review")]
        public async Task<IActionResult> AddReview(int bookId, [FromBody] ReviewRequest reviewRequest)
        {
            var request = await _reviewService.AddReview(bookId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value), reviewRequest);
            
            if(request == null) return BadRequest("Ошибка");

            return Ok(request);
        }
        [HttpDelete("{reviewId}review")]
        public async Task<IActionResult> RemoveReview (int reviewId)
        {
            var request = await _reviewService.RemoveReview(reviewId);

            if(request == false) return BadRequest("Не получилось удалить отзыв");

            return Ok(request);
        }
    }
}