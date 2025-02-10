namespace TeamWeeklyStatus.Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public bool? EmailNotificationsEnabled { get; set; } = false;
        public bool? SlackNotificationsEnabled { get; set; } = false;
        public bool? WeekReporterAutomaticAssignment { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public ICollection<TeamMember> TeamMembers { get; set; }
        public TeamAIConfiguration AIConfiguration { get; set; }
    }
}
