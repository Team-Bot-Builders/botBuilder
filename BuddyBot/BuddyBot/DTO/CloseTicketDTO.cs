using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuddyBot.DTO
{
    /// <summary>
    /// DTO for closed tickets
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
