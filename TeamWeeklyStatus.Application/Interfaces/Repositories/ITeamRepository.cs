using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Interfaces.Repositories
{
    public interface ITeamRepository
    {
        Task<Team> GetTeamByIdAsync(int memberId);
        Task<IEnumerable<Team>> GetAllTeamsAsync();
        Task<Team> AddTeamAsync(Team member);
        Task<Team> UpdateTeamAsync(Team member);
        Task<Team> DeleteTeamAsync(int memberId);
        Task<IEnumerable<Team>> GetTeamsWithEmailNotificationsEnabled();
        Task<IEnumerable<Team>> GetTeamsWithSlackNotificationsEnabled();
    }
}
