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
    public class ResolvedTicketsController : ControllerBase
    {
        // Dependency Injection
        private readonly IResolvedTickets _resolvedTickets;

        /// <summary>
        /// UserController constructor
        /// </summary>
        public ResolvedTicketsController(IResolvedTickets resolvedTickets)
        {
            _resolvedTickets = resolvedTickets;
        }

        /// <summary>
        /// Get all resolved tickets
        /// </summary>
        /// <permission> Adminmistrators and Discord Bots.</permission>
        /// <returns>List of all resolved tickets</returns>
        // GET: api/ResolvedTickets
        [Authorize(Roles = "Administrator, Discord")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResolvedTicketDTO>>> GetTickets()
        {
            var list = await _resolvedTickets.GetAllResolvedTickets();
            return Ok(list);
        }

        /// <summary>
        /// Get specific resolved ticket
        /// </summary>
        /// <param name="id">Targeted ticket id.</param>
        /// <permission> Adminmistrators and Discord Bots.</permission>
        /// <returns>Resolved ticket</returns>
        // GET: api/ResolvedTickets/5
        [Authorize(Roles = "Administrator, Discord")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResolvedTicketDTO>> GetSupportTicket(int id)
        {
            var list = await _resolvedTickets.GetAllResolvedTickets();
            // Loop through all the items the list to check for a match.
            foreach (ResolvedTicketDTO item in list)
            {
                if (id == item.Id)
                {
                    var resolvedTicket = await _resolvedTickets.GetResolvedTicket(id);
                    return resolvedTicket;
                }
            }
            return BadRequest($"ID does not match a current resolved ticket. Check with Admin to ensure ticket was not removed.");
        }

        /// <summary>
        /// Update a specific ticket
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
                return BadRequest($"ID does not match a current live ticket. Check if ticket has been resolved.");
            }
            var updatedTicket = await _resolvedTickets.UpdateResolvedTicket(id, supportTicket);
            return Ok(updatedTicket);
        }

        /// <summary>
        /// Delete resolved ticket with input id
        /// </summary>
        /// <param name="id">Targeted ticket id.</param>
        /// <permission> Adminmistrators.</permission>
        /// <returns>No content task response</returns>
        // DELETE: api/LiveTickets/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupportTicket(int id)
        {
            var list = await _resolvedTickets.GetAllResolvedTickets();
            foreach (ResolvedTicketDTO item in list)
            {
                if (id == item.Id)
                {
                    await _resolvedTickets.DeleteResolvedTicket(id);
                    return NoContent();
                }
            }
            return BadRequest($"ID does not match a current live ticket. Check if ticket has been resolved.");
        }
    }
}
