using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.MongoDB.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        Task<Team> ITeamRepository.AddTeamAsync(Team member)
        {
            throw new NotImplementedException();
        }

        Task<Team> ITeamRepository.DeleteTeamAsync(int memberId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Team>> ITeamRepository.GetAllTeamsAsync()
        {
            throw new NotImplementedException();
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
