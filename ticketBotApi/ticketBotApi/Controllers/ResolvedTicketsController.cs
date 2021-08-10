using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketBotApi.Data;
using ticketBotApi.Models;
using ticketBotApi.Models.DTOs;
using ticketBotApi.Models.Interfaces;

namespace ticketBotApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResolvedTicketsController : ControllerBase
    {
        private readonly IResolvedTickets _resolvedTickets;

        public ResolvedTicketsController(IResolvedTickets resolvedTickets)
        {
            _resolvedTickets = resolvedTickets;
        }

        // GET: api/LiveTickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResolvedTicketDTO>>> GetTickets()
        {
            var list = await _resolvedTickets.GetAllResolvedTickets();
            return Ok(list);
        }

        // GET: api/LiveTickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResolvedTicketDTO>> GetSupportTicket(int id)
        {
            var resolvedTickets = await _resolvedTickets.GetResolvedTicket(id);
            return resolvedTickets;
        }

        // PUT: api/LiveTickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupportTicket(int id, SupportTicket supportTicket)
        {
            if (id != supportTicket.Id)
            {
                return BadRequest();
            }
            var updatedTicket = await _resolvedTickets.UpdateResolvedTicket(id, supportTicket);
            return Ok(updatedTicket);
        }

        // DELETE: api/LiveTickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupportTicket(int id)
        {
            await _resolvedTickets.DeleteResolvedTicket(id);
            return NoContent();
        }
    }
}
