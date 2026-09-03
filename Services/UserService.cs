using Book.Models;

namespace Book
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> LoginUser(UserLoginRequest userLoginRequest)
        {
            if(!await _userRepository.FindUser(userLoginRequest.Username)) throw new NotFoundException("Такого пользователя не существует!");

            return await _userRepository.LoginUserAsync(userLoginRequest);
        }
        public async Task<User> RegisterUser(UserRequest userRequest)
        {
            if (await _userRepository.FindUser(userRequest.UserName)) throw new ConflictException("С таким именем пользователь уже существует");
            if(userRequest.Password.Length < 8) throw new ValidationException("Пароль не может быть меньше 8 символов");

            return await _userRepository.RegisterUserAsync(userRequest);
        }
    }
}