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
    public class LiveTicketService : ILiveTickets
    {
        // Dependency Injection
        private TicketBotDbContext _context;

        /// <summary>
        /// LiveTicketService Constructor
        /// </summary>
        public LiveTicketService(TicketBotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a support ticket
        /// </summary>
        /// <param name="ticket">A LiveTicketDTO.</param>
        /// <returns> The ticket stored in the database</returns>
        public async Task<LiveTicketDTO> CreateLiveTicket(LiveTicketDTO ticket)
        {
            //Create a support ticket from the input information
            SupportTicket newTicket = new SupportTicket()
            {
                Created = ticket.Created,
                Requestor = ticket.Requestor,
                Description = ticket.Description,
                Resolved = false
            };

            //Save the ticket in the database
            _context.Entry(newTicket).State = EntityState.Added;
            await _context.SaveChangesAsync();

            //Return the stored ticket
            return await _context.Tickets
                .Select(t => new LiveTicketDTO
                {
                    Id = t.Id,
                    Requestor = t.Requestor,
                    Description = t.Description
                }).FirstOrDefaultAsync(t => t.Description == newTicket.Description);
        }

        /// <summary>
        /// Return all un-closed tickets in the database
        /// </summary>
        /// <returns>List of tickets</returns>
        public async Task<List<LiveTicketDTO>> GetAllLiveTickets()
        {
            return await _context.Tickets
                .Where(ticket => ticket.Resolved == false)
                .Select(ticket => new LiveTicketDTO
                {
                    Id = ticket.Id,
                    Requestor = ticket.Requestor,
                    Description = ticket.Description,
                    Created = ticket.Created
                }).ToListAsync();
        }

        /// <summary>
        /// Retreive a live ticket by that ticket's ID
        /// </summary>
        /// <param name="id">The id of a live ticket.</param>
        /// <returns>The specified ticket</returns>
        public async Task<LiveTicketDTO> GetLiveTicket(int id)
        {
            SupportTicket ticket = await _context.Tickets.FindAsync(id);
            return await _context.Tickets
                .Select(ticket => new LiveTicketDTO
                {
                    Id = ticket.Id,
                    Requestor = ticket.Requestor,
                    Description = ticket.Description,
                    Created = ticket.Created
                }).FirstOrDefaultAsync(t => t.Id == ticket.Id);
        }

        /// <summary>
        /// Update the information stored in a ticket
        /// </summary>
        /// <param name="id">The id of a live ticket.</param>
        /// <param name="ticket">The updated ticket state.</param>
        /// <returns>The specified ticket</returns>
        public async Task<SupportTicket> UpdateTicket(int id, SupportTicket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Tickets.FindAsync(id);
        }

        /// <summary>
        /// Update the status of a ticket
        /// </summary>
        /// <param name="id">The id of a ticket to close.</param>
        /// <param name="closing">The updated ticket state.</param>
        /// <returns>The specified ticket after it has been closed</returns>
        public async Task<ResolvedTicketDTO> CloseTicket(int id, CloseTicketDTO closing)
        {
            SupportTicket thisTicket = await _context.Tickets.FindAsync(id);
            thisTicket.Resolved = true;
            thisTicket.Resolver = closing.Resolver;
            thisTicket.Resolution = closing.Resolution;
            thisTicket.Closed = closing.Closed;

            _context.Entry(thisTicket).State = EntityState.Modified;
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
                }).FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Removes a ticket from the database
        /// </summary>
        /// <param name="id">The id of a ticket to remove.</param>
        public async Task DeleteLiveTicket(int id)
        {
            SupportTicket ticket = await _context.Tickets.FindAsync(id);
            _context.Entry(ticket).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

    }
}
