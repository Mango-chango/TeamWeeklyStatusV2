using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamWeeklyStatus.Application.CompositionRoot;
using TeamWeeklyStatus.Infrastructure;
using TeamWeeklyStatus.Infrastructure.CompositionRoot;
using TeamWeeklyStatus.Infrastructure.Shared;
using TeamWeeklyStatus.Infrastructure.Shared.CompositionRoot;

namespace TeamWeeklyStatus.CompositionRoot
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCompositionRoot(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Shared Infrastructure Layer services
            services.AddSharedInfrastructureLayer(configuration);

            // Configure Persistence Infrastructure Layer services
            services.AddInfrastructureLayer(configuration);
            //services.AddInfrastructureLayer(configuration);

            // Configure Application Layer services
            services.AddApplicationLayer();

            return services;
        }
    }

}
