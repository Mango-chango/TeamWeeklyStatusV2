namespace TeamWeeklyStatus.Application.DTOs
{
    public class GoogleAuthenticationResult
    {
        public bool Success { get; set; }
        public string Email { get; set; }
        public string ErrorMessage { get; set; }
        public string Role { get; set; }
        public string TeamName { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public bool IsAdmin { get; set; }
    }

}
