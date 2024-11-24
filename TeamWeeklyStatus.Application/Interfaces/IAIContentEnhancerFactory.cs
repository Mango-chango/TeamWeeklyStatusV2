using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IAIContentEnhancerFactory
    {
        IAIContentEnhancer CreateContentEnhancer(TeamAIConfiguration config);
    }
}
