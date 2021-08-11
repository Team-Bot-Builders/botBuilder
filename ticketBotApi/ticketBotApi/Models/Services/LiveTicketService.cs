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
        private TicketBotDbContext _context;

        public LiveTicketService(TicketBotDbContext context)
        {
            _context = context;
        }

        public async Task<LiveTicketDTO> CreateLiveTicket(LiveTicketDTO ticket)
        {
            SupportTicket newTicket = new SupportTicket()
            {
                Created = ticket.Created,
                Requestor = ticket.Requestor,
                Description = ticket.Description,
                Resolved = false
            };
            _context.Entry(newTicket).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return await _context.Tickets
                .Select(ticket => new LiveTicketDTO
                {
                    Id = ticket.Id,
                    Requestor = ticket.Requestor,
                    Description = ticket.Description
                }).FirstOrDefaultAsync(t => t.Id == ticket.Id);
        }

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

        public async Task<SupportTicket> UpdateTicket(int id, SupportTicket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Tickets.FindAsync(id);
        }

        
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

        public async Task DeleteLiveTicket(int id)
        {
            SupportTicket ticket = await _context.Tickets.FindAsync(id);
            _context.Entry(ticket).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

    }
}
