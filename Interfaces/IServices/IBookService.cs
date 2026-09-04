using Book.Models;

namespace Book.Interfaces
{
    public interface IBookService
    {
        Task<PagedResult<GetBookResponse>> GetBooks(int page, int pageSize);
        Task<Boook> AddBook(BookRequest bookRequest);
        Task<bool> DeleteBook(int id);
    }
}