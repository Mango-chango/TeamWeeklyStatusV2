using Microsoft.EntityFrameworkCore;
using System.Collections;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.Repositories
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly TeamWeeklyStatusContext _context;

        public TeamMemberRepository(TeamWeeklyStatusContext context)
        {
            _context = context;
        }
        public async Task<TeamMember> GetTeamMemberAsync(int teamId, int memberId)
        {
            return await _context.TeamMembers
                .Include(tm => tm.Team)
                .Include(m => m.Member)
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.MemberId == memberId);
        }
        public async Task<IEnumerable<TeamMember>> GetAllTeamMembersAsync(int teamId)
        {
            return await _context.TeamMembers
                .Include(tm => tm.Team)
                .Include(m => m.Member)
                .Where(tm => tm.TeamId == teamId)
                .ToListAsync();
        }

        public async Task<TeamMember> AddTeamMemberAsync(TeamMemberDTO teamMemberDto)
        {
            var teamMember = new TeamMember
            {
                TeamId = teamMemberDto.TeamId,
                MemberId = teamMemberDto.MemberId,
                IsTeamLead = teamMemberDto.IsTeamLead,
                IsCurrentWeekReporter = teamMemberDto.IsCurrentWeekReporter,
                StartActiveDate = teamMemberDto.StartActiveDate,
                EndActiveDate = teamMemberDto.EndActiveDate
            };

            _context.TeamMembers.Add(teamMember);
            await _context.SaveChangesAsync();

            return teamMember;
        }

        public async Task<TeamMember> UpdateTeamMemberAsync(TeamMemberDTO teamMemberDto)
        {
            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamMemberDto.TeamId && tm.MemberId == teamMemberDto.MemberId);

            if (teamMember == null)
            {
                throw new KeyNotFoundException("Team member not found.");
            }

            teamMember.IsTeamLead = teamMemberDto.IsTeamLead;
            teamMember.IsCurrentWeekReporter = teamMemberDto.IsCurrentWeekReporter;
            teamMember.StartActiveDate = teamMemberDto.StartActiveDate;
            teamMember.EndActiveDate = teamMemberDto.EndActiveDate;

            _context.TeamMembers.Update(teamMember);
            await _context.SaveChangesAsync();

            return teamMember;
        }

        public async Task<TeamMember> DeleteTeamMemberAsync(TeamMemberDTO teamMemberDto)
        {
            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamMemberDto.TeamId && tm.MemberId == teamMemberDto.MemberId);

            if (teamMember == null)
            {
                throw new KeyNotFoundException("Team member not found.");
            }

            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();

            return teamMember;
        }

        public async Task AssignCurrentWeekReporter(int teamId, int memberId)
        {
            var currentReporter = _context.TeamMembers.FirstOrDefault(tm => tm.TeamId == teamId && tm.IsCurrentWeekReporter == true);
            if (currentReporter != null)
            {
                currentReporter.IsCurrentWeekReporter = false;
            }

            var newReporter = _context.TeamMembers.Single(tm => tm.TeamId == teamId && tm.MemberId == memberId);
            newReporter.IsCurrentWeekReporter = true;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TeamMember>> GetAllTeamsByMember(int memberId)
        {
            return _context.TeamMembers.Where(tm => tm.MemberId == memberId);
        }

        public async Task<IEnumerable<TeamMember>> GetAllTeamActiveMembersAsync(int teamId)
        {
            return await _context.TeamMembers
                .Include(tm => tm.Team)
                .Include(m => m.Member)
                .Where(tm => tm.TeamId == teamId && (tm.EndActiveDate == null || (tm.StartActiveDate <= DateTime.Now && tm.EndActiveDate >= DateTime.Now)))
                .OrderBy(tm => tm.Member.Id)
                .ToListAsync();
        }
    }
}
