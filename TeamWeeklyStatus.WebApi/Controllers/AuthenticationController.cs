using Microsoft.AspNetCore.Mvc;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.WebApi.DTOs;

namespace TeamWeeklyStatus.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _authenticationService.AuthenticateAsync(loginRequest.Email, loginRequest.Password);

            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }

        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthDto dto)
        {
            var validationResult = await _authenticationService.AuthenticateWithGoogleAsync(dto.IdToken);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            return Ok(validationResult);
        }

    }
}
