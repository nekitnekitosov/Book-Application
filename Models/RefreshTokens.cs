namespace Book.Models
{
    public class RefreshTokens
    {
        public int Id {get;set;}
        public int IdUser {get;set;}
        public string RefreshToken {get;set;}
        public bool IsRevoked {get;set;}
        public DateTime ExpiresDate {get;set;}
        public DateTime CreatedAt {get;set;}
        public User User {get;set;}
    }
}