using Microsoft.AspNetCore.Mvc;
namespace Book
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("books")]
        public async Task<IActionResult> GetModerationBooks()
        {
            return Ok(await _adminService.GetModerationBooks());
        }
    }
}