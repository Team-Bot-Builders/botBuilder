using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuddyBot.DTO
{
    /// <summary>
    /// DTO for users
    /// </summary>
    public class UserDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}
