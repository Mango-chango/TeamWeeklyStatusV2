using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Services;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Domain.Enums;

namespace TeamWeeklyStatus.Infrastructure.Shared.Services.AI
{
    public class AIContentEnhancerFactory : IAIContentEnhancerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AIContentEnhancerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAIContentEnhancer CreateContentEnhancer(TeamAIConfiguration config)
        {
            switch (config.AIEngine.AIEngineName)
            {
                case nameof(AIEngineName.OpenAI):
                    return new OpenAIContentEnhancer(config);
                case nameof(AIEngineName.Gemini):
                    return new GeminiContentEnhancer(config);
                default:
                    throw new NotSupportedException($"AI Engine '{config.AIEngine.AIEngineName}' is not supported.");
            }
        }
    }
}
