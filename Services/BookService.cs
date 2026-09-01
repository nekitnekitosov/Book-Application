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
        public async Task<Boook> AddBook(BookRequest bookRequest)
        {
            var requestFind = await _bookRepository.FindBookAsync(bookRequest.BookName);

            if (requestFind != null) return null;

            var requestAdd = await _bookRepository.AddBookAsync(bookRequest);

            if (requestAdd == null) return null;

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