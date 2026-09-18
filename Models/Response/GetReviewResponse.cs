namespace Book
{
    public class GetReviewResponse
    {
        public int BookId {get;set;}
        public string BookName {get;set;}
        public string AuthorName {get;set;}
        public string YearOfPublish {get;set;}
        public double AverageRating {get;set;}
        public int ReviewsCount {get;set;}
    }
}