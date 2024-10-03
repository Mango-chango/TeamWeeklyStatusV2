
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Application.DTOs;
using System.Dynamic;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface ITeamService
    {
        Task<Team> GetTeamByIdAsync(int teamId);

        Task<IEnumerable<Team>> GetAllTeamsAsync();

        Task<Team> UpdateTeamAsync(Team team);

        Task<Team> DeleteTeamAsync(Team team);

        Task<Team> AddTeamAsync(Team team);

    }
}
