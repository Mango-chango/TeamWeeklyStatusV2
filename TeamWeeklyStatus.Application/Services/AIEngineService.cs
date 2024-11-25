using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces.AI;
using TeamWeeklyStatus.Application.Interfaces.Repositories;

namespace TeamWeeklyStatus.Application.Services
{
    public class AIEngineService : IAIEngineService
    {
        private readonly IAIEngineRepository _aiEngineRepository;

        public AIEngineService(IAIEngineRepository aiEngineRepository)
        {
            _aiEngineRepository = aiEngineRepository;
        }

        public async Task<List<AIEngineDto>> GetAIEnginesAsync()
        {
            var aiEngines = await _aiEngineRepository.GetAIEnginesAsync();

            var aiEngineDtos = aiEngines.Select(engine => new AIEngineDto
            {
                Id = engine.AIEngineId,
                Name = engine.AIEngineName
            }).ToList();

            return aiEngineDtos;
        }
    }
}
