using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Application.Services
{
    public class MemberService : IMemberService
    {
        public MemberDTO GetMember(int memberId)
        {
            // Logic to get a member
            return new MemberDTO(); // Placeholder
        }

        public IEnumerable<MemberDTO> GetAllMembersForTeam(int teamId)
        {
            // Logic to get all members for a team
            return Enumerable.Empty<MemberDTO>(); // Placeholder
        }
    }
}
