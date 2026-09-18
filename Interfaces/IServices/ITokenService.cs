namespace Book.Models
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
        Task<UpdateRefreshTokenDto> UpdateRefreshToken(string refreshToken);
    }
}