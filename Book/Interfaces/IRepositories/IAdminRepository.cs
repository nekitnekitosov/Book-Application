using Book.Models;

namespace Book
{
    public interface IAdminRepository
    {
        Task<List<ModerationBook>> GetModerationBooksAsync();
        Task<bool> ModerateAsync(int bookId, string comment, ModerationStatus moderationStatus);
    }
}