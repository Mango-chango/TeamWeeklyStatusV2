using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Application.Exceptions
{
    public class WeeklyStatusNotFoundException : Exception
    {
        public WeeklyStatusNotFoundException(int weeklyStatusId)
            : base($"Team Weekly Status with ID {weeklyStatusId} was not found.")
        {
        }

        public WeeklyStatusNotFoundException(int teamId, DateTime startDate)
            : base($"Team Weekly Status for team {teamId} and week start date {startDate} was not found.")
        {
        }

        public WeeklyStatusNotFoundException(int memberId, int teamId, DateTime startDate)
            : base($"Team Weekly Status for member {memberId}, team {teamId}, and week start date {startDate} was not found.")
        {
        }
    }
}

