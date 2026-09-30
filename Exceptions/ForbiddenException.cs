using Microsoft.AspNetCore.Http.HttpResults;

namespace Book
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }
}