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
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userserv)
        {
            _userService = userserv;
        }

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

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> Me()
        {
            return await _userService.GetUser(this.User);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("/roles/{username}/{role}")]
        public async Task<UserDTO> AddRole(string username, string role)
        {
            return await _userService.AddRole(username, role);
        }
    }
}
