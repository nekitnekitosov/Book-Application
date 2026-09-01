using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Book.Interfaces;

namespace Book.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpPost("books")]
        public async Task<IActionResult> AddBook([FromBody] BookRequest bookRequest)
        {
            var book = await _bookService.AddBook(bookRequest);

            if(book == null) return BadRequest();

            return Ok(book);
        }
    }
}