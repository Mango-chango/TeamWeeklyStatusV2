using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces.Repositories;
using TeamWeeklyStatus.Application.Interfaces.Services;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Services
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IWeeklyStatusRepository _weeklyStatusRepository;


        public TeamMemberService(ITeamMemberRepository teamMemberRepository, ITeamRepository teamRepository, IWeeklyStatusRepository weeklyStatusRepository)
        {
            _teamMemberRepository = teamMemberRepository;
            _teamRepository = teamRepository;
            _weeklyStatusRepository = weeklyStatusRepository;
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
            var allTeams = await _teamMemberRepository.GetAllTeamsByMember(memberId);
            var allTeamEntities = await _teamRepository.GetAllTeamsAsync();

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

        public async Task CurrentWeekReporterAutomaticAssignment()
        {
            var today = DateTime.Today;
            var activeTeams = await GetSuitableTeams();

            foreach (var team in activeTeams)
            {
                var teamId = team.Id;
                var teamActiveMembers = (await _teamMemberRepository.GetAllTeamActiveMembersAsync(teamId))
                    .OrderBy(m => m.MemberId)
                    .ToList();

                if (teamActiveMembers == null || !teamActiveMembers.Any())
                {
                    // No active members to assign
                    continue;
                }

                // Handle the special case where there's only one active member
                if (teamActiveMembers.Count == 1)
                {
                    var soleMember = teamActiveMembers.First();

                    if (soleMember.IsCurrentWeekReporter != true)
                    {
                        // Assign the sole member as the current reporter
                        await _teamMemberRepository.AssignCurrentWeekReporter(teamId, soleMember.MemberId);
                    }
                    // Else, the sole member is already the current reporter; no action needed
                    continue; // Skip to the next team
                }

                var currentReporter = teamActiveMembers.FirstOrDefault(m => m.IsCurrentWeekReporter == true);

                if (currentReporter == null)
                {
                    // No current reporter found; default to the first member
                    currentReporter = teamActiveMembers.First();
                    // Assign the first member as the current reporter
                    await _teamMemberRepository.AssignCurrentWeekReporter(teamId, currentReporter.MemberId);
                    continue; // Skip to the next team
                }

                // Get the index of the current reporter in the ordered list
                int currentIndex = teamActiveMembers.IndexOf(currentReporter);

                // Create a circular list of candidates excluding the current reporter
                var candidateMembers = GetCandidateMembers(teamActiveMembers, currentIndex);

                // Get the Friday of the current week
                DateTime fridayOfCurrentWeek = GetFridayOfCurrentWeek(today);

                int nextReporterMemberId = currentReporter.MemberId; // Default to current reporter
                bool reporterAssigned = false;

                foreach (var member in candidateMembers)
                {
                    // Get the latest WeeklyStatus for this member
                    var latestWeeklyStatus = await _weeklyStatusRepository.GetLatestWeeklyStatusAsync(teamId, member.MemberId);

                    // Check if the member is available (no PTO on the upcoming Friday)
                    bool isAvailable = !(latestWeeklyStatus?.UpcomingPTO?.Contains(fridayOfCurrentWeek) ?? false);

                    if (isAvailable)
                    {
                        // Suitable member found
                        nextReporterMemberId = member.MemberId;
                        reporterAssigned = true;
                        break;
                    }
                }

                // Assign the new reporter if a suitable candidate was found
                if (reporterAssigned && nextReporterMemberId != currentReporter.MemberId)
                {
                    await _teamMemberRepository.AssignCurrentWeekReporter(teamId, nextReporterMemberId);
                }
                // Else, no suitable member found; keep the current reporter
            }
        }

        private async Task<IEnumerable<Team>> GetSuitableTeams()
        {
            var allTeams = await Task.Run(() => _teamRepository.GetAllTeamsAsync());
            var suitableTeams = allTeams
                .Where(t => t.IsActive == true && t.WeekReporterAutomaticAssignment == true)
                .ToList();

            return suitableTeams;
        }

        private List<TeamMember> GetCandidateMembers(List<TeamMember> members, int currentIndex)
        {
            var candidateMembers = new List<TeamMember>();

            // Start from the next member after the current reporter
            for (int i = 1; i < members.Count; i++)
            {
                int index = (currentIndex + i) % members.Count;
                candidateMembers.Add(members[index]);
            }

            return candidateMembers;
        }

        private DateTime GetFridayOfCurrentWeek(DateTime currentDate)
        {
            int daysUntilFriday = ((int)DayOfWeek.Friday - (int)currentDate.DayOfWeek + 7) % 7;
            return currentDate.AddDays(daysUntilFriday);
        }
    }
}
