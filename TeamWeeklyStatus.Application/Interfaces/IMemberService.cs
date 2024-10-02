
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Application.DTOs;
using System.Dynamic;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IMemberService
    {
        Task<Member> GetMemberByIdAsync(int memberId);

        Task<IEnumerable<Member>> GetAllMembersAsync();

        Task<Member> UpdateMemberAsync(Member member);

        Task<Member> DeleteMemberAsync(Member member);

        Task<Member> AddMemberAsync(MemberDTO member);

        Task<Member> GetMemberByEmailAsync(string email);

    }
}
