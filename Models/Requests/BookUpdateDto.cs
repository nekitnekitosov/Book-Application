namespace Book
{
    public class BookUpdateDto
    {
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string YearOfPublish { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedAt {get; set;}
    }
}