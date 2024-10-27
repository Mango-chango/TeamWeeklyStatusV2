using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.MongoDB.DataModels;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IMongoCollection<MemberDataModel> _members;
        public MemberRepository(IMongoDatabase database)
        {
            _members = database.GetCollection<MemberDataModel>("Members");
        }
        public async Task<Member> GetMemberByIdAsync(int memberId)
        {
            var filter = Builders<MemberDataModel>.Filter.Eq(m => m.Id, memberId);
            var dataModel = await _members.Find(filter).FirstOrDefaultAsync();
            return dataModel?.ToDomainEntity();
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
            var filter = Builders<MemberDataModel>.Filter.Eq(m => m.Email, email);
            var dataModel = await _members.Find(filter).FirstOrDefaultAsync();
            return dataModel?.ToDomainEntity();
        }

        Task<Member> IMemberRepository.UpdateMemberAsync(Member member)
        {
            throw new NotImplementedException();
        }
    }
}
