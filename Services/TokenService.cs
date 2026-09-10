using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Book.Models;
using Microsoft.IdentityModel.Tokens;

namespace Book
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;
        public TokenService(IConfiguration configuration, ITokenRepository tokenRepository)
        {
            _configuration = configuration;
            _tokenRepository = tokenRepository;
        }
        public async Task<UpdateRefreshTokenDto> UpdateRefreshToken(string refreshToken)
        {
            if (refreshToken == null) throw new ValidationException("Введите токен. Пустой запрос");

            var receivedUser = await _tokenRepository.UpdateRefreshTokenAsync(refreshToken); // полученный user

            if (receivedUser == null) throw new ValidationException("Ошибка");

            await _tokenRepository.RevokeTokenAsync(receivedUser.UserId); // отзываем токен и записываем в бд

            var newAccessToken = GenerateJwtToken(receivedUser);
            var newRefreshToken = GenerateRefreshToken();

            await _tokenRepository.AddRefreshTokenAsync(newRefreshToken, receivedUser.UserId); // сохранение в бд рефреш токена

            return new UpdateRefreshTokenDto {UserId = receivedUser.UserId, AccessToken = newAccessToken, RefreshToken = newRefreshToken};
        }
        public string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var expireMinutes = Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                     new SymmetricSecurityKey(key),
                     SecurityAlgorithms.HmacSha256Signature
                 )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToHexString(randomNumber);
        }
    }
}