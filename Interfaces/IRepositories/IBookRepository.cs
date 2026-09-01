namespace Book.Models
{
    public interface IBookRepository
    {
        Task<Boook> AddBookAsync(BookRequest bookRequest);
        Task<bool> DeleteBookAsync(int id);
        Task<string> FindBookAsync(string nameBook);
    }
}