namespace Book.Models
{
    public interface IBookRepository
    {
        Task<Boook> AddBookAsync(BookRequest bookRequest);
    }
}