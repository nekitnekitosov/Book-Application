using Book.Models;

namespace Book
{
    public interface IAdminService
    {
        Task<List<ModerationBook>> GetModerationBooks();
    }
}