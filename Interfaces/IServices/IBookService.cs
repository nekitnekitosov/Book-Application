using Book.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Book.Interfaces
{
    public interface IBookService
    {
        Task<Boook> PutBook(JsonPatchDocument<BookUpdateDto> bookRequest, int bookId);
        Task<PagedResult<GetBookResponse>> GetBooks(int page, int pageSize, BookSortBy bookSortBy, SortDirection sortDirection);
        Task<Boook> AddBook(BookRequest bookRequest);
        Task<bool> DeleteBook(int id);
    }
}