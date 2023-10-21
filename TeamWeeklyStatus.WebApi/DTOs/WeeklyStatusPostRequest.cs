namespace TeamWeeklyStatus.WebApi.DTOs
{
    public class WeeklyStatusPostRequest
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public DateTime WeekStartDate { get; set; }

        public List<string> DoneThisWeek { get; set; }

        public List<string> PlanForNextWeek { get; set; }

        public string Blockers { get; set; }

        public List<DateTime> UpcomingPTO { get; set; }
    }
}
