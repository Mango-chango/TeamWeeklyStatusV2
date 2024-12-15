using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.DTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public bool? EmailNotificationsEnabled { get; set; } = false;
        public bool? SlackNotificationsEnabled { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool? WeekReporterAutomaticAssignment { get; set; } = false;
        public TeamAIConfigurationDTO AIConfiguration { get; set; }

    }
}
