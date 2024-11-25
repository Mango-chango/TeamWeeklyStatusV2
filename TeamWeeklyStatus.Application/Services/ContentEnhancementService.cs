using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces.AI;
using TeamWeeklyStatus.Application.Interfaces.Repositories;

namespace TeamWeeklyStatus.Application.Services
{
    public class ContentEnhancementService
    {
        private readonly IAIContentEnhancerFactory _aiContentEnhancerFactory;
        private readonly ITeamAIConfigurationRepository _configRepository;

        public ContentEnhancementService(
            IAIContentEnhancerFactory aiContentEnhancerFactory,
            ITeamAIConfigurationRepository configRepository)
        {
            _aiContentEnhancerFactory = aiContentEnhancerFactory;
            _configRepository = configRepository;
        }

        public async Task<string> EnhanceContentAsync(int teamId, string content)
        {
            var config = await _configRepository.GetByTeamIdAsync(teamId);
            if (config == null)
            {
                throw new Exception("AI configuration for the team is not found.");
            }

            var enhancer = _aiContentEnhancerFactory.CreateContentEnhancer(config);

            return await enhancer.EnhanceContentAsync(content);
        }
    }
}
