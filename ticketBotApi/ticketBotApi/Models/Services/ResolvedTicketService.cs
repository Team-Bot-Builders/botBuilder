using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketBotApi.Data;
using ticketBotApi.Models.DTOs;
using ticketBotApi.Models.Interfaces;

namespace ticketBotApi.Models.Services
{
    public class ResolvedTicketService : IResolvedTickets
    {
        //Dependency Injection
        private TicketBotDbContext _context;

        /// <summary>
        /// ResolvedTicketService Constructor
        /// </summary>
        public ResolvedTicketService(TicketBotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Removes a ticket from the database
        /// </summary>
        /// <param name="id">The id of a ticket to remove.</param>
        public async Task DeleteResolvedTicket(int id)
        {
            SupportTicket ticket = await _context.Tickets.FindAsync(id);
            _context.Entry(ticket).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Return all closed tickets in the database
        /// </summary>
        /// <returns>List of all resolved tickets</returns>
        public async Task<List<ResolvedTicketDTO>> GetAllResolvedTickets()
        {
            return await _context.Tickets
                .Where(ticket => ticket.Resolved == true)
                .Select(ticket => new ResolvedTicketDTO
                {
                    Id = ticket.Id,
                    Requestor = ticket.Requestor,
                    Description = ticket.Description,
                    Resolver = ticket.Resolver,
                    Resolution = ticket.Resolution,
                    Created = ticket.Created,
                    Closed = ticket.Closed
                }).ToListAsync();
        }

        /// <summary>
        /// Retreive a resolved ticket by that ticket's ID
        /// </summary>
        /// <param name="id">The id of a resolved ticket.</param>
        /// <returns>The specified ticket</returns>
        public async Task<ResolvedTicketDTO> GetResolvedTicket(int id)
        {
            SupportTicket ticket = await _context.Tickets.FindAsync(id);
            return await _context.Tickets
                .Select(ticket => new ResolvedTicketDTO
                {
                    Id = ticket.Id,
                    Requestor = ticket.Requestor,
                    Description = ticket.Description,
                    Resolver = ticket.Resolver,
                    Resolution = ticket.Resolution,
                    Created = ticket.Created,
                    Closed = ticket.Closed
                }).FirstOrDefaultAsync(t => t.Id == ticket.Id);
        }

        /// <summary>
        /// Update the information stored in a resolved ticket
        /// </summary>
        /// <param name="id">The id of a resolved ticket.</param>
        /// <param name="ticket">The updated ticket state.</param>
        /// <returns>The specified ticket</returns>
        public async Task<ResolvedTicketDTO> UpdateResolvedTicket(int id, SupportTicket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Tickets
                .Select(ticket => new ResolvedTicketDTO
                {
                    Id = ticket.Id,
                    Requestor = ticket.Requestor,
                    Description = ticket.Description,
                    Resolver = ticket.Resolver,
                    Resolution = ticket.Resolution,
                    Created = ticket.Created,
                    Closed = ticket.Closed
                }).FirstOrDefaultAsync(t => t.Id == ticket.Id);
        }
    }
}
