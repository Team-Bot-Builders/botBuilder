using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ticketBotApi.Models.DTOs;

namespace ticketBotApi.Models.Interfaces
{
    public interface IUserService
    {
        public Task<UserDTO> Register(RegisterUserDTO data, ModelStateDictionary modelState);
        public Task<UserDTO> Login(string username, string password);
        public Task<UserDTO> BotLogin(string username, string password);
        public Task<UserDTO> GetUser(ClaimsPrincipal principal);
        public Task<UserDTO> AddRole(string username, string role);

    }
}
