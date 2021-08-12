using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketBotApi.Models
{
    /// <summary>
    /// The shape of a User in the database, everything is inherited from the IdentityUser
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
    }
}
