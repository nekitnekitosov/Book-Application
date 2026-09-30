using Microsoft.AspNetCore.JsonPatch;

namespace Book.Models
{
    public interface IBookRepository
    {
        Task<Boook> PutBookAsync(JsonPatchDocument<BookUpdateDto> bookRequest, int idBook);
        Task<PagedResult<GetBooksResponse>> GetBooksAsync(int page, int pageSize, BookSortBy bookSortBy, SortDirection sortDirection);
        Task<GetBookResponse> GetBookAsync(int bookId);
        Task<Boook> AddBookAsync(BookRequest bookRequest);
        Task<ModerationBook> AddModerationBookAsync(BookRequest bookRequest, int userId);
        Task<bool> DeleteBookAsync(int id);
        Task<string> FindBookAsync(string nameBook);
    }
}