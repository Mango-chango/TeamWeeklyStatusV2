using Google;
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
    public class AIEngineRepository : IAIEngineRepository
    {
        private readonly TeamWeeklyStatusContext _context;

        public AIEngineRepository(TeamWeeklyStatusContext context)
        {
            _context = context;
        }

        public async Task<List<AIEngine>> GetAIEnginesAsync()
        {
            return await _context.AIEngines.ToListAsync();
        }
    }
}
