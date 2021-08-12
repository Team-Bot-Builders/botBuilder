using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketBotApi.Models.DTOs;

namespace ticketBotApi.Models.Interfaces
{
    public interface IResolvedTickets
    {
        // GET ALL RESOLVED TICKETS
        Task<List<ResolvedTicketDTO>> GetAllResolvedTickets();

        // GET SINGLE RESOLVED TICKET
        Task<ResolvedTicketDTO> GetResolvedTicket(int id);


        // UPDATE RESOLVED TICKET
        Task<ResolvedTicketDTO> UpdateResolvedTicket(int id, SupportTicket ticket);

        // DELETE RESOLVED TICKET
        Task DeleteResolvedTicket(int id);
    }
}
