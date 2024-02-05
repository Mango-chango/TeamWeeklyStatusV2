using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;
using TeamWeeklyStatus.Infrastructure;

namespace TeamStatus.Infrastructure.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(TeamWeeklyStatusContext context) : base(context) { }

        //TODO
        //public IEnumerable<Member> GetMembersByTeamId(int teamId)
        //{
        //    return _context.Members.Where(m => m.TeamId == teamId).ToList();
        //}

        public IEnumerable<Team> GetTeams(int memberId)
        {
            return _context.Teams.Where(t => t.TeamMembers.Any(m => m.MemberId == memberId)).ToList();
        }

    }
}
