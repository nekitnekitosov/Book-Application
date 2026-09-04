namespace Book.Models
{
    public interface IBookRepository
    {
        Task<PagedResult<GetBookResponse>> GetBooksAsync(int page, int pageSize);
        Task<Boook> AddBookAsync(BookRequest bookRequest);
        Task<bool> DeleteBookAsync(int id);
        Task<string> FindBookAsync(string nameBook);
    }
}