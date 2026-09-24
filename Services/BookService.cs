using Book.Models;
using Book.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace Book
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<Boook> PutBook(JsonPatchDocument<BookUpdateDto> bookRequest, int bookId)
        {
            if (bookRequest == null) throw new ValidationException("Тело запроса пустое");
            if (bookId < 0) throw new ValidationException("ID книги не может быть меньше или равно нулю");

            return await _bookRepository.PutBookAsync(bookRequest, bookId);
        }
        public async Task<PagedResult<GetBooksResponse>> GetBooks(int page, int pageSize, BookSortBy bookSortBy, SortDirection sortDirection)
        {
            if (page <= 0 || pageSize <= 0) throw new ValidationException("Укажите page или pageSize");

            return await _bookRepository.GetBooksAsync(page, pageSize, bookSortBy, sortDirection);
        }
        public async Task<GetBookResponse> GetBook(int bookId)
        {
            if(bookId < 0) throw new ValidationException("Введите корректный id книги");

            return await _bookRepository.GetBookAsync(bookId);
        }
        public async Task<Boook> AddBook(BookRequest bookRequest, string role, int userId)
        {
            var requestFind = await _bookRepository.FindBookAsync(bookRequest.BookName);

            if (requestFind != null) throw new ConflictException("Такая книга уже существует в базе!"); ;

            if(role == "User") await _bookRepository.AddModerationBookAsync(bookRequest, userId);

            if(role == "Admin") await _bookRepository.AddBookAsync(bookRequest);

            return new Boook
            {
                BookName = bookRequest.BookName,
                AuthorName = bookRequest.AuthorName,
                YearOfPublish = bookRequest.YearOfPublish,
                Description = bookRequest.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        public async Task<bool> DeleteBook(int id)
        {
            return await _bookRepository.DeleteBookAsync(id);
        }
    }
}