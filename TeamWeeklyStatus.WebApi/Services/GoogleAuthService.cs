using Google.Apis.Auth;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.WebApi.DTOs;

namespace TeamWeeklyStatus.WebApi.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IUserService _userService;

        public GoogleAuthService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserValidationResult> ValidateGoogleUser(string idToken)
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

        private async Task<UserValidationResult> ValidateUserWithPayload(GoogleJsonWebSignature.Payload payload)
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

            return new UserValidationResult
            {
                Success = true,
                Email = payload.Email,
                Role = applicationValidationResult.Role,
                TeamName = applicationValidationResult.TeamName,
                MemberId = applicationValidationResult.MemberId,
                MemberName = applicationValidationResult.MemberName,
                IsAdmin = applicationValidationResult.IsAdmin,
                ErrorMessage = string.Empty,
            };
        }

        private static UserValidationResult InvalidTokenResult()
        {
            return new UserValidationResult
            {
                Success = false,
                ErrorMessage = "Invalid Google token."
            };
        }

        private static UserValidationResult UserNotFoundResult()
        {
            return new UserValidationResult
            {
                Success = false,
                ErrorMessage = "User not found."
            };
        }
    }
}
