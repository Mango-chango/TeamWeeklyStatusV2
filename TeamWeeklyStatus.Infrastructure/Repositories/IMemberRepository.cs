using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;

namespace TeamStatus.Infrastructure.Repositories
{
    public interface IMemberRepository : IRepository<Member>
    {
        //TODO
        //IEnumerable<Member> GetMembersByTeamId(int teamId);

        public IEnumerable<Team> GetTeams(int memberId);
    }
}
