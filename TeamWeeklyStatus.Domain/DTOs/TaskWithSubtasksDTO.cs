namespace TeamWeeklyStatus.Domain.DTOs
{
    public class TaskWithSubtasksDTO
    {
        public string TaskDescription { get; set; }
        public List<string> Subtasks { get; set; } = new List<string>();
    }
}
