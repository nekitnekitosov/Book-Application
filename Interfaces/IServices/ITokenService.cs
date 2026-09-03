namespace Book.Models
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}