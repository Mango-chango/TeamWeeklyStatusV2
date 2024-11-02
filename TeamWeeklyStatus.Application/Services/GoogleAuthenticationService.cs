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
    public class GoogleAuthenticationService : IGoogleAuthenticationService
    {
        private readonly IGoogleAuthenticationProvider _googleAuthenticationProvider;
        public GoogleAuthenticationService(IGoogleAuthenticationProvider googleAuthenticationProvider)
        {
            _googleAuthenticationProvider = googleAuthenticationProvider;
        }

        public async Task<GoogleAuthenticationResult> AuthenticateWithGoogleAsync(string idToken)
        {
            var result = await _googleAuthenticationProvider.ValidateGoogleUser(idToken);
            return result;

        }
    }

}
