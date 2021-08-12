using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System;
using ticketBotApi.Models;
using ticketBotApi.Models.DTOs;
using ticketBotApi.Models.Services;
using Xunit;

namespace TicketAPITests
{
    public class TicketTests : Mock
    {
        [Fact]
        public async void CanCreateTicket()
        {

            LiveTicketService service = new LiveTicketService(_dbContext);

            LiveTicketDTO ticketInput = new LiveTicketDTO()
            {
                Created = DateTime.Now,
                Requestor = "TestRequestor",
                Description = "I need help checking if a new ticket can be made"
            };
            LiveTicketDTO createdTicket = await service.CreateLiveTicket(ticketInput);

            LiveTicketDTO ticketFromTable = await service.GetLiveTicket(createdTicket.Id);

            Assert.Equal("TestRequestor", ticketFromTable.Requestor);

        }
        [Fact]
        public async void CanCloseAndDeleteTicket()
        {
            LiveTicketService service = new LiveTicketService(_dbContext);
            ResolvedTicketService resService = new ResolvedTicketService(_dbContext);
            SupportTicket newTicket = await CreateTestTicket();

            CloseTicketDTO closer = new()
            {
                Closed = DateTime.Now,
                Resolution = "Ran some tests and did some detailed debugging",
                Resolver = "Ed Younskevicius"
            };

            ResolvedTicketDTO result = await service.CloseTicket(1, closer);

            Assert.Equal(newTicket.Requestor, result.Requestor);

            await resService.DeleteResolvedTicket(1);

            ResolvedTicketDTO missing = await resService.GetResolvedTicket(1);

            Assert.Null(missing);
        }
    }
}
