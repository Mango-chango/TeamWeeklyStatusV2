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
        private readonly IHttpClientFactory _httpClientFactory;

        public AIContentEnhancerFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IAIContentEnhancer CreateContentEnhancer(TeamAIConfiguration config)
        {
            switch (config.AIEngine.AIEngineName)
            {
                case nameof(AIEngineName.OpenAI):
                    var openAIHttpClient = _httpClientFactory.CreateClient();
                    return new OpenAIContentEnhancer(openAIHttpClient, config);
                case nameof(AIEngineName.Gemini):
                    var geminiHttpClient = _httpClientFactory.CreateClient();
                    return new GeminiContentEnhancer(geminiHttpClient, config);
                default:
                    throw new NotSupportedException($"AI Engine '{config.AIEngine.AIEngineName}' is not supported.");
            }
        }
    }
}