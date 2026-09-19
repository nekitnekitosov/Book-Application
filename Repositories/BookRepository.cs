using Book.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

namespace Book.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<GetBookResponse>> GetBooksAsync(int page, int pageSize, GetBookSortRequest getBookSortRequest)
        {
            var querry = _context.Books.AsQueryable();
            
            querry = (getBookSortRequest.BookSortBy, getBookSortRequest.SortDirection) switch
            {
                (BookSortBy.Name, SortDirection.Asc) => querry.OrderBy(b => b.BookName),
                (BookSortBy.Name, SortDirection.Desc) => querry.OrderByDescending(b => b.BookName),

                (BookSortBy.Year, SortDirection.Asc) => querry.OrderBy(b => b.YearOfPublish),
                (BookSortBy.Year, SortDirection.Desc) => querry.OrderByDescending(b => b.YearOfPublish),

               _ => querry.OrderBy(b => b.BookName) // по умолчанию
            };

            var totalCount = await querry.CountAsync();

            var books = await querry
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new GetBookResponse
                {
                    NameBook = a.BookName,
                    AuthorName = a.AuthorName,
                    YearOfPublish = a.YearOfPublish,
                    Description = a.Description
                })
                .ToListAsync();

            return new PagedResult<GetBookResponse>
            {
                Items = books,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
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
        public async Task<Boook> PutBookAsync(JsonPatchDocument<BookUpdateDto> bookRequest, int idBook)
        {
            var book = await _context.Books.FirstOrDefaultAsync(a => a.BookId == idBook);

            if(book == null) throw new NotFoundException("Книга не найдена!");

            var bookDto = new BookUpdateDto
            {
                BookName = book.BookName,
                YearOfPublish = book.YearOfPublish,
                AuthorName = book.AuthorName,
                Description = book.Description,
                UpdatedAt = DateTime.UtcNow
            };

            bookRequest.ApplyTo(bookDto);
        
            book.BookName = bookDto.BookName;
            book.AuthorName = bookDto.AuthorName;
            book.YearOfPublish = bookDto.YearOfPublish;
            book.Description = bookDto.Description;
            book.UpdatedAt = bookDto.UpdatedAt;

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