using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketBotApi.Models;

namespace ticketBotApi.Data
{
    public class TicketBotDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<SupportTicket> Tickets { get; set; }

        public TicketBotDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
