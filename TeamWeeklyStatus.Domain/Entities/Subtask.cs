namespace TeamWeeklyStatus.Domain.Entities
{
    public class Subtask
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int TaskId { get; set; }
        public DoneThisWeekTask Task { get; set; }
    }

}
