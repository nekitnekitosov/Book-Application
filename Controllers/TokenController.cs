using Book.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book
{
        [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> UpdateToken(string refreshToken)
        {
            var request = await _tokenService.UpdateRefreshToken(refreshToken);

            if(request == null) throw new NotFoundException("Ошибка, пустой ответ");

            return Ok(request);
        }
    }
}