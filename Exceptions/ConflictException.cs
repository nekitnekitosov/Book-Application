using Microsoft.AspNetCore.Http.HttpResults;

namespace Book
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}