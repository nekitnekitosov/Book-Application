using Book.Models;
using Microsoft.EntityFrameworkCore;

namespace Book.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

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
        public async Task<string> FindBookAsync(string nameBook)
        {
            var request = await _context.Books.FirstOrDefaultAsync(a => a.BookName == nameBook);

            if(request == null) return null;

           return request.BookName;
        }
    }
}