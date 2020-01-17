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

        public UserEntity(User user)
        {
            if (user != null)
            {
                Name = user.UserName;
                if (user.FreeTime != null)
                {
                    foreach (FreeTime freeTime in user.FreeTime)
                    {
                        UserFreeTimes.Add(new FreeTimeEntity(freeTime));
                    }
                }
            }
        }
    }
}
