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
            _context.Entry(ticket).State = EntityState.Added;
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

        public async Task<LiveTicketDTO> UpdateTicket(int id, SupportTicket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await _context.Tickets
                .Select(ticket => new LiveTicketDTO
                {
                    Id = ticket.Id,
                    Requestor = ticket.Requestor,
                    Description = ticket.Description,
                    Created = ticket.Created
                }).FirstOrDefaultAsync(t => t.Id == ticket.Id);
        }

        public async Task DeleteLiveTicket(int id)
        {
            SupportTicket ticket = await _context.Tickets.FindAsync(id);
            _context.Entry(ticket).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
