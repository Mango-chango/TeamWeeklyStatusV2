using System.Text.Json.Serialization;

namespace TeamWeeklyStatus.Application.DTOs
{
    public class WeeklyStatusGetDTO
    {
        public int? MemberId { get; set; }

        public int? TeamId { get; set; }

        public DateTime WeekStartDate { get; set; }
    }
}
