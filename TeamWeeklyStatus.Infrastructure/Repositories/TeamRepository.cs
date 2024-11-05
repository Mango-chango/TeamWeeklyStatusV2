using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;
using TeamWeeklyStatus.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TeamWeeklyStatus.Application.Interfaces;
using System.Collections;
using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly TeamWeeklyStatusContext _context;
        public TeamRepository(TeamWeeklyStatusContext context)
        {
            _context = context;
        }
        public async Task<Team> GetTeamByIdAsync(int teamId) => await _context.Teams
                .Where(predicate: t => t.Id == teamId)
                .Select(t => new Team
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    EmailNotificationsEnabled = t.EmailNotificationsEnabled,
                    SlackNotificationsEnabled = t.SlackNotificationsEnabled,
                })
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<Team>> GetAllTeamsAsync() => await _context.Teams
                .Select(t => new Team
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    EmailNotificationsEnabled = t.EmailNotificationsEnabled,
                    SlackNotificationsEnabled = t.SlackNotificationsEnabled,
                    WeekReporterAutomaticAssignment = t.WeekReporterAutomaticAssignment,
                    IsActive = t.IsActive
                }).OrderBy(t => t.Name)
                .ToListAsync();

        public async Task<Team> AddTeamAsync(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<Team> UpdateTeamAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();

            return team;
        }

        public async Task<Team> DeleteTeamAsync(int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
            {
                throw new KeyNotFoundException($"Team with Id {teamId} not found.");
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return team;
        }

        public async Task<IEnumerable<Team>> GetTeamsWithEmailNotificationsEnabled()
        {
            return await _context.Teams
                .Where(predicate: t => t.EmailNotificationsEnabled == true)
                .Select(t => new Team
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    EmailNotificationsEnabled = t.EmailNotificationsEnabled,
                    SlackNotificationsEnabled = t.SlackNotificationsEnabled,
                    TeamMembers = t.TeamMembers
                        .Where(tm =>
                            (tm.StartActiveDate == null || tm.StartActiveDate <= DateTime.Now) &&
                            (tm.EndActiveDate == null || tm.EndActiveDate >= DateTime.Now))
                        .Select(tm => new TeamMember
                        {
                            TeamId = tm.TeamId,
                            StartActiveDate = tm.StartActiveDate,
                            EndActiveDate = tm.EndActiveDate,
                            IsTeamLead = tm.IsTeamLead,
                            IsCurrentWeekReporter = tm.IsCurrentWeekReporter,
                            Member = new Member
                            {
                                Id = tm.Member.Id,
                                Name = tm.Member.Name,
                                Email = tm.Member.Email,
                            }
                        }).ToList()
                })
                .ToListAsync();
        }

        public Task<IEnumerable<Team>> GetTeamsWithSlackNotificationsEnabled()
        {
            throw new NotImplementedException();
        }
    }
}
