
using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IMemberService
    {
        MemberDTO GetMember(int memberId);
        IEnumerable<MemberDTO> GetAllMembersForTeam(int teamId);
    }
}
