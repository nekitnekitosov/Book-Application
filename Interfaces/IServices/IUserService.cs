namespace Book.Models
{
    public interface IUserService
    {
        Task<User> GetMeUser(int userId);
        Task<LoginResponse> LoginUser(UserLoginRequest userLoginRequest);
        Task<User> RegisterUser(UserRequest userRequest);
    }
}