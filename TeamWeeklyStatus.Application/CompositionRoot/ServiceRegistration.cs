using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Application.Services;

namespace TeamWeeklyStatus.Application.CompositionRoot
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // Register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IReminderService, ReminderService>();
            services.AddScoped<IWeeklyStatusService, WeeklyStatusService>();
            services.AddScoped<ITeamMemberService, TeamMemberService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
