using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TeamWeeklyStatus.Infrastructure
{
    public class TeamWeeklyStatusContextFactory : IDesignTimeDbContextFactory<TeamWeeklyStatusContext>
    {
        public TeamWeeklyStatusContext CreateDbContext(string[] args)
        {
            string azureSqlConnection = "Data Source=mangochango.database.windows.net;Initial Catalog=team-weekly-status;User ID=linovallejo;Password=TitiEuropa#24;Connect Timeout=60;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            var optionsBuilder = new DbContextOptionsBuilder<TeamWeeklyStatusContext>();
            optionsBuilder.UseSqlServer(azureSqlConnection);

            return new TeamWeeklyStatusContext(optionsBuilder.Options);
        }
    }
}
