using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces.AI;
using TeamWeeklyStatus.Application.Interfaces.Services;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Shared.Services;
using TeamWeeklyStatus.Infrastructure.Shared.Services.AI;

namespace TeamWeeklyStatus.Infrastructure.Shared.CompositionRoot
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddSharedInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Register HttpClient
            services.AddHttpClient();

            // Register services
            services.AddScoped<IJungleAuthenticationProvider, JungleAuthenticationProvider>();
            services.AddScoped<IGoogleAuthenticationProvider, GoogleAuthenticationProvider>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAIService, AIService>();

            services.AddSingleton<IAIContentEnhancerFactory, AIContentEnhancerFactory>();

            return services;
        }
    }

}
