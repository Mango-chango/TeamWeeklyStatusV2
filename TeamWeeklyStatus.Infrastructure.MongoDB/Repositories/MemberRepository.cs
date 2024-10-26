using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IMongoCollection<Member> _members;
        public MemberRepository(IMongoDatabase database)
        {
            _members = database.GetCollection<Member>("Members");
        }
        public async Task<Member> GetMemberByIdAsync(int memberId)
        {
            return await _members.Find(m => m.Id == memberId).FirstOrDefaultAsync();
        }
        Task<Member> IMemberRepository.AddMemberAsync(Member member)
        {
            throw new NotImplementedException();
        }

        Task<Member> IMemberRepository.DeleteMemberAsync(int memberId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Member>> IMemberRepository.GetAllMembersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Member> GetMemberByEmailAsync(string email)
        {
            return await _members.Find(m => m.Email == email).FirstOrDefaultAsync();
        }

        Task<Member> IMemberRepository.UpdateMemberAsync(Member member)
        {
            throw new NotImplementedException();
        }
    }
}
