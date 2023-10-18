using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserValidationResultDTO> ValidateUser(string email);
    }
}
