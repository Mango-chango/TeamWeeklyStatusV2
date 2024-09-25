using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.WebApi.DTOs;

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
        public async Task<IActionResult> ValidateUser([FromBody] EmailRequest request)
        {
            var result = await _userService.ValidateUser(request.Email);

            if (!result.IsValid)
                return NotFound(new { success = false, message = result.ErrorMessage });

            return Ok(new { success = true, role = result.Role, teamName = result.TeamName, memberId = result.MemberId, memberName = result.MemberName, isAdmin = result.IsAdmin });
        }
    }

}
