using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Infrastructure.Shared
{
    public class JungleAuthResponse
    {
        public UserData User { get; set; }
        public string AccessToken { get; set; }
    }

    public class UserData
    {
        public string Name { get; set; }
        // Include other user properties as needed
    }

}
