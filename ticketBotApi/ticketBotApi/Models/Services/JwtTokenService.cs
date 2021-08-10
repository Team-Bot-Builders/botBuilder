using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticketBotApi.Models.Services
{
    public class JwtTokenService
    {
        private IConfiguration _configuration;
        private SignInManager<ApplicationUser> _signInManager;

        public JwtTokenService(IConfiguration config, SignInManager<ApplicationUser> manager)
        {
            _configuration = config;
            _signInManager = manager;
        }

        public static TokenValidationParameters GetValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(configuration),

                ValidateIssuer = false,
                ValidateAudience = false,
            };
        }

        private static SecurityKey GetSecurityKey(IConfiguration configuration)
        {
            var secret = configuration["JWT:Secret"];
            if(secret == null) { throw new InvalidOperationException("JWT: Secret is missing"); }
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            return new SymmetricSecurityKey(secretBytes);
        }

        public async Task<string> GetTokenAsync(ApplicationUser user, TimeSpan exspiresIn)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            if(principal == null) { return null; }

            var signingKey = GetSecurityKey(_configuration);
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow + exspiresIn,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                claims: principal.Claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
