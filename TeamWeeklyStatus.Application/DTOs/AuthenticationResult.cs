using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Application.DTOs
{
    public class AuthenticationResult
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string JwtToken { get; set; }
        public bool IsAdmin { get; set; }
    }

}
