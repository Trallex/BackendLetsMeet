﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class User : IdentityUser
    {
        public ICollection<GroupUser> GroupUsers { get; set; }
        public ICollection<Days> Days { get; set; }
    }
}
