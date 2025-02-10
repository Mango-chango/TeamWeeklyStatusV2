using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Application.DTOs
{
    public class TeamAIConfigurationDTO
    {
        public int TeamId { get; set; }
        public int AIEngineId { get; set; }

        // Sensitive data
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public string Model { get; set; }

    }
}
