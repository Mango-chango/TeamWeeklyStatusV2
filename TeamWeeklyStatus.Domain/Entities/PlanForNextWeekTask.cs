namespace TeamWeeklyStatus.Domain.Entities
{
    public class PlanForNextWeekTask
    {
        public int Id { get; set; }
        public string TaskDescription { get; set; }

        public int WeeklyStatusId { get; set; }
        public WeeklyStatus WeeklyStatus { get; set; }
    }

}