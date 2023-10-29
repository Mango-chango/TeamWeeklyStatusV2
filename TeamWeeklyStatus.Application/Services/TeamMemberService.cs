using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;

namespace TeamWeeklyStatus.Application.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;

        public TeamMemberService(ITeamMemberRepository teamMemberRepository)
        {
            _teamMemberRepository = teamMemberRepository;
        }
        Task<TeamMember> ITeamMemberService.AddTeamMemberAsync(TeamMember teamMember)
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberService.DeleteTeamMemberAsync(int teamId, int memberId)
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberService.GetTeamMemberAsync(int teamId, int memberId)
        {
            throw new NotImplementedException();
        }

        Task<List<TeamMember>> ITeamMemberService.GetTeamMembersAsync()
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberService.UpdateTeamMemberAsync(TeamMember teamMember)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersWithoutCurrentReporter()
        {
            var members = await _teamMemberRepository.GetMembersWithoutCurrentReporter();
            return members.Select(m => new MemberDTO { Id = m.Id, Name = m.Name }).ToList();
        }

        public async Task AssignReporter(int memberId)
        {
            await _teamMemberRepository.AssignReporter(memberId);
        }

    }
}
