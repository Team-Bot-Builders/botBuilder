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
        // Dependency Injection
        private IConfiguration _configuration;
        private SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// JwtTokenService Constructor
        /// </summary>
        public JwtTokenService(IConfiguration config, SignInManager<ApplicationUser> manager)
        {
            _configuration = config;
            _signInManager = manager;
        }

        /// <summary>
        /// Return token validation parameters
        /// </summary>
        /// <param name="configuration">IConfiguration to be used with JWT verrification.</param>
        /// <returns>Token Validation Parameters</returns>
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

        /// <summary>
        /// Generate new security key
        /// </summary>
        /// <param name="configuration">IConfiguration to be used with JWT verrification.</param>
        /// <returns>Token Validation Parameters</returns>
        private static SecurityKey GetSecurityKey(IConfiguration configuration)
        {
            var secret = configuration["JWT:Secret"];
            if(secret == null) { throw new InvalidOperationException("JWT: Secret is missing"); }
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            return new SymmetricSecurityKey(secretBytes);
        }

        /// <summary>
        /// Generate new security key
        /// </summary>
        /// <param name="user">ApplicationUser to receive the token.</param>
        /// <param name="exspiresIn">The number of minutes the token will be valid for.</param>
        /// <returns>Token tied to the user</returns>
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
