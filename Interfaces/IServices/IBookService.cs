namespace Book
{
    public interface IBookService
    {
        Task<bool> AddBook(BookRequest bookRequest);
    }
}