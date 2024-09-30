using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface ITeamMemberService
    {
        Task<TeamMember> GetTeamMemberAsync(int teamId, int memberId);
        Task<IEnumerable<TeamMemberDTO>> GetAllTeamMembersAsync(int teamId);
        Task<TeamMember> AddTeamMemberAsync(TeamMemberDTO teamMember);
        Task<TeamMember> UpdateTeamMemberAsync(TeamMemberDTO teamMember);
        Task<TeamMember> DeleteTeamMemberAsync(TeamMemberDTO teamMember);
        Task<TeamMember> GetTeamMemberByEmailWithTeamData(string email);
        Task<IEnumerable<MemberDTO>> GetTeamMembersExcludingCurrentReporter(int teamId);
        Task AssignWeekReporter(int teamId, int memberId);
        Task<IEnumerable<TeamMemberDTO>> GetActiveTeamsByMember(int memberId);

    }
}
