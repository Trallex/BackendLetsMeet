﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class Group
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string GroupId { get; set; }
        public string EventId { get; set; }
        public ICollection<Event >Events { get; set; }
        public ICollection<GroupUser> GroupUsers { get; set;}
        public ICollection<FreeTime> FreeTimes { get; set; }
        public string Name { get; set; }
        public string InvId { get; set; }
    }
}
