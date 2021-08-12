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

        //Allows get'ing and set'ing of tickets
        public DbSet<SupportTicket> Tickets { get; set; }

        /// <summary>
        /// TicketBotDbContext Constructor
        /// </summary>
        public TicketBotDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Initialize some roles into the database
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            SeedRole(modelbuilder, "Administrator");
            SeedRole(modelbuilder, "Moderator");
            SeedRole(modelbuilder, "Discord");
        }

        /// <summary>
        /// Add a role to the databse
        /// </summary>
        /// <param name="modelBuilder">Context to use.</param>
        /// <param name="roleName">Name of the role to be added.</param>
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
