namespace Book.Models
{
    public interface IBookRepository
    {
        Task<Boook> AddBookAsync(BookRequest bookRequest);
        Task<string> FindBookAsync(string nameBook);
    }
}