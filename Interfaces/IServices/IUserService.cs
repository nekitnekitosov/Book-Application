namespace Book.Models
{
    public interface IUserService
    {
        Task<User> LoginUser(UserLoginRequest userLoginRequest);
        Task<User> RegisterUser(UserRequest userRequest);
    }
}