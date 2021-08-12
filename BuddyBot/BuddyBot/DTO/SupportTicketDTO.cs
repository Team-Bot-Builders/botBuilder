using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuddyBot.DTO
{
    /// <summary>
    /// DTO for full support tickets
    /// </summary>
    class SupportTicketDTO
    {
        public int Id { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime Closed { get; set; }

        [Required]
        public string Description { get; set; }

        public string Resolver { get; set; }

        public string Resolution { get; set; }

        public bool Resolved { get; set; }

        [Required]
        public string Requestor { get; set; }
    }
}
