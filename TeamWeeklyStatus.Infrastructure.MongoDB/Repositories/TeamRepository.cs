using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.MongoDB.DataModels;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;


namespace TeamWeeklyStatus.Infrastructure.MongoDB.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IMongoCollection<TeamDataModel> _teams;
        public TeamRepository(IMongoDatabase database)
        {
            _teams = database.GetCollection<TeamDataModel>("Teams");
        }

        Task<Team> ITeamRepository.AddTeamAsync(Team member)
        {
            throw new NotImplementedException();
        }

        Task<Team> ITeamRepository.DeleteTeamAsync(int memberId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            var teamDataModels = await _teams.Find(_ => true).ToListAsync();
            return teamDataModels.Select(teamDataModel => teamDataModel.ToDomainEntity());
        }

        Task<Team> ITeamRepository.GetTeamByIdAsync(int memberId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Team>> ITeamRepository.GetTeamsWithEmailNotificationsEnabled()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Team>> ITeamRepository.GetTeamsWithSlackNotificationsEnabled()
        {
            throw new NotImplementedException();
        }

        Task<Team> ITeamRepository.UpdateTeamAsync(Team member)
        {
            throw new NotImplementedException();
        }
    }
}
