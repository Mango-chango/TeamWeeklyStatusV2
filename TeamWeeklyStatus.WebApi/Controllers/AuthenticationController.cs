using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJungleAuthenticationService _jungleAuthenticationService;
        private readonly IGoogleAuthenticationService _googleAuthenticationService;

        public AuthenticationController(IJungleAuthenticationService jungleAuthenticationService,
            IGoogleAuthenticationService googleAuthenticationService)
        {
            _jungleAuthenticationService = jungleAuthenticationService;
            _googleAuthenticationService = googleAuthenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] JungleLoginDTO loginRequest)
        {
            var result = await _jungleAuthenticationService.AuthenticateAsync(loginRequest.Email, loginRequest.Password);
            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }

        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDTO loginRequest)
        {
            var validationResult = await _googleAuthenticationService.AuthenticateWithGoogleAsync(loginRequest.IdToken);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            return Ok(validationResult);
        }

    }
}
