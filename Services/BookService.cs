using Book.Models;
using Book.Repositories;

namespace Book
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        public async Task<bool> AddBook(BookRequest bookRequest)
        {
            var request = await _bookRepository.AddBookAsync(bookRequest);

            if(request == null) return false;

            return true;
        }
    }
}