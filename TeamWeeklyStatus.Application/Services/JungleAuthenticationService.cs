using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.WebApi.DTOs;

namespace TeamWeeklyStatus.Application.Services
{
    public class JungleAuthenticationService : IJungleAuthenticationService
    {
        private readonly IJungleAuthenticationProvider _jungleAuthenticationProvider;
        public JungleAuthenticationService(IJungleAuthenticationProvider jungleAuthenticationProvider)
        {
            _jungleAuthenticationProvider = jungleAuthenticationProvider;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string email, string password)
        {
            var result = await _jungleAuthenticationProvider.AuthenticateAsync(email, password);
            return result;
        }
    }

}
