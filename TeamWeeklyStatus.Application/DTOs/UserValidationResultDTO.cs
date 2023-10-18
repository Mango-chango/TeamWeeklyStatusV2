namespace TeamWeeklyStatus.Application.DTOs
{
    public class UserValidationResultDTO
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public string Role { get; set; }
        public string TeamName { get; set; }
    }
}
