namespace TeamWeeklyStatus.Domain.DTOs
{
    public class WeeklyStatusDTO
    {
        public int Id { get; set; }
        public DateTime WeekStartDate { get; set; }
        public List<string> DoneThisWeek { get; set; }
        public List<string> PlanForNextWeek { get; set; }
        public string Blockers { get; set; }
        public List<DateTime> UpcomingPTO { get; set; }
        public int MemberId { get; set; }

    }
}
