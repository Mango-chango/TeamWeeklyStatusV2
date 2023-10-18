namespace TeamWeeklyStatus.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<WeeklyStatus> WeeklyStatuses { get; set; } = new List<WeeklyStatus>();

        public ICollection<TeamMember> TeamMembers { get; set; }
    }
}
