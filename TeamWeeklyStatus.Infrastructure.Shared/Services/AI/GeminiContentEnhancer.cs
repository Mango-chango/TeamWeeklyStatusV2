using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces.AI;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.Shared.Services.AI
{
    public class GeminiContentEnhancer : IAIContentEnhancer
    {
        private readonly HttpClient _httpClient;
        private readonly TeamAIConfiguration _config;

        public GeminiContentEnhancer(HttpClient httpClient, TeamAIConfiguration config)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<string> EnhanceContentAsync(string prompt)
        {
            return await Task.FromResult(prompt);
        }
    }
}
