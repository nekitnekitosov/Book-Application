using Microsoft.AspNetCore.Mvc;
using Book.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Book.Controllers
{

    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [Authorize]
        [HttpGet("books")]
        public async Task<IActionResult> GetBooks(int page, int pageSize, [FromQuery] BookSortBy bookSortBy = BookSortBy.Name, [FromQuery] SortDirection sortDirection = SortDirection.Asc)
        {
            var books = await _bookService.GetBooks(page, pageSize, bookSortBy, sortDirection);

            return Ok(books);
        }

        [Authorize]
        [HttpGet("book{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            return Ok(await _bookService.GetBook(id));
        }
        [HttpPost("book")]
        public async Task<IActionResult> AddBook([FromBody] BookRequest bookRequest)
        {
            var role = User.FindFirst(ClaimTypes.Role).Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var book = await _bookService.AddBook(bookRequest, role, int.Parse(userId));

            if (book == null) return BadRequest();

            return Ok(book);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("book{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var request = await _bookService.DeleteBook(id);

            if (request == true) return Ok();

            return BadRequest();
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("book{id}")]
        public async Task<IActionResult> PutBook([FromBody] JsonPatchDocument<BookUpdateDto> patchDoc, int id)
        {
            if (patchDoc == null || patchDoc.Operations == null || patchDoc.Operations.Count == 0)
                return BadRequest(new { error = "Запрос не содержит операций для обновления" });

            var updatedBook = await _bookService.PutBook(patchDoc, id);
            return Ok(updatedBook);
        }
    }
}
