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
        private TicketBotDbContext _context;

        public ResolvedTicketService(TicketBotDbContext context)
        {
            _context = context;
        }
        public async Task DeleteResolvedTicket(int id)
        {
            SupportTicket ticket = await _context.Tickets.FindAsync(id);
            _context.Entry(ticket).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<List<ResolvedTicketDTO>> GetAllResolvedTickets()
        {
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
                }).ToListAsync();
        }

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
