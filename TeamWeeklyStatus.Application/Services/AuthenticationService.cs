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
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        public AuthenticationService(IAuthenticationProvider authenticationProvider)
        {
            _authenticationProvider = authenticationProvider;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string email, string password)
        {
            var result = await _authenticationProvider.AuthenticateAsync(email, password);
            return result;
        }

        public async Task<GoogleAuthenticationResult> AuthenticateWithGoogleAsync(string idToken)
        {
            var result = await _authenticationProvider.ValidateGoogleUser(idToken);
            return result;

        }
    }

}
