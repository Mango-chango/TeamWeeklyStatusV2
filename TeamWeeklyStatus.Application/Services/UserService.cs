using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces.Repositories;
using TeamWeeklyStatus.Application.Interfaces.Services;

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
            var member = await _memberRepository.GetMemberByEmailAsync(email);
            if (member == null)
            {
                return new UserValidationResultDTO
                {
                    IsValid = false,
                    ErrorMessage = "Email address not found in the system. Please contact the administrator for assistance."
                };
            }

            return new UserValidationResultDTO
            {
                IsValid = true,
                MemberId = member.Id,
                MemberName = member.Name,
                IsAdmin = member.IsAdmin ?? false // Handle nullable boolean
            };
        }
    }
}
