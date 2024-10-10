using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.Interfaces;
using Microsoft.Extensions.Configuration;

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

        public async Task SendReminderEmails(string eventName)
        {
            // Get all teams where EmailNotificationsEnabled is true
            var teams = await _teamRepository.GetTeamsWithEmailNotificationsEnabled();

            string subject = string.Empty;
            string emailTemplate = string.Empty;
            // Get the email template from appsettings.json
            if (eventName == "Post")
            {
                subject = _configuration["EmailTemplates:PostWeeklyStatusSubject"];
                emailTemplate = _configuration["EmailTemplates:PostWeeklyStatus"];
            }

            foreach (var team in teams)
            {
                foreach (var member in team.TeamMembers)
                {
                    // Send email to each team member
                    await _emailService.SendEmailAsync(member.Member.Name, member.Member.Email, subject, emailTemplate);
                }
            }
        }
    }
}
