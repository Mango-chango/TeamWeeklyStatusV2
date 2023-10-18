namespace TeamWeeklyStatus.Domain.Entities
{
    public class WeeklyStatus
    {
        public int Id { get; set; }
        public DateTime WeekStartDate { get; set; } // representing the start of the week for clarity
        public List<string> DoneThisWeek { get; set; } = new List<string>();
        public List<string> PlanForNextWeek { get; set; } = new List<string>();
        public string Blockers { get; set; }
        public List<DateTime> UpcomingPTO { get; set; } = new List<DateTime>();
        public Member Member { get; set; }
        public int MemberId { get; set; }
    }
}
