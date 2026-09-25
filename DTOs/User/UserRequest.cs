namespace Book
{
        public class UserRequest
        {
                public int UserId { get; set; }
                public int RoleId { get; set; }
                public string UserName { get; set; }
                public string Password { get; set; }
                public DateTime CreatedAt { get; set; }
        }
}