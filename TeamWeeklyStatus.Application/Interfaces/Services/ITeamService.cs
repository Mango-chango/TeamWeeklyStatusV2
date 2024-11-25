
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Application.DTOs;
using System.Dynamic;

namespace TeamWeeklyStatus.Application.Interfaces.Services
{
    public interface ITeamService
    {
        Task<Team> GetTeamByIdAsync(int teamId);

        Task<IEnumerable<Team>> GetAllTeamsAsync();

        Task<Team> UpdateTeamAsync(TeamDTO team);

        Task<Team> DeleteTeamAsync(TeamDTO team);

        Task<Team> AddTeamAsync(TeamDTO team);

    }
}
