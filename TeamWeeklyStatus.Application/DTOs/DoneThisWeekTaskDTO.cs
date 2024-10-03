namespace TeamWeeklyStatus.Application.DTOs
{
    public class DoneThisWeekTaskDTO
    {
        public int Id { get; set; }
        public string TaskDescription { get; set; }
        public List<SubtaskDTO> Subtasks { get; set; } = new List<SubtaskDTO>();
    }

}
