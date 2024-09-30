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

        public async Task<TeamMember> UpdateTeamMemberAsync(TeamMemberDTO teamMemberDto)
        {
            var teamMember = await _teamMemberRepository.GetTeamMemberAsync(teamMemberDto.TeamId, teamMemberDto.MemberId);

            if (teamMember == null)
            {
                throw new KeyNotFoundException("Team member not found.");
            }

            teamMember.IsTeamLead = teamMemberDto.IsTeamLead;
            teamMember.IsCurrentWeekReporter = teamMemberDto.IsCurrentWeekReporter;
            teamMember.StartActiveDate = teamMemberDto.StartActiveDate;
            teamMember.EndActiveDate = teamMemberDto.EndActiveDate;

            var updatedTeamMember = await _teamMemberRepository.UpdateTeamMemberAsync(teamMemberDto);
            return updatedTeamMember;
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

        public async Task<TeamMember> GetTeamMemberByEmailWithTeamData(string email)
        {
            var teamMember = await _teamMemberRepository.GetTeamMemberByEmailWithTeamData(email);

            if (teamMember == null)
            {
                throw new KeyNotFoundException("Team member not found.");
            }

            return teamMember;
        }

        public async Task<IEnumerable<MemberDTO>> GetTeamMembersExcludingCurrentReporter(int teamId)
        {
            var members = await _teamMemberRepository.GetTeamMembersExcludingCurrentReporter(teamId);
            return members.Select(m => new MemberDTO { Id = m.Id, Name = m.Name }).ToList();
        }

        public async Task AssignWeekReporter(int teamId, int memberId)
        {
            await _teamMemberRepository.AssignWeekReporter(teamId, memberId);
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


    }
}
