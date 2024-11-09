using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;

namespace TeamWeeklyStatus.Application.Services
{
    public class UserProvisioningService : IUserProvisioningService
    {
        private readonly IMemberRepository _userRepository;
        private readonly IEmailService _notificationService;
        private readonly IConfiguration _configuration;

        public UserProvisioningService(
            IMemberRepository userRepository,
            IEmailService notificationService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _notificationService = notificationService;
            _configuration = configuration;
        }

        public async Task<UserProvisioningResult> ProvisionUserAsync(string newUserEmail)
        {
            var user = await _userRepository.GetMemberByEmailAsync(newUserEmail);
            if (user != null)
            {
                return new UserProvisioningResult
                {
                    IsNewUser = false,
                    RequiresConfiguration = false,
                    Message = "User exists."
                };
            }

            // Add user to the database with a temporary name
            var nameMatch = Regex.Match(newUserEmail, @"^[^@]+");
            var temporaryName = nameMatch.Success ? nameMatch.Value : newUserEmail;

            user = new Member
            {
                Name = temporaryName,
                Email = newUserEmail,
                IsAdmin = false,
            };

            // await _userRepository.AddMemberAsync(user);

            // Send notifications to contacts from appsettings.json 
            var contacts = GetSupportContacts();

            var notificationSubject = _configuration["Notifications:Templates:Email:NewUserMissingConfigurationsSubject"];
            string notificationTemplate = _configuration["Notifications:Templates:Email:NewUserMissingConfigurations"];
            string userAddedMessage = _configuration["Notifications:Templates:OnScreen:UserAdded"];

            foreach (var contact in contacts)
            {
                var notificationMessage = string.Format(notificationTemplate, newUserEmail);
                await _notificationService.SendEmailAsync(contact.Name, contact.Email, "TeamWeeklyStatus", notificationSubject, notificationMessage, null, null);
            }

            return new UserProvisioningResult
            {
                IsNewUser = true,
                RequiresConfiguration = true,
                Message = userAddedMessage ?? "Your user has been added to the application, but some extra settings are required. The following contacts were notified: "
            };
        }

        public IEnumerable<SupportContact> GetSupportContacts()
        {
            return _configuration.GetSection("Support:Contacts").Get<List<SupportContact>>() ?? new List<SupportContact>();
        }
    }


}
