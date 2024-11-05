using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.WebApi.Controllers
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

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateUser([FromBody] EmailDTO request)
        {
            var result = await _userService.ValidateUser(request.Email);

            if (!result.IsValid)
                return NotFound(new { success = false, message = result.ErrorMessage });

            return Ok(new { success = true, memberId = result.MemberId, memberName = result.MemberName, isAdmin = result.IsAdmin });
        }
    }

}
