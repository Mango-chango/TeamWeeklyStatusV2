using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces.AI;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Domain.Enums;

namespace TeamWeeklyStatus.Infrastructure.Shared.Services.AI
{
    public class AIContentEnhancerFactory : IAIContentEnhancerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpClient _httpClient;

        public AIContentEnhancerFactory(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            _serviceProvider = serviceProvider;
            _httpClient = httpClient;
        }

        public IAIContentEnhancer CreateContentEnhancer(TeamAIConfiguration config)
        {
            switch (config.AIEngine.AIEngineName)
            {
                case nameof(AIEngineName.OpenAI):
                    return new OpenAIContentEnhancer(_httpClient, config);
                case nameof(AIEngineName.Gemini):
                    return new GeminiContentEnhancer(config);
                default:
                    throw new NotSupportedException($"AI Engine '{config.AIEngine.AIEngineName}' is not supported.");
            }
        }
    }
}