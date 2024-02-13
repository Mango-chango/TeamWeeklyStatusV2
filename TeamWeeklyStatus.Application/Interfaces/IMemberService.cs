
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IMemberService
    {
        public IEnumerable<Member> GetAllMembers();

        public MemberDTO GetMemberById(int memberId);
    }
}
