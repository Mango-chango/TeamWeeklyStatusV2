using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces.AI;

namespace TeamWeeklyStatus.Application.Services
{
    public class ContentEnhancementOrchestrationService : IContentEnhancementOrchestrationService
    {
        private readonly IContentEnhancementService _contentEnhancementService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ContentEnhancementOrchestrationService(
            IContentEnhancementService contentEnhancementService,
            IServiceScopeFactory serviceScopeFactory)
        {
            _contentEnhancementService = contentEnhancementService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<EnhancedContentDTO> EnhanceContentAsync(PromptDTO request)
        {
            var enhancementTasks = new List<Task<KeyValuePair<string, string>>>();

            if (!string.IsNullOrWhiteSpace(request.DoneThisWeekContent))
            {
                enhancementTasks.Add(EnhanceContentAsync("DoneThisWeekContent", request.TeamId, request.DoneThisWeekContent));
            }

            if (!string.IsNullOrWhiteSpace(request.PlanForNextWeekContent))
            {
                enhancementTasks.Add(EnhanceContentAsync("PlanForNextWeekContent", request.TeamId, request.PlanForNextWeekContent));
            }

            if (!string.IsNullOrWhiteSpace(request.BlockersContent))
            {
                enhancementTasks.Add(EnhanceContentAsync("BlockersContent", request.TeamId, request.BlockersContent));
            }

            var results = await Task.WhenAll(enhancementTasks);

            var enhancedContent = new EnhancedContentDTO();

            foreach (var result in results)
            {
                switch (result.Key)
                {
                    case "DoneThisWeekContent":
                        enhancedContent.EnhancedDoneThisWeekContent = result.Value;
                        break;
                    case "PlanForNextWeekContent":
                        enhancedContent.EnhancedPlanForNextWeekContent = result.Value;
                        break;
                    case "BlockersContent":
                        enhancedContent.EnhancedBlockersContent = result.Value;
                        break;
                }
            }

            return enhancedContent;
        }

        private async Task<KeyValuePair<string, string>> EnhanceContentAsync(string contentType, int teamId, string content)
        {
            // If using Entity Framework, create a new scope to get a new DbContext
            using var scope = _serviceScopeFactory.CreateScope();
            var contentEnhancementService = scope.ServiceProvider.GetRequiredService<IContentEnhancementService>();

            var enhancedContent = await contentEnhancementService.EnhanceContentAsync(teamId, content);
            return new KeyValuePair<string, string>(contentType, enhancedContent);
        }
    }
}
