namespace Book.Models
{
    public interface IUserService
    {
        Task<LoginResponse> LoginUser(UserLoginRequest userLoginRequest);
        Task<User> RegisterUser(UserRequest userRequest);
    }
}