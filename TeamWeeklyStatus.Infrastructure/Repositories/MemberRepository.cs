using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;
using TeamWeeklyStatus.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TeamWeeklyStatus.Application.Interfaces;

namespace TeamWeeklyStatus.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly TeamWeeklyStatusContext _context;
        public MemberRepository(TeamWeeklyStatusContext context)
        {
            _context = context;
        }
        public async Task<Member> GetMemberByIdAsync(int memberId)
        {
            return await _context.Members
                .Where(m => m.Id == memberId)
                .Select(m => new Member
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    IsAdmin = m.IsAdmin
                })
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _context.Members
                .Select(m => new Member
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    IsAdmin = m.IsAdmin
                }).OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Member> AddMemberAsync(Member member)
        {
            throw new NotImplementedException();
        }

        public async Task<Member> UpdateMemberAsync(Member member)
        {
            throw new NotImplementedException();
        }

        public async Task<Member> DeleteMemberAsync(int memberId)
        {
            throw new NotImplementedException();
        }


    }
}
