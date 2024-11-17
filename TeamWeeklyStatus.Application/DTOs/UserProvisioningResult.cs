using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Application.DTOs
{
    public class UserProvisioningResult
    {
        public bool IsNewUser { get; set; }
        public bool RequiresConfiguration { get; set; }
        public string Message { get; set; }
    }

}
