using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Domain.DTOs
{
    public class WeeklyStatusWithMemberNameDTO
    {
        public string MemberName { get; set; }
        public WeeklyStatus WeeklyStatus { get; set; }
    }
}
