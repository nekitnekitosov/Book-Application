namespace Book
{
    public class UpdateRefreshTokenDto
    {
        public int UserId {get;set;}
        public string RefreshToken {get;set;}
        public string AccessToken {get;set;}
    }
}