using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketBotApi.Models.DTOs;

namespace ticketBotApi.Models.Interfaces
{
    public interface ILiveTickets
    {
        // Create
        Task<LiveTicketDTO> CreateLiveTicket(LiveTicketDTO ticket);

        // GET ALL LIVE TICKETS

        Task<List<LiveTicketDTO>> GetAllLiveTickets();


        Task<LiveTicketDTO> GetLiveTicket(int id);


        // UPDATE
        Task<LiveTicketDTO> UpdateTicket(int id, SupportTicket ticket);

        // DELETE
        Task DeleteLiveTicket(int id);
    }
}
