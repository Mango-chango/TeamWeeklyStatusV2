namespace TeamWeeklyStatus.Application.DTOs
{
    public class UserValidationResultDTO
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
