using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Domain.Enums;

namespace TeamWeeklyStatus.Application.Interfaces.Services
{
    public interface IReminderService
    {
        Task SendReminderEmails(EventName eventName);

    }
}
