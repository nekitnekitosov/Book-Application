using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Book.Models;

namespace Book
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        
        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ModerationBook>> GetModerationBooksAsync()
        {
            return await _context.ModerationBooks.ToListAsync();
        }
    }
}