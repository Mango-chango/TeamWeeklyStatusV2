using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using Google.Apis.Auth;
using TeamWeeklyStatus.WebApi.DTOs;


namespace TeamWeeklyStatus.Infrastructure.Services
{
    public class GoogleAuthenticationProvider: IGoogleAuthenticationProvider
    {
        private readonly IUserService _userService;

        public GoogleAuthenticationProvider(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GoogleAuthenticationResult> ValidateGoogleUser(string idToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                return await ValidateUserWithPayload(payload);
            }
            catch (InvalidJwtException)
            {
                return InvalidTokenResult();
            }
        }

        private async Task<GoogleAuthenticationResult> ValidateUserWithPayload(GoogleJsonWebSignature.Payload payload)
        {
            if (payload == null)
            {
                return InvalidTokenResult();
            }

            var applicationValidationResult = await _userService.ValidateUser(payload.Email);
            if (applicationValidationResult == null)
            {
                return UserNotFoundResult();
            }

            return new GoogleAuthenticationResult
            {
                Success = true,
                Email = payload.Email,
                MemberId = applicationValidationResult.MemberId,
                MemberName = applicationValidationResult.MemberName,
                IsAdmin = applicationValidationResult.IsAdmin,
                ErrorMessage = string.Empty,
            };
        }

        private static GoogleAuthenticationResult InvalidTokenResult()
        {
            return new GoogleAuthenticationResult
            {
                Success = false,
                ErrorMessage = "Invalid Google token."
            };
        }

        private static GoogleAuthenticationResult UserNotFoundResult()
        {
            return new GoogleAuthenticationResult
            {
                Success = false,
                ErrorMessage = "User not found."
            };
        }

    }
}
