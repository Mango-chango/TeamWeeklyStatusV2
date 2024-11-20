using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IJungleAuthenticationProvider
    {
        Task<AuthenticationResult> AuthenticateAsync(string email, string password);
    }

    public interface IGoogleAuthenticationProvider 
    {        
        Task<GoogleAuthenticationResult> ValidateGoogleUser(string idToken);
    }
}
