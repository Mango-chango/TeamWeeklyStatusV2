using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;
using TeamWeeklyStatus.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TeamWeeklyStatus.Application.Interfaces.Repositories;

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
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<Member> UpdateMemberAsync(Member member)
        {
            var existingMember = await _context.Members.FindAsync(member.Id);
            if (existingMember == null)
            {
                throw new KeyNotFoundException($"Member with Id {member.Id} not found.");
            }

            existingMember.Name = member.Name;
            existingMember.Email = member.Email;
            existingMember.IsAdmin = member.IsAdmin;

            _context.Members.Update(existingMember);
            await _context.SaveChangesAsync();

            return existingMember;
        }

        public async Task<Member> DeleteMemberAsync(int memberId)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
            {
                throw new KeyNotFoundException($"Member with Id {memberId} not found.");
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return member;
        }

        public async Task<Member> GetMemberByEmailAsync(string email)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Email == email);
        }
    }
}
