using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Application.Services
{
    public interface IAIContentEnhancer
    {
        Task<string> EnhanceContentAsync(string content);
    }
}
