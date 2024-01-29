namespace TeamWeeklyStatus.Domain.DTOs
{
    public class DoneThisWeekTaskDTO
    {
        public string TaskDescription { get; set; }
        public List<SubtaskDTO> Subtasks { get; set; } = new List<SubtaskDTO>();
    }
}
