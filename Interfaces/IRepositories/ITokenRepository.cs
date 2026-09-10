using Book.Models;

namespace Book
{
    public interface ITokenRepository
    {
        Task AddRefreshTokenAsync(string token, int userId);
        Task RevokeTokenAsync(int UserId);
        Task<User> UpdateRefreshTokenAsync(string refreshToken);
    }
}