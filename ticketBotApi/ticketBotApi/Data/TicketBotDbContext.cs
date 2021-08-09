using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketBotApi.Data
{
    public class TicketBotDbContext : DbContext 
    {
        public TicketBotDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
