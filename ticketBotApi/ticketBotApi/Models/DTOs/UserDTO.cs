using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketBotApi.Models.DTOs
{
    /// <summary>
    /// DTO to shape user data
    /// </summary>
    public class UserDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}
