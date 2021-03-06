using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ticketBotApi.Models.DTOs
{
    /// <summary>
    /// DTO to shape user data
    /// </summary>
    public class CloseTicketDTO
    {
        [Required]
        public DateTime Closed { get; set; }

        [Required]
        public string Resolution { get; set; }

        [Required]
        public string Resolver { get; set; }
    }
}
