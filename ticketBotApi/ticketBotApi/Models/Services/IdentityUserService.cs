using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketBotApi.Models.Interfaces;
using ticketBotApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace ticketBotApi.Models.Services
{
    public class IdentityUserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager;
        private JwtTokenService _tokenService;

        public IdentityUserService(UserManager<ApplicationUser> manager, JwtTokenService jwtTokenService)
        {
            _userManager = manager;
            _tokenService = jwtTokenService;
        }
            
        public async Task<UserDTO> Register(RegisterUserDTO data, ModelStateDictionary modelState)
        {
            var user = new ApplicationUser
            {
                UserName = data.Username,
                Email = data.Email
            };

            var result = await _userManager.CreateAsync(user, data.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, data.Roles);
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Token = await _tokenService.GetTokenAsync(user, System.TimeSpan.FromMinutes(60)),
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }

            foreach (var error in result.Errors)
            {
                var errorKey =
                    error.Code.Contains("Password") ? nameof(data.Password) :
                    error.Code.Contains("Email") ? nameof(data.Email) :
                    error.Code.Contains("UserName") ? nameof(data.Username) :
                    "";
                modelState.AddModelError(errorKey, error.Description);
            }
            return null;
        }

        public async Task<UserDTO> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if(await _userManager.CheckPasswordAsync(user, password))
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Token = await _tokenService.GetTokenAsync(user, System.TimeSpan.FromMinutes(60))
                };
            }
            return null;
        }

        public async Task<UserDTO> GetUser(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            return new UserDTO
            {
                Id = user.Id,
                Username = user.UserName
            };
        }
    }
}
