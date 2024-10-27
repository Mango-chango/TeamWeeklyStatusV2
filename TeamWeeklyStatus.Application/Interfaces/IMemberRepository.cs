using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetMemberByIdAsync (int memberId);
        Task<IEnumerable<Member>> GetAllMembersAsync();
        Task<Member> AddMemberAsync(Member member);
        Task<Member> UpdateMemberAsync(Member member);
        Task<Member> DeleteMemberAsync(int memberId);
        Task<Member> GetMemberByEmailAsync(string email);

    }
}
