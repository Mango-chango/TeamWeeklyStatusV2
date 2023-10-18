using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;

namespace TeamStatus.Infrastructure.Repositories
{
    public interface IMemberRepository : IRepository<Member>
    {
        // Add any member-specific methods here, for example:
        //IEnumerable<Member> GetMembersByTeamId(int teamId);
    }
}
