using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Member> GetMemberByIdAsync(int memberId)
        {
            Member member = await _memberRepository.GetMemberByIdAsync(memberId);
            return member;
        }

        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _memberRepository.GetAllMembersAsync();
        }

        public async Task<Member> AddMemberAsync(MemberDTO newMemberDto)
        {
            var member = new Member
            {
                Name = newMemberDto.Name,
                Email = newMemberDto.Email,
                IsAdmin = newMemberDto.IsAdmin
            }; 

            var newMember = await _memberRepository.AddMemberAsync(member);

            return newMember;
        }

        public async Task<Member> UpdateMemberAsync(Member member)
        {
            var updatedMember = await _memberRepository.UpdateMemberAsync(member);

            return updatedMember;
        }

        public async Task<Member> DeleteMemberAsync(Member member)
        {
            var deletedMember = await _memberRepository.DeleteMemberAsync(member.Id);

            return deletedMember;
        }

        public async Task<Member> GetMemberByEmailAsync(string email)
        {
            return await _memberRepository.GetMemberByEmailAsync(email);
        }
    }
}
