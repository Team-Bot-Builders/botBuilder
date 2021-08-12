using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketBotApi.Models.DTOs
{
    /// <summary>
    /// DTO to shape resolved ticket data
    /// </summary>
    public class ResolvedTicketDTO
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Closed { get; set; }
        public string Description { get; set; }
        public string Resolver { get; set; }
        public string Resolution { get; set; }
        public string Requestor { get; set; }
    }
}
