using Book.Models;

namespace Book.Interfaces
{
    public interface IBookService
    {
        Task<Boook> AddBook(BookRequest bookRequest);
    }
}