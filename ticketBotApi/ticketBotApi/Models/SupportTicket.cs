﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ticketBotApi.Models
{
    public class SupportTicket
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
