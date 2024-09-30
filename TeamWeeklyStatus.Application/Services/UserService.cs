using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;

namespace TeamWeeklyStatus.Application.Services
{
    public class UserService: IUserService
    {
        private readonly ITeamMemberRepository _repository;
        private readonly IMemberRepository _memberRepository;

        public UserService(ITeamMemberRepository repository, IMemberRepository memberRepository)
        {
            _repository = repository;
            _memberRepository = memberRepository;
        }

        public async Task<UserValidationResultDTO> ValidateUser(string email)
        {
            var teamMember = await _repository.GetTeamMemberByEmailWithTeamData(email);
            if (teamMember == null)
            {
                return new UserValidationResultDTO
                {
                    IsValid = false,
                    ErrorMessage = "Invalid email address."
                };
            }

            var role = teamMember.IsTeamLead == true ? "TeamLead" :
                       teamMember.IsCurrentWeekReporter == true ? "CurrentWeekReporter" :
                       "Normal";

            var member = await _memberRepository.GetMemberByIdAsync(teamMember.MemberId);

            return new UserValidationResultDTO
            {
                IsValid = true,
                Role = role,
                TeamName = teamMember.Team.Name,
                MemberId = teamMember.MemberId,
                MemberName = teamMember.Member.Name,
                IsAdmin = (bool)member.IsAdmin
            };
        }
    }
}
