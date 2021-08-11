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
    public class LiveTicketsController : ControllerBase
    {
        private readonly ILiveTickets _liveTickets;

        public LiveTicketsController(ILiveTickets liveTickets)
        {
            _liveTickets = liveTickets;
        }

        // GET: api/LiveTickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiveTicketDTO>>> GetTickets()
        {
            var list = await _liveTickets.GetAllLiveTickets();
            return Ok(list);
        }

        // GET: api/LiveTickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LiveTicketDTO>> GetSupportTicket(int id)
        {
            var liveTicket = await _liveTickets.GetLiveTicket(id);
            return liveTicket;
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
            var updatedTicket = await _liveTickets.UpdateTicket(id, supportTicket);
            return Ok(updatedTicket);
        }

        //PUT: /api/LiveTickets/close/3
        [HttpPut("/close/{id}")]
        public async Task<IActionResult> CloseLiveTicket(int id, CloseTicketDTO closing)
        {
            var updatedTicket = await _liveTickets.CloseTicket(id, closing);
            return Ok(updatedTicket);
        }

        // POST: api/LiveTickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LiveTicketDTO>> PostSupportTicket(LiveTicketDTO liveTicket)
        {
            await _liveTickets.CreateLiveTicket(liveTicket);
            return CreatedAtAction("GetSupportTicket", new { id = liveTicket.Id }, liveTicket);
        }

        // DELETE: api/LiveTickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupportTicket(int id)
        {
            await _liveTickets.DeleteLiveTicket(id);
            return NoContent();
        }
    }
}
