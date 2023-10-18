using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;
using TeamWeeklyStatus.Infrastructure;

namespace TeamStatus.Infrastructure.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(TeamWeeklyStatusContext context) : base(context) { }

        //public IEnumerable<Member> GetMembersByTeamId(int teamId)
        //{
        //    return _context.Members.Where(m => m.TeamId == teamId).ToList();
        //}

        // Implement any other member-specific methods here.
    }
}
