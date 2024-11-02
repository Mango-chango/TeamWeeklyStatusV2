using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeamWeeklyStatus.Application.DTOs;
using TeamWeeklyStatus.Application.Interfaces;
using TeamWeeklyStatus.Domain.Entities;
using TeamWeeklyStatus.WebApi.DTOs;

namespace TeamWeeklyStatus.Infrastructure.Services
{
    public class JungleAuthenticationProvider: IJungleAuthenticationProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMemberRepository _memberRepository;

        public JungleAuthenticationProvider(HttpClient httpClient, IConfiguration configuration, IMemberRepository memberRepository)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _memberRepository = memberRepository;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(string email, string password)
        {
            // Prepare the data for the Jungle authentication service
            var data = new Dictionary<string, string>
        {
            { "client_id", _configuration["JungleAuthSettings:ClientId"] },
            { "client_secret", _configuration["JungleAuthSettings:ClientSecret"] },
            { "scope", "*" },
            { "grant_type", "password" },
            { "username", email },
            { "password", password }
        };

            var requestContent = new FormUrlEncodedContent(data);

            var response = await _httpClient.PostAsync(_configuration["JungleAuthSettings:AuthUrl"], requestContent);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseData = await response.Content.ReadFromJsonAsync<JungleAuthResponse>();

            var member = await _memberRepository.GetMemberByEmailAsync(email);

            var token = GenerateJwtToken(member);

            return new AuthenticationResult
            {
                MemberId = member.Id,
                MemberName = member.Name,
                JwtToken = token,
                IsAdmin = (bool)member.IsAdmin
            };
        }

        private string GenerateJwtToken(Member member)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JungleAuthSettings:JwtSecret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, member.Name),
                    new Claim(ClaimTypes.Email, member.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
