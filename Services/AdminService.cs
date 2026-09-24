using Book.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Book
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public async Task<List<ModerationBook>> GetModerationBooks()
        {
            var books = await _adminRepository.GetModerationBooksAsync();

            if(books == null) throw new NotFoundException("На модерации книг нет");

            return books;
        }
    }
}