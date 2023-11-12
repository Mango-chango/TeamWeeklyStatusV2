using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.WebApi.DTOs;
using TeamWeeklyStatus.WebApi.Services;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleAuthController : ControllerBase
    {
        private readonly IGoogleAuthService _googleAuthService;

        public GoogleAuthController(IGoogleAuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
        }

        [HttpPost]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthDto dto)
        {
            var validationResult = await _googleAuthService.ValidateGoogleUser(dto.IdToken);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            // TODO: Handle a JWT token for the authenticated user and return it.
            // For now, we're returning the validation result directly.

            return Ok(validationResult);
        }
    }

}
