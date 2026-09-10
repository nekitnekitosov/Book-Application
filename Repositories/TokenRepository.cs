using Microsoft.EntityFrameworkCore;

namespace Book.Models
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext _context;
        public TokenRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> UpdateRefreshTokenAsync(string refreshToken)
        {
            var oldRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(a => a.RefreshToken == refreshToken);

            if (oldRefreshToken == null) throw new NotFoundException("Старый токен не найден в базе данных");
            if (oldRefreshToken.IsRevoked == true) throw new UnauthorizedException("Токен был отозван");
            if (oldRefreshToken.ExpiresDate < DateTime.UtcNow) throw new UnauthorizedException("Токен просрочен");

            var userId = oldRefreshToken.IdUser; // получили ид пользователя

            var user = await _context.Users.FirstOrDefaultAsync(a => a.UserId == userId);
            if (user == null) throw new NotFoundException("Пользователь не найден");

            return user;
        }
        public async Task AddRefreshTokenAsync(string token, int userId)
        {
            var newToken = new RefreshTokens
            {
                IdUser = userId,
                RefreshToken = token,
                IsRevoked = false,
                ExpiresDate = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow
            };
            _context.RefreshTokens.Add(newToken);

            await _context.SaveChangesAsync();
        }
        public async Task RevokeTokenAsync(int userId)
        {
            var findTokens = await _context.RefreshTokens
                .Where(a => a.IdUser == userId)
                .ToListAsync();

            if(findTokens == null) throw new NotFoundException("Токены не найдены");

            foreach(var token in findTokens)
            {
                token.IsRevoked = true;
            }
            
            await _context.SaveChangesAsync();
        }
        public async Task<int> FindOldRefreshTokenAsync(int userId)
        {
            var token = await _context.RefreshTokens.FirstOrDefaultAsync(a => a.IdUser == userId);

            return token.Id;
        }
    }
}