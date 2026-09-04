using Book.Models;
using Book.Interfaces;

namespace Book
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<PagedResult<GetBookResponse>> GetBooks(int page, int pageSize)
        {
            if(page <= 0 || pageSize <= 0) throw new ValidationException("Укажите page или pageSize");

            return await _bookRepository.GetBooksAsync(page, pageSize);
        }
        public async Task<Boook> AddBook(BookRequest bookRequest)
        {
            var requestFind = await _bookRepository.FindBookAsync(bookRequest.BookName);

            if (requestFind != null) throw new ConflictException("Такая книга уже существует в базе!");;

            var requestAdd = await _bookRepository.AddBookAsync(bookRequest);

            if (requestAdd == null) throw new NotFoundException("Ошибка! Пустой запрос");;

            return new Boook {
            BookName = bookRequest.BookName, 
            AuthorName = bookRequest.AuthorName, 
            YearOfPublish = bookRequest.YearOfPublish,
            Description = bookRequest.Description, 
            CreatedAt = DateTime.UtcNow, 
            UpdatedAt = DateTime.UtcNow };
        }
        public async Task<bool> DeleteBook(int id)
        {
            return await _bookRepository.DeleteBookAsync(id);
        }
    }
}