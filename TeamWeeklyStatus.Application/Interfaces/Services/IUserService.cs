using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserValidationResultDTO> ValidateUser(string email);
    }
}
