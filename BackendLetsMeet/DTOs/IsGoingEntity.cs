using BackendLetsMeet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.DTOs
{
    public class IsGoingEntity
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool? IsGoing { get; set; }

        public IsGoingEntity(IsGoing isGoing)
        {
            UserId = isGoing.UserId;
            UserName = isGoing.User.UserName;
            IsGoing = isGoing.Response;
        }
    }
}
