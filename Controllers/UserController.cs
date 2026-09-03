using Book.Models;
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