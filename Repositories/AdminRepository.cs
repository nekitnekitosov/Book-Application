using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Book.Models;

namespace Book
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ModerationBook>> GetModerationBooksAsync()
        {
            return await _context.ModerationBooks.ToListAsync();
        }
        public async Task<bool> ModerateAsync(int bookId, string comment, ModerationStatus moderationStatus)
        {
            var findModerationBook = await _context.ModerationBooks.FirstOrDefaultAsync(a => a.BookId == bookId);

            if (findModerationBook == null) throw new NotFoundException("Такая заявка не найдена");
            if (findModerationBook.Status != ModerationStatus.Pending) throw new ConflictException("Заявка уже обработана");

            if (moderationStatus == ModerationStatus.Accepted)
            {
                bool isDuplicate = await _context.Books.AnyAsync(a =>
                    a.BookName == findModerationBook.BookName &&
                    a.AuthorName == findModerationBook.AuthorName); // если есть такая книга в основной базе то выведет true

                if (isDuplicate) throw new ConflictException("Такая книга уже есть в базе");

                findModerationBook.Status = ModerationStatus.Accepted;
                _context.Books.Add(new Boook
                {
                    BookName = findModerationBook.BookName,
                    AuthorName = findModerationBook.AuthorName,
                    YearOfPublish = findModerationBook.YearOfPublish,
                    Description = findModerationBook.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            if (moderationStatus == ModerationStatus.Rejected)
            {
                findModerationBook.Status = ModerationStatus.Rejected;
                findModerationBook.ModeratorComment = comment;
            }

            await _context.SaveChangesAsync();

            return false;
        }
    }
}