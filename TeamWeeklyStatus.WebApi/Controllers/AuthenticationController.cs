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
        private readonly IUserProvisioningService _userProvisioningService;

        public AuthenticationController(IJungleAuthenticationService jungleAuthenticationService,
            IGoogleAuthenticationService googleAuthenticationService,
            IUserProvisioningService userProvisioningService)
        {
            _jungleAuthenticationService = jungleAuthenticationService;
            _googleAuthenticationService = googleAuthenticationService;
            _userProvisioningService = userProvisioningService;
        }

        [HttpPost("JungleLogin")]
        public async Task<IActionResult> Login([FromBody] JungleLoginDTO loginRequest)
        {
            var authResult = await _jungleAuthenticationService.AuthenticateAsync(loginRequest.Email, loginRequest.Password);
            if (authResult == null)
                return Unauthorized("Invalid credentials");

            // NOTE: Leaving this for testing and demo purposes:
            // loginRequest.Email = "nuevochango@mangochango.com";

            var provisioningResult = await _userProvisioningService.ProvisionUserAsync(loginRequest.Email);

            if (provisioningResult.IsNewUser)
            {
                return Ok(new
                {
                    Message = provisioningResult.Message,
                    ContactsNotified = _userProvisioningService.GetSupportContacts()
                });
            }

            return Ok(authResult);
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
