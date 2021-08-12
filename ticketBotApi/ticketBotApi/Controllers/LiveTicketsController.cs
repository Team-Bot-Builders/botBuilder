using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketBotApi.Data;
using ticketBotApi.Models;
using ticketBotApi.Models.DTOs;
using ticketBotApi.Models.Interfaces;

namespace ticketBotApi.Controllers
{
    /// <summary>
    /// Controller to handle all of the different methods related to users and their interactions with the Db
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LiveTicketsController : ControllerBase
    {
        // Dependency Injection
        private readonly ILiveTickets _liveTickets;

        /// <summary>
        /// LiveTicketsController constructor
        /// </summary>
        public LiveTicketsController(ILiveTickets liveTickets)
        {
            _liveTickets = liveTickets;
        }

        /// <summary>
        /// Get all live tickets
        /// </summary>
        /// <permission> Adminmistrators, Moderators and Discord Bots.</permission>
        /// <returns>List of all live tickets</returns>
        // GET: api/LiveTickets
        [Authorize(Roles ="Administrator, Discord, Moderator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiveTicketDTO>>> GetTickets()
        {
            var list = await _liveTickets.GetAllLiveTickets();
            return Ok(list);
        }

        /// <summary>
        /// Get specific live ticket
        /// </summary>
        /// <param name="id">Targeted ticket id.</param>
        /// <permission> Adminmistrators, Moderators and Discord Bots.</permission>
        /// <returns>Live ticket</returns>
        // GET: api/LiveTickets/5
        [Authorize(Roles = "Administrator, Discord, Moderator")]
        [HttpGet("{id}")]
        public async Task<ActionResult<LiveTicketDTO>> GetSupportTicket(int id)
        {
            
            var list = await _liveTickets.GetAllLiveTickets();
            foreach(LiveTicketDTO item in list)
            {
                if(id == item.Id)
                {
                    var liveTicket = await _liveTickets.GetLiveTicket(id);
                    return liveTicket;
                }
            }
            return BadRequest($"ID does not match a current live ticket. Check if ticket has been resolved.");
        }

        /// <summary>
        /// Update a specific live ticket
        /// </summary>
        /// <param name="id">Targeted ticket id.</param>
        /// <param name="supportTicket">Support Ticket with updated information .</param>
        /// <permission> Adminmistrators.</permission>
        /// <returns>Updated Ticket</returns>
        // PUT: api/LiveTickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupportTicket(int id, SupportTicket supportTicket)
        {
            if (id != supportTicket.Id)
            {
                return BadRequest($"ID does not exist. Please choose a valid index.");
            }
            var updatedTicket = await _liveTickets.UpdateTicket(id, supportTicket);
            return Ok(updatedTicket);
        }

        /// <summary>
        /// Set the state of a live ticket to reolved
        /// </summary>
        /// <param name="id">Targeted ticket id.</param>
        /// <param name="closing">Support Ticket with updated information .</param>
        /// <permission> Adminmistrators.</permission>
        /// <returns>Updated Ticket</returns>
        //PUT: /api/LiveTickets/close/3
        [Authorize(Roles = "Administrator, Discord, Moderator")]
        [HttpPut("close/{id}")]
        public async Task<IActionResult> CloseLiveTicket(int id, CloseTicketDTO closing)
        {
            var list = await _liveTickets.GetAllLiveTickets();
            foreach (LiveTicketDTO item in list)
            {
                if (id == item.Id)
                {
                    var updatedTicket = await _liveTickets.CloseTicket(id, closing);
                    return Ok(updatedTicket);
                }
            }
            return BadRequest($"ID does not match a current live ticket. Check if ticket has been resolved.");
        }

        /// <summary>
        /// Add ticket to the database
        /// </summary>
        /// <param name="liveTicket">Support Ticket with all information .</param>
        /// <returns>Created Ticket</returns>
        // POST: api/LiveTickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LiveTicketDTO>> PostSupportTicket(LiveTicketDTO liveTicket)
        {
            await _liveTickets.CreateLiveTicket(liveTicket);
            return CreatedAtAction("GetSupportTicket", new { id = liveTicket.Id }, liveTicket);
        }

        /// <summary>
        /// Delete live ticket with input id
        /// </summary>
        /// <param name="id">Targeted ticket id.</param>
        /// <permission> Adminmistrators.</permission>
        /// <returns>No content task response</returns>
        // DELETE: api/LiveTickets/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupportTicket(int id)
        {
            var list = await _liveTickets.GetAllLiveTickets();
            foreach (LiveTicketDTO item in list)
            {
                if (id == item.Id)
                {
                    await _liveTickets.DeleteLiveTicket(id);
                    return NoContent();
                }
            }
            return BadRequest($"ID does not match a current live ticket. Check if ticket has been resolved.");
        }
    }
}
