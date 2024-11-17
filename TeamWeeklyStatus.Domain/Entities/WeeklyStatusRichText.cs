using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Domain.Entities
{
    public class WeeklyStatusRichText
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int? TeamId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public string? DoneThisWeekContent { get; set; }
        public string? PlanForNextWeekContent { get; set; }
        public string? Blockers { get; set; }
        public List<DateTime>? UpcomingPTO { get; set; } = new List<DateTime>();
        public DateTime CreatedDate { get; set; }


        // Navigation Properties
        public Member Member { get; set; }

        public Team Team { get; set; }
    }
}
