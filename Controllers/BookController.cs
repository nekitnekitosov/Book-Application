using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Book.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Azure;
using Microsoft.AspNetCore.JsonPatch;

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
        //[Authorize]
        [HttpGet("book")]
        public async Task<IActionResult> GetBooks(int page, int pageSize, [FromQuery] GetBookSortRequest getBookSortRequest)
        {
            var books = await _bookService.GetBooks(page, pageSize, getBookSortRequest);

            return Ok(books);
        }
        // [Authorize(Roles = "Admin")]
        [HttpPost("book")]
        public async Task<IActionResult> AddBook([FromBody] BookRequest bookRequest)
        {
            var book = await _bookService.AddBook(bookRequest);

            if (book == null) return BadRequest();

            return Ok(book);
        }
        // [Authorize(Roles = "Admin")]
        [HttpDelete("book{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var request = await _bookService.DeleteBook(id);

            if (request == true) return Ok();

            return BadRequest();
        }
        [HttpPatch("book/{bookId}")]
        public async Task<IActionResult> PutBook([FromBody] JsonPatchDocument<BookUpdateDto> patchDoc, int bookId)
        {
            // var request = await _bookService.PutBook(bookRequest, bookId);
            // if (!ModelState.IsValid)
            // {
            //     var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            //     return BadRequest(new { errors });
            // }
            // return Ok(new
            // {
            //     request.BookId,
            //     request.BookName,
            //     request.AuthorName,
            //     request.YearOfPublish,
            //     request.Description,
            //     request.UpdatedAt
            // });
            if (patchDoc == null || patchDoc.Operations == null || patchDoc.Operations.Count == 0)
                return BadRequest(new { error = "Запрос не содержит операций для обновления" });

            var updatedBook = await _bookService.PutBook(patchDoc, bookId);
            return Ok(updatedBook);
        }
        [HttpPatch("test")]
        public IActionResult TestPatch([FromBody] JsonPatchDocument<BookUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("patchDoc is null");

            if (patchDoc.Operations == null || patchDoc.Operations.Count == 0)
                return BadRequest("No operations");

            // Создаём тестовый объект
            var test = new BookUpdateDto
            {
                BookName = "Старое название",
                AuthorName = "Старый автор"
            };

            // Применяем патч
            patchDoc.ApplyTo(test);

            // Возвращаем результат
            return Ok(new
            {
                Before = new { test.BookName, test.AuthorName },
                After = new { test.BookName, test.AuthorName },
                Operations = patchDoc.Operations.Select(o => new { o.op, o.path, o.value })
            });
        }
    }
}