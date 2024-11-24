using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;

namespace TeamWeeklyStatus.Application.Interfaces
{
    public interface IGoogleAuthenticationService
    {
        Task<GoogleAuthenticationResult> AuthenticateWithGoogleAsync(string idToken);
    }
}
