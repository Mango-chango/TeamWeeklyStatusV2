using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TeamWeeklyStatus.Domain.Entities;


namespace TeamWeeklyStatus.Infrastructure.MongoDB.DataModels
{
    public class TeamDataModel
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public bool? EmailNotificationsEnabled { get; set; } = false;
        public bool? SlackNotificationsEnabled { get; set; } = false;
        public bool? WeekReporterAutomaticAssignment { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public Team ToDomainEntity()
        {
            return new Team
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                EmailNotificationsEnabled = this.EmailNotificationsEnabled,
                SlackNotificationsEnabled = this.SlackNotificationsEnabled,
                WeekReporterAutomaticAssignment = this.WeekReporterAutomaticAssignment,
                IsActive = this.IsActive,
            };
        }

        public static TeamDataModel FromDomainEntity(Team team)
        {
            return new TeamDataModel
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                EmailNotificationsEnabled = team.EmailNotificationsEnabled,
                SlackNotificationsEnabled = team.SlackNotificationsEnabled,
                WeekReporterAutomaticAssignment = team.WeekReporterAutomaticAssignment,
                IsActive = team.IsActive,
            };
        }


    }
}
