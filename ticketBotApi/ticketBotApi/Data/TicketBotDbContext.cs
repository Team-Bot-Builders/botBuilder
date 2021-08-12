using Microsoft.AspNetCore.Identity;
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

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            SeedRole(modelbuilder, "Administrator");
            SeedRole(modelbuilder, "Moderator");
            SeedRole(modelbuilder, "Discord");
        }
        private void SeedRole(ModelBuilder modelBuilder, string roleName)
        {
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.Empty.ToString()
            };
            modelBuilder.Entity<IdentityRole>().HasData(role);
        }
    }
}
