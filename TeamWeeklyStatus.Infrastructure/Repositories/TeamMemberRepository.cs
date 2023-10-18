using Microsoft.EntityFrameworkCore;
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
        Task<TeamMember> ITeamMemberRepository.AddTeamMemberAsync(TeamMember teamMember)
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberRepository.DeleteTeamMemberAsync(int teamId, int memberId)
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberRepository.GetTeamMemberAsync(int teamId, int memberId)
        {
            throw new NotImplementedException();
        }

        Task<List<TeamMember>> ITeamMemberRepository.GetTeamMembersAsync()
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberRepository.UpdateTeamMemberAsync(TeamMember teamMember)
        {
            throw new NotImplementedException();
        }

        public async Task<TeamMember> GetByEmailWithTeamData(string email)
        {
            return await _context.TeamMembers
                .Include(tm => tm.Team)
                .Include(m => m.Member)
                .FirstOrDefaultAsync(m => m.Member.Email == email);
        }
    }
}
