using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Application.DTOs
{
    public class AuthenticationResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Jwt { get; set; }
        public bool IsAdmin { get; set; }
    }

}
