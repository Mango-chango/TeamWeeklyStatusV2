using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;

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

        public async Task<TeamMember> GetTeamMemberAsync(int teamId, int memberId)
        {
            return await _teamMemberRepository.GetTeamMemberAsync(teamId, memberId);
        }

        public async Task<IEnumerable<TeamMemberDTO>> GetAllTeamMembersAsync(int teamId)
        {
            var teamMembers = await _teamMemberRepository.GetAllTeamMembersAsync(teamId);

            var teamMemberDTOs = teamMembers.Select(tm => new TeamMemberDTO
            {
                TeamId = tm.TeamId,
                TeamName = tm.Team?.Name,
                MemberId = tm.MemberId,
                MemberName = tm.Member?.Name,
                IsTeamLead = tm.IsTeamLead,
                IsCurrentWeekReporter = tm.IsCurrentWeekReporter,
                StartActiveDate = tm.StartActiveDate,
                EndActiveDate = tm.EndActiveDate
            }).ToList();

            return teamMemberDTOs;
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

            var addedTeamMember = await _teamMemberRepository.AddTeamMemberAsync(teamMemberDto);
            return addedTeamMember;
        }

        public async Task<TeamMemberDTO> UpdateTeamMemberAsync(TeamMemberDTO teamMemberDto)
        {
            var teamMember = await _teamMemberRepository.GetTeamMemberAsync(teamMemberDto.TeamId, teamMemberDto.MemberId);

            if (teamMember == null)
            {
                throw new KeyNotFoundException("Team member not found.");
            }

            var updatedTeamMember = await _teamMemberRepository.UpdateTeamMemberAsync(teamMemberDto);

            var updatedTeamMemberDto = new TeamMemberDTO
            {
                TeamId = updatedTeamMember.TeamId,
                TeamName = updatedTeamMember.Team?.Name,
                MemberId = updatedTeamMember.MemberId,
                MemberName = updatedTeamMember.Member?.Name,
                IsTeamLead = updatedTeamMember.IsTeamLead,
                IsCurrentWeekReporter = updatedTeamMember.IsCurrentWeekReporter,
                StartActiveDate = updatedTeamMember.StartActiveDate,
                EndActiveDate = updatedTeamMember.EndActiveDate
            };

            return updatedTeamMemberDto;
        }

        public async Task<TeamMember> DeleteTeamMemberAsync(TeamMemberDTO teamMemberDto)
        {
            var teamMember = await _teamMemberRepository.GetTeamMemberAsync(teamMemberDto.TeamId, teamMemberDto.MemberId);

            if (teamMember == null)
            {
                throw new KeyNotFoundException("Team member not found.");
            }

            var deletedTeamMember = await _teamMemberRepository.DeleteTeamMemberAsync(teamMemberDto);
            return deletedTeamMember;
        }

        public async Task AssignCurrentWeekReporter(int teamId, int memberId)
        {
            await _teamMemberRepository.AssignCurrentWeekReporter(teamId, memberId);
        }

        public async Task<IEnumerable<TeamMemberDTO>> GetActiveTeamsByMember(int memberId)
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
                          IsCurrentWeekReporter = tm.IsCurrentWeekReporter,
                          IsTeamLead = tm.IsTeamLead
                      })
                .ToList();

            return activeTeamsDTOs;
        }

        public async Task<IEnumerable<TeamMemberDTO>> GetTeamActiveMembers(int teamId)
        {
            DateTime today = DateTime.Now;
            var teamMembers = await _teamMemberRepository.GetAllTeamMembersAsync(teamId);
            var teamMembersDTOs = teamMembers
                .Where(tm => tm.EndActiveDate == null || (tm.StartActiveDate <= today && tm.EndActiveDate >= today))
                .Select(tm => new TeamMemberDTO
                {
                    TeamId = tm.TeamId,
                    TeamName = tm.Team?.Name,
                    MemberId = tm.MemberId,
                    MemberName = tm.Member?.Name,
                    IsTeamLead = tm.IsTeamLead,
                    IsCurrentWeekReporter = tm.IsCurrentWeekReporter,
                    StartActiveDate = tm.StartActiveDate,
                    EndActiveDate = tm.EndActiveDate
                })
                .ToList();

            return teamMembersDTOs;
        }

        Task ITeamMemberService.CurrentWeekReporterAutomaticAssignment()
        {
            throw new NotImplementedException();
        }
    }
}
