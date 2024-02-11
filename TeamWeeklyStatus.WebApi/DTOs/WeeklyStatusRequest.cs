using System.Text.Json.Serialization;

namespace TeamWeeklyStatus.WebApi.DTOs
{
    public class WeeklyStatusGetRequest
    {
        public int? MemberId { get; set; }

        public int? TeamId { get; set; }

        public DateTime WeekStartDate { get; set; }
    }
}
