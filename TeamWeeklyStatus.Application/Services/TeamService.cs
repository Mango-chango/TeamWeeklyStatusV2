using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<Team> GetTeamByIdAsync(int teamId)
        {
            Team team = await _teamRepository.GetTeamByIdAsync(teamId);
            return team;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _teamRepository.GetAllTeamsAsync();
        }

        public async Task<Team> AddTeamAsync(Team newTeamDto)
        {
            var team = new Team
            {
                Name = newTeamDto.Name,
            }; 

            var newTeam = await _teamRepository.AddTeamAsync(team);

            return newTeam;
        }

        public async Task<Team> UpdateTeamAsync(Team team)
        {
            var updatedTeam = await _teamRepository.UpdateTeamAsync(team);

            return updatedTeam;
        }

        public async Task<Team> DeleteTeamAsync(Team team)
        {
            var deletedTeam = await _teamRepository.DeleteTeamAsync(team.Id);

            return deletedTeam;
        }

    }
}
