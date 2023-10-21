using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Infrastructure
{
    public class TeamWeeklyStatusContext : DbContext
    {
        public TeamWeeklyStatusContext(DbContextOptions<TeamWeeklyStatusContext> options)
            : base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<WeeklyStatus> WeeklyStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamMember>().HasKey(tm => new { tm.TeamId, tm.MemberId });

            modelBuilder
                .Entity<TeamMember>()
                .HasOne(tm => tm.Team)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tm => tm.TeamId);

            modelBuilder
                .Entity<TeamMember>()
                .HasOne(tm => tm.Member)
                .WithMany(m => m.TeamMembers)
                .HasForeignKey(tm => tm.MemberId);

            modelBuilder
                .Entity<WeeklyStatus>()
                .HasOne(ws => ws.Member)
                .WithMany(m => m.WeeklyStatuses)
                .HasForeignKey(ws => ws.MemberId);

            modelBuilder
                .Entity<DoneThisWeekTask>()
                .HasOne(p => p.WeeklyStatus)
                .WithMany(b => b.DoneThisWeekTasks)
                .HasForeignKey(p => p.WeeklyStatusId);

            modelBuilder
                .Entity<WeeklyStatus>()
                .Property(e => e.UpcomingPTO)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<DateTime>>(v, (JsonSerializerOptions)null)
                )
                .Metadata.SetValueComparer(
                    new ValueComparer<List<DateTime>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()
                    )
                );
        }
    }
}
