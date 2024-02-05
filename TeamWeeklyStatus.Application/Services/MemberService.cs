using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;
using TeamStatus.Infrastructure.Repositories;

namespace TeamWeeklyStatus.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IRepository<Member> _memberRepository;
        private readonly IMemberRepository _customMemberRepository;

        public MemberService(IRepository<Member> memberRepository, IMemberRepository customMemberRepository)
        {
            _memberRepository = memberRepository;
            _customMemberRepository = customMemberRepository;
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _memberRepository.GetAll();
        }

        public MemberDTO GetMemberById(int memberId)
        {
            Member member = _memberRepository.GetById(memberId);
            return new MemberDTO
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
            };
        }

        public IEnumerable<Team> GetTeams(int memberId)
        {
            return _customMemberRepository.GetTeams(memberId);
        }
    }
}
