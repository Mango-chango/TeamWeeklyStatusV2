using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using TeamWeeklyStatus.Domain.Enums;

namespace TeamWeeklyStatus.Application.Services
{
    public class ReminderService : IReminderService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ReminderService(ITeamRepository teamRepository, IEmailService emailService, IConfiguration configuration)
        {
            _teamRepository = teamRepository;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task SendReminderEmails(EventName eventName)
        {
            // Get all teams where EmailNotificationsEnabled is true
            var teams = await _teamRepository.GetTeamsWithEmailNotificationsEnabled();

            string subject = string.Empty;
            string emailTemplate = string.Empty;
            // Get the email template from appsettings.json based on the event name
            if (eventName == EventName.Post)
            {
                subject = _configuration["Notifications:Templates:Email:PostWeeklyStatusSubject"];
                emailTemplate = _configuration["Notifications:Templates:Email:PostWeeklyStatus"];
            }
            else if (eventName == EventName.SendReport)
            {
                subject = _configuration["Notifications:Templates:Email:SendWeeklyStatusReportSubject"];
                emailTemplate = _configuration["Notifications:Templates:Email:SendWeeklyStatusReport"];
            }

            foreach (var team in teams)
            {
                // Get the team lead
                var teamLead = team.TeamMembers.FirstOrDefault(tm => tm.IsTeamLead == true);

                foreach (var member in team.TeamMembers)
                {
                    if (eventName == EventName.Post)
                        // Send email to each active team member for the event Post
                        await _emailService.SendEmailAsync(member.Member.Name, member.Member.Email, subject, emailTemplate, null, null);

                    // Send emails to the current week reporter and cc to the team lead if the event is SendReport
                    else if (eventName == EventName.SendReport)
                    {
                        var ccEmail = teamLead != null && teamLead.IsCurrentWeekReporter == false;
                        if (member.IsCurrentWeekReporter == true)
                        {
                            await _emailService.SendEmailAsync(member.Member.Name, member.Member.Email, subject, emailTemplate, ccEmail ? teamLead.Member.Name : null, ccEmail ? teamLead.Member.Email : null);
                        }
                    }
                }
            }
        }
    }
}
