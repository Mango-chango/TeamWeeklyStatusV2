namespace TeamWeeklyStatus.Domain.Entities
{
    public class WeeklyStatus
    {
        public int Id { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string? Blockers { get; set; }
        public List<DateTime>? UpcomingPTO { get; set; } = new List<DateTime>(); // TODO: Consider another child table for this if needed in future
        public Member Member { get; set; }
        public int MemberId { get; set; }
        public Team? Team { get; set; }
        public int? TeamId { get; set; }

        public List<DoneThisWeekTask> DoneThisWeekTasks { get; set; } = new List<DoneThisWeekTask>();
        public List<PlanForNextWeekTask> PlanForNextWeekTasks { get; set; } = new List<PlanForNextWeekTask>();
    }

}
