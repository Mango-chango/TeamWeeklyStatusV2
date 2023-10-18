using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Services
{
    public class TeamMemberService : ITeamMemberService
    {
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
    }
}
