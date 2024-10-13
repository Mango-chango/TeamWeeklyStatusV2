using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface ITeamMemberService
    {
        Task<TeamMember> GetTeamMemberAsync(int teamId, int memberId);
        Task<IEnumerable<TeamMemberDTO>> GetAllTeamMembersAsync(int teamId);
        Task<TeamMember> AddTeamMemberAsync(TeamMemberDTO teamMember);
        Task<TeamMemberDTO> UpdateTeamMemberAsync(TeamMemberDTO teamMember);
        Task<TeamMember> DeleteTeamMemberAsync(TeamMemberDTO teamMember);
        Task AssignCurrentWeekReporter(int teamId, int memberId);
        Task<IEnumerable<TeamMemberDTO>> GetActiveTeamsByMember(int memberId);
        Task<IEnumerable<TeamMemberDTO>> GetTeamActiveMembers(int teamId);

    }
}
