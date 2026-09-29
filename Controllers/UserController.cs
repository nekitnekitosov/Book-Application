using System.Security.Claims;
using Book.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetMeUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId)) return Unauthorized("Id пользователя не найдено в токене");

            var user = await _userService.GetMeUser(int.Parse(userId));

            return Ok(new { Username = user.UserName, Role = user.Role, CreatedAt = user.CreatedAt });
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginRequest userLoginRequest)
        {
            var user = await _userService.LoginUser(userLoginRequest);

            if (user == null)
            {
                _logger.LogError("[ERROR] Пользователь не найден");
                throw new NotFoundException("Пользователь не найден!");
            }

            _logger.LogInformation("Пользователь {Username} вошел в аккаунт", user.Username);

            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRequest userRequest)
        {
            var newUser = await _userService.RegisterUser(userRequest);

            if (newUser == null)
            {
                _logger.LogError("[ERROR] Ошибка регистрации пользователя, метод вернул null ");
                throw new NotFoundException("Ошибка!");
            }

            _logger.LogInformation("Зарегистрирован новый аккаунт - {UserName}", newUser.UserName);

            return Ok(newUser);
        }
    }
}
