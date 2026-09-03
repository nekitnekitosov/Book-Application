using Book.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Book.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }
        // public async Task GetBooksAsync()
        // {
            
        // }    
        public async Task<Boook> AddBookAsync(BookRequest bookRequest)
        {
            var book = new Boook
            {
                BookName = bookRequest.BookName,
                AuthorName = bookRequest.AuthorName,
                YearOfPublish = bookRequest.YearOfPublish,
                Description = bookRequest.Description,
                CreatedAt = bookRequest.CreatedAt,
                UpdatedAt = bookRequest.UpdatedAt
            };

            _context.Books.Add(book);

            await _context.SaveChangesAsync();

            return book;
        }
        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if(book != null)
            {
                _context.Books.Remove(book);
               await _context.SaveChangesAsync();
               return true;
            }

            return false;
        }
        public async Task<Boook> PutBookAsync(BookRequest bookRequest, int idBook)
        {
            var book = await _context.Books.FirstOrDefaultAsync(a => a.BookId == idBook);

            if(book == null) throw new NotFoundException("Книга не найдена!");

            var newBook = new Boook
            {
                BookId = bookRequest.BookId,
                BookName = bookRequest.BookName,
                AuthorName = bookRequest.AuthorName
            };

            return newBook;
        }
        public async Task<string> FindBookAsync(string nameBook)
        {
            var request = await _context.Books.FirstOrDefaultAsync(a => a.BookName == nameBook);

            if(request == null) return null;

           return request.BookName;
        }
    }
}