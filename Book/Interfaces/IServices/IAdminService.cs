using Book.Models;

namespace Book
{
    public interface IAdminService
    {
        Task<List<ModerationBook>> GetModerationBooks();
        Task<bool> ModerateBook(int bookId, string comment, ModerationStatus moderationStatus);
    }
}