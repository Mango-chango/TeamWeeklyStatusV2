using TeamWeeklyStatus.WebApi.DTOs;

namespace TeamWeeklyStatus.WebApi.Services
{
    public interface IGoogleAuthService
    {
        Task<UserValidationResult> ValidateGoogleUser(string idToken);
    }
}
