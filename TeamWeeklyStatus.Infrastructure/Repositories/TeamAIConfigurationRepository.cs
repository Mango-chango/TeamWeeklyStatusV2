using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces.Repositories;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.Repositories
{
    public class TeamAIConfigurationRepository : ITeamAIConfigurationRepository
    {
        private readonly IDataProtector _protector;
        private readonly TeamWeeklyStatusContext _context;

        public TeamAIConfigurationRepository(IDataProtectionProvider dataProtectionProvider, TeamWeeklyStatusContext context)
        {
            _protector = dataProtectionProvider.CreateProtector("AIConfigurationProtector");
            _context = context;
        }
        public async Task<TeamAIConfiguration> GetByTeamIdAsync(int teamId)
        {
            var config = await _context.TeamAIConfigurations
                .Include(c => c.AIEngine)
                .FirstOrDefaultAsync(c => c.TeamId == teamId);

            if (config != null)
            {
                // Decrypt sensitive data
                config.ApiKey = _protector.Unprotect(config.ApiKey);
            }

            return config;
        }

        public async Task SaveAsync(TeamAIConfiguration config)
        {
            config.ApiKey = _protector.Protect(config.ApiKey);

            if (_context.TeamAIConfigurations.Any(c => c.TeamId == config.TeamId))
            {
                _context.TeamAIConfigurations.Update(config);
            }
            else
            {
                _context.TeamAIConfigurations.Add(config);
            }

            await _context.SaveChangesAsync();
        }
    }
}
