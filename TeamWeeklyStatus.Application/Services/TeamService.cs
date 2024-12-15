using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.Entities;
using Org.BouncyCastle.Asn1.Ocsp;
using TeamWeeklyStatus.Application.Exceptions;
using TeamWeeklyStatus.Application.Interfaces.Repositories;
using TeamWeeklyStatus.Application.Interfaces.Services;

namespace TeamWeeklyStatus.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamAIConfigurationRepository _teamAIConfigurationRepository;

        public TeamService(ITeamRepository teamRepository, ITeamAIConfigurationRepository teamAIConfigurationRepository)
        {
            _teamRepository = teamRepository;
            _teamAIConfigurationRepository = teamAIConfigurationRepository;
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

        public async Task<Team> AddTeamAsync(TeamDTO newTeamDto)
        {
            var team = new Team
            {
                Name = newTeamDto.Name,
                Description = newTeamDto.Description,
                EmailNotificationsEnabled = newTeamDto.EmailNotificationsEnabled,
                SlackNotificationsEnabled = newTeamDto.SlackNotificationsEnabled,
                IsActive = newTeamDto.IsActive,
                WeekReporterAutomaticAssignment = newTeamDto.WeekReporterAutomaticAssignment
            };

            var newTeam = await _teamRepository.AddTeamAsync(team);

            return newTeam;
        }

        public async Task<TeamDTO> UpdateTeamAsync(TeamDTO teamDto)
        {
            var existingTeam = await GetTeamByIdAsync(teamDto.Id);
            if (existingTeam == null)
            {
                throw new TeamNotFoundException(teamDto.Id);
            }

            existingTeam.Name = teamDto.Name;
            existingTeam.Description = teamDto.Description;
            existingTeam.EmailNotificationsEnabled = teamDto.EmailNotificationsEnabled;
            existingTeam.SlackNotificationsEnabled = teamDto.SlackNotificationsEnabled;
            existingTeam.IsActive = teamDto.IsActive;
            existingTeam.WeekReporterAutomaticAssignment = teamDto.WeekReporterAutomaticAssignment;

            var existingTeamAIConfiguration = await _teamAIConfigurationRepository.GetByTeamIdAsync(teamDto.Id);
            if (existingTeamAIConfiguration != null)
            {
                existingTeamAIConfiguration.AIEngineId = teamDto.AIConfiguration.AIEngineId;
                existingTeamAIConfiguration.ApiKey = teamDto.AIConfiguration.ApiKey;
                existingTeamAIConfiguration.Model = teamDto.AIConfiguration.Model;
                existingTeamAIConfiguration.ApiUrl = teamDto.AIConfiguration.ApiUrl;

                await _teamAIConfigurationRepository.SaveChangesAsync();
            }

            var updatedTeam = await _teamRepository.UpdateTeamAsync(existingTeam);

            return new TeamDTO
            {
                Id = updatedTeam.Id,
                Name = updatedTeam.Name,
                Description = updatedTeam.Description,
                EmailNotificationsEnabled = updatedTeam.EmailNotificationsEnabled,
                SlackNotificationsEnabled = updatedTeam.SlackNotificationsEnabled,
                IsActive = updatedTeam.IsActive,
                WeekReporterAutomaticAssignment = updatedTeam.WeekReporterAutomaticAssignment,
                AIConfiguration = new TeamAIConfigurationDTO
                {
                    AIEngineId = updatedTeam.AIConfiguration.AIEngineId,
                    ApiKey = updatedTeam.AIConfiguration.ApiKey,
                    Model = updatedTeam.AIConfiguration.Model,
                    ApiUrl = updatedTeam.AIConfiguration.ApiUrl
                }
            };
        }

        public async Task<Team> DeleteTeamAsync(TeamDTO teamDto)
        {
            var existingTeam = await GetTeamByIdAsync(teamDto.Id);
            if (existingTeam == null)
            {
                throw new TeamNotFoundException(teamDto.Id);
            }

            var deletedTeam = await _teamRepository.DeleteTeamAsync(teamDto.Id);

            return deletedTeam;
        }

    }
}
