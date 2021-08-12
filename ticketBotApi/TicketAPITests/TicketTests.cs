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
        public async void Test1()
        {

            LiveTicketService service = new LiveTicketService(_dbContext);

            LiveTicketDTO ticketInput = new LiveTicketDTO()
            {
                Created = DateTime.Now,
                Requestor = "TestRequestor",
                Description = "I need help checking if a new ticket can be made"
            };
            LiveTicketDTO createdTicket = await service.CreateLiveTicket(ticketInput);
            
            SqliteCommand newComm = _connection.CreateCommand();
            newComm.CommandText = "SELECT * FROM Tickets";
            var result = newComm.ExecuteReader();
            while(result.Read())
            {
                string value = Convert.ToString(result["Id"]);
                Console.WriteLine("hello");
            }


            LiveTicketDTO ticketFromTable = await service.GetLiveTicket(createdTicket.Id);

            Assert.Equal("TestRequestor", ticketFromTable.Requestor);

        }
        [Fact]
        public async void CanCloseAndDeleteTicket()
        {

        }
        [Fact]
        public async void CanRegisterAndLogin()
        {

        }
        [Fact]
        public async void NoClosesForNonMods()
        {

        }

    }
}
