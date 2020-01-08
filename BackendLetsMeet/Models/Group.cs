using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class Group
    {
        [Key]
        public int IdGroup { get; set; }
        public ICollection<User> Members {get; set;}
        public string Name { get; set; }
    }
}
