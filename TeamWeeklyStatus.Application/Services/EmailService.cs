using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using TeamWeeklyStatus.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TeamWeeklyStatus.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string recipientName, string recipientEmail, string teamName, string subject, string body, string? ccName, string? ccEmail)
        {
            var message = new MimeMessage();
            var senderEmail = _configuration["Notifications:Configuration:SenderEmail"];
            var senderName = _configuration["Notifications:Configuration:SenderName"];
            var senderPassword = _configuration["Notifications:Configuration:Password"];
            var smtpServer = _configuration["Notifications:Configuration:SmtpServer"];
            var smtpPort = int.Parse(_configuration["Notifications:Configuration:SmtpPort"]);

            message.From.Add(new MailboxAddress(senderName, senderEmail));
            message.To.Add(new MailboxAddress(recipientName, recipientEmail));
            if (!string.IsNullOrEmpty(ccEmail))
            {
                message.Cc.Add(new MailboxAddress(ccName, ccEmail));
            }
            message.Subject = subject;

            body = string.Format(body, recipientName, teamName);

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
                client.Authenticate(senderEmail, senderPassword);

                client.Send(message);
                client.Disconnect(true);
            }

            Console.WriteLine("Email sent successfully!");
        }
    }
}
