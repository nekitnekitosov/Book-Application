namespace Book
{
    public class ReviewDTO
    {
        public int Id {get;set;}
        public string Username {get;set;}
        public double Rating {get;set;}
        public string Comment {get;set;}
        public DateTime CreatedAt {get;set;}
    }
}