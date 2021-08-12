using Microsoft.Data.Sqlite;
using ticketBotApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ticketBotApi.Models;

namespace TicketAPITests
{
    public class Mock : IDisposable
    {
        public readonly SqliteConnection _connection;
        public readonly TicketBotDbContext _dbContext;

        public Mock()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            _dbContext = new TicketBotDbContext(
                new DbContextOptionsBuilder<TicketBotDbContext>()
                    .UseSqlite(_connection)
                    .Options);

            _dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _connection?.Dispose();
        }

        protected async Task<SupportTicket> CreateTestTicket()
        {
            SupportTicket ticket = new SupportTicket
            {
                Created = DateTime.Now,
                Requestor = "TestRequestor",
                Description = "I need help checking what I can do with a ticket"
            };
            _dbContext.Tickets.Add(ticket);
            await _dbContext.SaveChangesAsync();

            return ticket;
        }
    }
}

