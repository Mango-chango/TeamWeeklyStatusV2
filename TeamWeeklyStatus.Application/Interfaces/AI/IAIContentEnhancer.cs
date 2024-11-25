using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Application.Interfaces.AI
{
    public interface IAIContentEnhancer
    {
        Task<string> EnhanceContentAsync(string prompt);
    }
}
