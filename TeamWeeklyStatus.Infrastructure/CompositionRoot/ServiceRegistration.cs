using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.Infrastructure.Repositories;
using TeamWeeklyStatus.Infrastructure.Services;


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

            // Register HttpClient
            services.AddHttpClient();

            // Register services
            services.AddScoped<IJungleAuthenticationProvider, JungleAuthenticationProvider>();
            services.AddScoped<IGoogleAuthenticationProvider, GoogleAuthenticationProvider>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }

}
