namespace Book.Models
{
    public class ModerationBook
    {
        public int BookId {get; set;}
        public int UserId {get;set;}
        public User User {get; set;}
        
        public string BookName {get; set;}
        public string AuthorName {get; set;}
        public string YearOfPublish {get; set;}
        public string Description {get; set;}

        public ModerationStatus Status {get;set;}
        public string ModeratorComment {get;set;}

        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
    }
}