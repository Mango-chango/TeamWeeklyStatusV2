namespace TeamWeeklyStatus.WebApi.DTOs
{
    public class TeamPostRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public bool? EmailNotificationsEnabled { get; set; } = false;
        public bool? SlackNotificationsEnabled { get; set; } = false;
    }
}
