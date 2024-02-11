using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.DTOs;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;

namespace TeamWeeklyStatus.Application.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IRepository<Team> _teamRepository;

        public TeamMemberService(ITeamMemberRepository teamMemberRepository, IRepository<Team> teamRepository)
        {
            _teamMemberRepository = teamMemberRepository;
            _teamRepository = teamRepository;
        }

        Task<TeamMember> ITeamMemberService.AddTeamMemberAsync(TeamMember teamMember)
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberService.DeleteTeamMemberAsync(int teamId, int memberId)
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberService.GetTeamMemberAsync(int teamId, int memberId)
        {
            throw new NotImplementedException();
        }

        Task<List<TeamMember>> ITeamMemberService.GetTeamMembersAsync()
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberService.UpdateTeamMemberAsync(TeamMember teamMember)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersWithoutCurrentReporter()
        {
            var members = await _teamMemberRepository.GetMembersWithoutCurrentReporter();
            return members.Select(m => new MemberDTO { Id = m.Id, Name = m.Name }).ToList();
        }

        public async Task AssignReporter(int memberId)
        {
            await _teamMemberRepository.AssignReporter(memberId);
        }

        public async Task<List<TeamMemberDTO>> GetActiveTeamsByMember(int memberId)
        {
            DateTime today = DateTime.Now;
            var allTeams = _teamMemberRepository.GetAllTeamsByMember(memberId).Result.ToList();
            var allTeamEntities = _teamRepository.GetAll().ToList();

            var activeTeamsDTOs = allTeams
                .Where(tm => tm.EndActiveDate == null || (tm.StartActiveDate <= today && tm.EndActiveDate >= today))
                .Join(allTeamEntities, // The collection of teams to join with
                      tm => tm.TeamId, // Key selector from TeamMember
                      team => team.Id, // Key selector from Team
                      (tm, team) => new TeamMemberDTO // Project into a TeamMemberDTO
                      {
                          TeamId = tm.TeamId,
                          TeamName = team.Name,
                          StartActiveDate = tm.StartActiveDate,
                          EndActiveDate = tm.EndActiveDate,
                          MemberId = tm.MemberId,
                          MemberName = tm.Member?.Name ?? "",
                          IsAdminMember = tm.IsAdminMember,
                          IsCurrentWeekReporter = tm.IsCurrentWeekReporter,
                          IsTeamLead = tm.IsTeamLead
                      })
                .ToList();

            return activeTeamsDTOs;
        }

    }
}
