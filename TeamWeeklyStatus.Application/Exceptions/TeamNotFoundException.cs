using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Application.Exceptions
{
    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException(int teamId)
            : base($"Team with ID {teamId} was not found.")
        {
        }
    }
}

