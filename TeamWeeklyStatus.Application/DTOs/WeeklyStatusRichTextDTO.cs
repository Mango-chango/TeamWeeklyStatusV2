namespace TeamWeeklyStatus.Application.DTOs
{
    public class WeeklyStatusRichTextDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int TeamId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string? DoneThisWeekContent { get; set; }
        public string? PlanForNextWeekContent { get; set; }
        public string? Blockers { get; set; }
        public List<DateTime> UpcomingPTO { get; set; }

    }
}
