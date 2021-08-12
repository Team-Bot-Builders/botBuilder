using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketBotApi.Models.Interfaces;
using ticketBotApi.Models.DTOs;
using ticketBotApi.Models;

namespace ticketBotApi.Controllers
{
    /// <summary>
    /// Controller to handle all of the different methods related to users and their interactions with the Db
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Dependency Injection
        private IUserService _userService;

        /// <summary>
        /// UserController constructor
        /// </summary>
        public UsersController(IUserService userserv)
        {
            _userService = userserv;
        }

        /// <summary>
        /// Register a user with the database
        /// </summary>
        /// <param name="newUser"> A RegisterUserDTO containing all the information needed to create the new user.</param>
        /// <returns>The new user object if succesfully created, or a BadRequest response if that failed</returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterUserDTO newUser)
        {
            var response = await _userService.Register(newUser, this.ModelState);

            if (ModelState.IsValid)
            {
                return response;
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        /// <summary>
        /// Checks if a UserDTO can be linked to an existing user in the databse
        /// </summary>
        /// <param name="data"> A LoginDTO containing the username and password of the user trying to log in.</param>
        /// <returns>The user if the password and username match an exsting record, Unauthorized if not</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO data)
        {
            var user = await _userService.Login(data.Username, data.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            return user;
        }

        /// <summary>
        /// Checks if a UserDTO can be linked to an existing discord bot in the databse
        /// </summary>
        /// <param name="data"> A LoginDTO containing the username and password of the discord bot trying to log in.</param>
        /// <returns>The user if the password and username match an exsting record, Unauthorized if not</returns>
        [HttpPost("botlogin")]
        public async Task<ActionResult<UserDTO>> BotLogin(LoginDTO data)
        {
            var user = await _userService.BotLogin(data.Username, data.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            return user;
        }

        /// <summary>
        /// Retreive the information of the current user
        /// </summary>
        /// <permission cref="System.Security.PermissionSet">Only users that have been authorized.</permission>
        /// <returns>The stored information for the current user from the database</returns>
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> Me()
        {
            return await _userService.GetUser(this.User);
        }

        /// <summary>
        /// Add a user to a specific role
        /// </summary>
        /// <permission cref="System.Security.PermissionSet">Only administrator role users.</permission>
        /// <returns>The updated user information</returns>
        [Authorize(Roles = "Administrator")]
        [HttpPut("/roles/{username}/{role}")]
        public async Task<UserDTO> AddRole(string username, string role)
        {
            return await _userService.AddRole(username, role);
        }
    }
}
