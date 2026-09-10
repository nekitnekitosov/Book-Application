using Book.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Book
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;

            _tokenService = tokenService;
        }
        public async Task<User> GetMeUser(int userId)
        {
           var user = await _userRepository.GetMeUserAsync(userId);

           if(user == null) throw new NotFoundException("Пользователь не найден");

           return user;
        }
        public async Task<LoginResponse> LoginUser(UserLoginRequest userLoginRequest)
        {
            if (!await _userRepository.FindUser(userLoginRequest.Username)) throw new NotFoundException("Такого пользователя не существует!");

            var user = await _userRepository.LoginUserAsync(userLoginRequest);

            var accessToken = _tokenService.GenerateJwtToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _tokenRepository.AddRefreshTokenAsync(refreshToken, user.UserId);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = user.UserName,
                Role = user.Role
            };
        }
        public async Task<User> RegisterUser(UserRequest userRequest)
        {
            if (await _userRepository.FindUser(userRequest.UserName)) throw new ConflictException("С таким именем пользователь уже существует");
            if (userRequest.Password.Length < 8) throw new ValidationException("Пароль не может быть меньше 8 символов");

            return await _userRepository.RegisterUserAsync(userRequest);
        }
    }
}