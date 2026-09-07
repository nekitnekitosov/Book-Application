using System.Security.Claims;
using Book.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Book
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetMeUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(userId)) return Unauthorized("Id пользователя не найдено в токене");

            var user = await _userService.GetMeUser(int.Parse(userId));

            return Ok(new { Username = user.UserName, Role = user.Role, CreatedAt = user.CreatedAt});
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginRequest userLoginRequest)
        {
            var user = await _userService.LoginUser(userLoginRequest);
            
            if(user == null) throw new NotFoundException("Пользователь не найден!");
            
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.RegisterUser(userRequest);

            if(newUser == null) throw new NotFoundException("Ошибка!");

            return Ok(newUser);
        }
    }
}