﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IAIService
    {
        Task<string> EnhanceTextAsync(string prompt);
    }
}
