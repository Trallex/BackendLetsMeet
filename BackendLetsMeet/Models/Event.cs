using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class Event
    {
        [Key]
        public int IdEvent { get; set; }
        [ForeignKey("Group")]
        public int IdGroup { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Localiztion { get; set; }
    }
}
