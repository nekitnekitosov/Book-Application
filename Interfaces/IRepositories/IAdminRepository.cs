using Book.Models;

namespace Book
{
    public interface IAdminRepository
    {
        Task<List<ModerationBook>> GetModerationBooksAsync();
    }
}