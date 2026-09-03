using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace Book
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                int statusCode = 0;
                string message = null;

                switch (ex)
                {
                    case NotFoundException e:
                        statusCode = context.Response.StatusCode = StatusCodes.Status404NotFound;
                        message = e.Message;
                        break;
                    case ConflictException e:
                        statusCode = context.Response.StatusCode = StatusCodes.Status409Conflict;
                        message = e.Message;
                        break;
                    case ValidationException e:
                        statusCode = context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        message = e.Message;
                        break;
                    case UnauthorizedException e:
                        statusCode = context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        message = e.Message;
                        break;
                    default:
                        statusCode = context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        message = "Internal server error";
                        break;
                }

                string jsonString = JsonSerializer.Serialize(new { StatusCode = statusCode, Message = message});
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(jsonString);
            }
        }
    }
}