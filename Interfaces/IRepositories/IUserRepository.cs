namespace Book.Models
{
    public interface IUserRepository
    {
        Task<User> GetMeUserAsync(int userId);
        Task<User> LoginUserAsync(UserLoginRequest userLoginRequest);
        Task<User> RegisterUserAsync(UserRequest userRequest);
        Task<bool> FindUser(string username);
    }
}