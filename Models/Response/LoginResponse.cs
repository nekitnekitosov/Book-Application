namespace Book
{
    public class LoginResponse
    {
        public string AccessToken {get; set;}
        public string RefreshToken {get; set;}
        public string Username {get;set;}
        public string Role {get; set;}
    }
}