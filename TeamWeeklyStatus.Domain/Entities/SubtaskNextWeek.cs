namespace TeamWeeklyStatus.Domain.Entities
{
    public class SubtaskNextWeek
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int TaskId { get; set; }
        public PlanForNextWeekTask Task { get; set; }
    }

}
