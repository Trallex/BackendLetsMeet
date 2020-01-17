using BackendLetsMeet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.DTOs
{
    public class UserTokenEntity
    {
        public string Token { get; set; }
        public string UserName { get; }
        public string UserId { get; set; }       

        public UserTokenEntity(string token, User user)
        {
            Token = token;
            UserName = user.UserName;
            UserId = user.Id;
        }
    }
}
