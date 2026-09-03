using Book.Models;

namespace Book
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<LoginResponse> LoginUser(UserLoginRequest userLoginRequest)
        {
            if (!await _userRepository.FindUser(userLoginRequest.Username)) throw new NotFoundException("Такого пользователя не существует!");

            var user = await _userRepository.LoginUserAsync(userLoginRequest);

            var token = _tokenService.GenerateJwtToken(user);

            return new LoginResponse
            {
                Token = token,
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