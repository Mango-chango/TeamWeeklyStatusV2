using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Infrastructure.Repositories;

namespace TeamWeeklyStatus.Application.Services
{
    public class UserService: IUserService
    {
        private readonly ITeamMemberRepository _repository;

        public UserService(ITeamMemberRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserValidationResultDTO> ValidateUser(string email)
        {
            var member = await _repository.GetByEmailWithTeamData(email);
            if (member == null)
            {
                return new UserValidationResultDTO
                {
                    IsValid = false,
                    ErrorMessage = "Invalid email address."
                };
            }

            var role = member.IsTeamLead == true ? "TeamLead" :
                       member.IsCurrentWeekReporter == true ? "CurrentWeekReporter" :
                       "Normal";

            return new UserValidationResultDTO
            {
                IsValid = true,
                Role = role,
                TeamName = member.Team.Name
            };
        }
    }
}
