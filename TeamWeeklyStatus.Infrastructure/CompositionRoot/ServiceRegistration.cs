using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamWeeklyStatus.Application.Interfaces.Repositories;
using TeamWeeklyStatus.Infrastructure.Repositories;

namespace TeamWeeklyStatus.Infrastructure.CompositionRoot
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlServerConnectionString = configuration.GetConnectionString("AzureSqlConnection");

            services.AddDbContext<TeamWeeklyStatusContext>(
                options => options.UseSqlServer(sqlServerConnectionString)
            );

            services.AddSingleton<IDesignTimeDbContextFactory<TeamWeeklyStatusContext>,
                TeamWeeklyStatusContextFactory>();

            // Register repositories
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
            services.AddScoped<IWeeklyStatusRepository, WeeklyStatusRepository>();
            services.AddScoped<IWeeklyStatusRichTextRepository, WeeklyStatusRichTextRepository>();
            services.AddScoped<IAIEngineRepository, AIEngineRepository>();
            services.AddScoped<ITeamAIConfigurationRepository, TeamAIConfigurationRepository>();

            return services;
        }
    }

}
