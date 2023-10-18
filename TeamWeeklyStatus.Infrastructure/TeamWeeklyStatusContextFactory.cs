using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TeamWeeklyStatus.Infrastructure
{
    public class TeamWeeklyStatusContextFactory : IDesignTimeDbContextFactory<TeamWeeklyStatusContext>
    {
        public TeamWeeklyStatusContext CreateDbContext(string[] args)
        {
            string azureSqlConnection = "conn string here";

            var optionsBuilder = new DbContextOptionsBuilder<TeamWeeklyStatusContext>();
            optionsBuilder.UseSqlServer(azureSqlConnection);

            return new TeamWeeklyStatusContext(optionsBuilder.Options);
        }
    }
}
