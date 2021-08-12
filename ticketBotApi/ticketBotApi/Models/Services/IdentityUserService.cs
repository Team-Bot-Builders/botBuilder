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

        //Dependency Injections
        private UserManager<ApplicationUser> _userManager;
        private JwtTokenService _tokenService;

        /// <summary>
        /// IdentityUserService Constructor
        /// </summary>
        public IdentityUserService(UserManager<ApplicationUser> manager, JwtTokenService jwtTokenService)
        {
            _userManager = manager;
            _tokenService = jwtTokenService;
        }

        /// <summary>
        /// Create a new User and load them into the database
        /// </summary>
        /// <param name="data">A RegisterUserDTO with all of the information needed to make a new user.</param>
        /// <param name="modelState">A ModelStateDictionary used to handle the errors and aid in processing the user.</param>
        /// <returns> If succesful a UserDTO with the user's info, if not the errors that blocked that process</returns>
        public async Task<UserDTO> Register(RegisterUserDTO data, ModelStateDictionary modelState)
        {
            //Create new ApplicationUser from DTO input
            var user = new ApplicationUser
            {
                UserName = data.Username,
                Email = data.Email
            };
            var result = await _userManager.CreateAsync(user, data.Password);

            //If the user was created successfully add in the input roles and return a UserDTO
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

            //If the user was not created give all of the errors back
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

        /// <summary>
        /// Check the password against the stored user and see if they are in the database
        /// </summary>
        /// <param name="username">The username of the user attempting to login.</param>
        /// <param name="password">The password of the user attempting to login.</param>
        /// <returns>A user DTO if login is succesful, null if not</returns>
        public async Task<UserDTO> Login(string username, string password)
        {
            //Find the user in the database by the username
            var user = await _userManager.FindByNameAsync(username);

            //If the password matches that user return a UserDTO with their info
            if(await _userManager.CheckPasswordAsync(user, password))
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Token = await _tokenService.GetTokenAsync(user, System.TimeSpan.FromMinutes(60)),
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }
            return null;
        }

        /// <summary>
        /// A login service for the discord bot, the login lasts much longer than the regular user login
        /// </summary>
        /// <param name="username">Discord bot username.</param>
        /// <param name="password">Discord bot password.</param>
        /// <returns>Discord bot userDTO if succesful, null if not</returns>
        public async Task<UserDTO> BotLogin(string username, string password)
        {
            //Find the user in the database by the username
            var user = await _userManager.FindByNameAsync(username);

            //If the password matches that user return a UserDTO with their info
            if (await _userManager.CheckPasswordAsync(user, password))
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Token = await _tokenService.GetTokenAsync(user, System.TimeSpan.FromMinutes(300)),
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }
            return null;
        }

        /// <summary>
        /// Retrieve a user from the database
        /// </summary>
        /// <param name="principal">ClaimsPrincipal from System.Security.Claims.</param>
        /// <returns>UserDTO with the users Id and Username</returns>
        public async Task<UserDTO> GetUser(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            return new UserDTO
            {
                Id = user.Id,
                Username = user.UserName
            };
        }

        /// <summary>
        /// Add a role to a user
        /// </summary>
        /// <param name="username">String that is the username of the targeted user.</param>
        /// <param name="role">String that is the name of the role to be added to the targeted user.</param>
        /// <returns>UserDTO with the users Id, Username, and roles</returns>
        public async Task<UserDTO> AddRole(string username, string role)
        {
            //Find the user in the database by the username
            var user = await _userManager.FindByNameAsync(username);

            //Add a role to that user
            await _userManager.AddToRoleAsync(user, role);

            //Return a new DTO with all of the information relevant to the user
            return new UserDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }
    }
}
