using Microsoft.AspNetCore.JsonPatch;

namespace Book.Models
{
    public interface IBookRepository
    {
        Task<Boook> PutBookAsync(JsonPatchDocument<BookUpdateDto> bookRequest, int idBook);
        Task<PagedResult<GetBookResponse>> GetBooksAsync(int page, int pageSize, BookSortBy bookSortBy, SortDirection sortDirection);
        Task<Boook> AddBookAsync(BookRequest bookRequest);
        Task<bool> DeleteBookAsync(int id);
        Task<string> FindBookAsync(string nameBook);
    }
}