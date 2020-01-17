using BackendLetsMeet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.DTOs
{
    public class UserEntity
    {
        public string Name { get; set; }
        public List<FreeTimeEntity> UserFreeTimes { get; set; }

        public UserEntity(User user, List<FreeTime> freeTimes)
        {
            UserFreeTimes = new List<FreeTimeEntity>();
            Name = user.UserName;
            foreach(FreeTime freeTime in freeTimes)
            {
                var x = new FreeTimeEntity(freeTime);
                UserFreeTimes.Add(x);
            }
        }
    }
}
