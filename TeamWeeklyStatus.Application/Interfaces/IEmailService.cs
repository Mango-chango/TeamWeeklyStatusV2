using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string recipientName, string recipientEmail, string subject, string body);

    }
}
