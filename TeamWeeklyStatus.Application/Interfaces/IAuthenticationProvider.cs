using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.WebApi.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IAuthenticationProvider
    {
        Task<AuthenticationResult> AuthenticateAsync(string email, string password);
        Task<GoogleAuthenticationResult> ValidateGoogleUser(string idToken);
    }
}
