using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.Repositories
{
    public interface IMemberRepository : IRepository<Member>
    {
        //TODO
        //IEnumerable<Member> GetMembersByTeamId(int teamId);

    }
}
