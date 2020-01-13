using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class IsGoing
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string EventId { get; set; }
        public Event Event { get; set; }
        public bool? Response { get; set; }

    }
}
