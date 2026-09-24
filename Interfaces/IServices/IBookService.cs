using Book.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Book.Interfaces
{
    public interface IBookService
    {
        Task<Boook> PutBook(JsonPatchDocument<BookUpdateDto> bookRequest, int bookId);
        Task<PagedResult<GetBooksResponse>> GetBooks(int page, int pageSize, BookSortBy bookSortBy, SortDirection sortDirection);
        Task<GetBookResponse> GetBook(int bookId);
        Task<Boook> AddBook(BookRequest bookRequest, string role, int userId);
        Task<bool> DeleteBook(int id);
    }
}