namespace TeamWeeklyStatus.Domain.Entities
{
    public class TeamMember
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public bool? IsTeamLead { get; set; }
        public bool? IsCurrentWeekReporter { get; set; }
        public DateTime? StartActiveDate { get; set; }
        public DateTime? EndActiveDate { get; set; }
    }
}
