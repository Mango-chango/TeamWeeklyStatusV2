using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.DTOs;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface ITeamMemberService
    {
        Task<List<TeamMember>> GetTeamMembersAsync();
        Task<TeamMember> GetTeamMemberAsync(int teamId, int memberId);
        Task<TeamMember> AddTeamMemberAsync(TeamMember teamMember);
        Task<TeamMember> UpdateTeamMemberAsync(TeamMember teamMember);
        Task<TeamMember> DeleteTeamMemberAsync(int teamId, int memberId);

        Task<IEnumerable<MemberDTO>> GetMembersWithoutCurrentReporter();

        Task AssignReporter(int memberId);

        Task<List<TeamMemberDTO>> GetActiveTeamsByMember(int memberId);

    }
}
