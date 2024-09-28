using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface ITeamMemberRepository
    {
        Task<List<TeamMember>> GetTeamMembersAsync();
        Task<TeamMember> GetTeamMemberAsync(int teamId, int memberId);
        Task<TeamMember> AddTeamMemberAsync(TeamMember teamMember);
        Task<TeamMember> UpdateTeamMemberAsync(TeamMember teamMember);
        Task<TeamMember> DeleteTeamMemberAsync(int teamId, int memberId);

        Task<TeamMember> GetByEmailWithTeamData(string email);

        Task<IEnumerable<Member>> GetMembersWithoutCurrentReporter();

        Task AssignReporter(int memberId);

        Task<IEnumerable<TeamMember>> GetAllTeamsByMember(int memberId);
    }
}
