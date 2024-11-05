namespace TeamWeeklyStatus.Application.DTOs
{
    public class TeamMemberGetDTO
    {
        public int? TeamId { get; set; }
        public int? MemberId { get; set; }
        public string? Email { get; set; }
    }
}
