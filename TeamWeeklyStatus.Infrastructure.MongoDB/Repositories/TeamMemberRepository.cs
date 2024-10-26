using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.Repositories
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly IMongoCollection<TeamMember> _teamMembers;
        public TeamMemberRepository(IMongoDatabase database)
        {
            _teamMembers = database.GetCollection<TeamMember>("TeamMembers");
        }

        Task<TeamMember> ITeamMemberRepository.AddTeamMemberAsync(TeamMemberDTO teamMember)
        {
            throw new NotImplementedException();
        }

        Task ITeamMemberRepository.AssignCurrentWeekReporter(int teamId, int memberId)
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberRepository.DeleteTeamMemberAsync(TeamMemberDTO teamMember)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TeamMember>> ITeamMemberRepository.GetAllTeamActiveMembersAsync(int teamId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TeamMember>> ITeamMemberRepository.GetAllTeamMembersAsync(int teamId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TeamMember>> GetAllTeamsByMember(int memberId)
        {
            var filter = Builders<TeamMember>.Filter.Eq(tm => tm.MemberId, memberId);
            return await _teamMembers.Find(filter).ToListAsync();
        }

        Task<TeamMember> ITeamMemberRepository.GetTeamMemberAsync(int teamId, int memberId)
        {
            throw new NotImplementedException();
        }

        Task<TeamMember> ITeamMemberRepository.UpdateTeamMemberAsync(TeamMemberDTO teamMember)
        {
            throw new NotImplementedException();
        }
    }
}
