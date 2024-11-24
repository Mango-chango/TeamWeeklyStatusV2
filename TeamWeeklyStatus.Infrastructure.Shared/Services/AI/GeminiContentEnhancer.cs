using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Services;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure.Shared.Services.AI
{
    public class GeminiContentEnhancer : IAIContentEnhancer
    {
        private readonly TeamAIConfiguration _config;

        public GeminiContentEnhancer(TeamAIConfiguration config)
        {
            _config = config;
        }

        public async Task<string> EnhanceContentAsync(string content)
        {
            return await Task.FromResult(content);
        }
    }
}
