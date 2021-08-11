using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuddyBot.DTO
{
    class LiveTicketDTO
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public string Requestor { get; set; }
    }
}
