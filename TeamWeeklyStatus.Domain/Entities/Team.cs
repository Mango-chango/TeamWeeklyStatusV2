namespace TeamWeeklyStatus.Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }
    }
}
