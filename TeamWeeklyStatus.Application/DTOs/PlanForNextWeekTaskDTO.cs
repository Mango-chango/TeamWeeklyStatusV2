namespace TeamWeeklyStatus.Application.DTOs
{
    public class PlanForNextWeekTaskDTO
    {
        public int Id { get; set; }
        public string TaskDescription { get; set; }
        public List<SubtaskNextWeekDTO> Subtasks { get; set; } = new List<SubtaskNextWeekDTO>();

    }

}
