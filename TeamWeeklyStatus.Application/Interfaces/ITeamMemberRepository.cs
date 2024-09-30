using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface ITeamMemberRepository
    {
        Task<TeamMember> GetTeamMemberAsync(int teamId, int memberId);
        Task<IEnumerable<TeamMember>> GetAllTeamMembersAsync(int teamId);
        Task<TeamMember> AddTeamMemberAsync(TeamMemberDTO teamMember);
        Task<TeamMember> UpdateTeamMemberAsync(TeamMemberDTO teamMember);
        Task<TeamMember> DeleteTeamMemberAsync(TeamMemberDTO teamMember);

        Task<TeamMember> GetTeamMemberByEmailWithTeamData(string email);

        Task<IEnumerable<Member>> GetTeamMembersExcludingCurrentReporter(int teamId);

        Task AssignWeekReporter(int teamId, int memberId);

        Task<IEnumerable<TeamMember>> GetAllTeamsByMember(int memberId);
    }
}
