namespace TeamWeeklyStatus.WebApi.DTOs
{
    public class TeamMemberRequest
    {
        public int? TeamId { get; set; }
        public int? MemberId { get; set; }
        public string? Email { get; set; }
    }
}
