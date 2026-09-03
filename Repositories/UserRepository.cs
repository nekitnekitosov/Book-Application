using Book.Models;
using Microsoft.EntityFrameworkCore;

namespace Book
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> LoginUserAsync(UserLoginRequest userLoginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userLoginRequest.Username);
            var accessPassword = BCrypt.Net.BCrypt.Verify(userLoginRequest.Password, user.PasswordHash);
            
            if(!accessPassword) throw new UnauthorizedException("Неправильный пароль");
            
            return user;
        }
        public async Task<User> RegisterUserAsync(UserRequest userRequest)
        {
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);

            var newUser = new User
            {
                Role = "User",
                UserName = userRequest.UserName,
                PasswordHash = hashPassword,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();
            
            return newUser;
        }
        public async Task<bool> FindUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == username);

            if(user != null) return true;

            return false;
        }
    }
}