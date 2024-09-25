namespace TeamWeeklyStatus.WebApi.DTOs
{
    public class SubtaskDTO
    {
        public string SubtaskDescription { get; set; }
    }

    public class DoneThisWeekTaskDTO
    {
        public string TaskDescription { get; set; }
        public List<SubtaskDTO> Subtasks { get; set; } = new List<SubtaskDTO>();
    }

    public class WeeklyStatusDTO
    {
        public int Id { get; set; }
        public DateTime WeekStartDate { get; set; }
        public List<DoneThisWeekTaskDTO> DoneThisWeek { get; set; }
        public List<string> PlanForNextWeek { get; set; }
        public string Blockers { get; set; }
        public List<DateTime> UpcomingPTO { get; set; }
        public int MemberId { get; set; }

    }

    public class WeeklyStatusPostRequest
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public List<DoneThisWeekTaskDTO> DoneThisWeek { get; set; }
        public List<string> PlanForNextWeek { get; set; }
        public string Blockers { get; set; }
        public List<DateTime> UpcomingPTO { get; set; }

        public int TeamId { get; set; }
    }

    public class MemberPostRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
