using BackendLetsMeet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.DTOs
{
    public class GroupEntity
    {
        public string GroupId { get; set; }
        public string Name { get; set; }
        public string InvId { get; set; }
        public List<EventEntity> Events { get; set; }
        public List<UserEntity> Users { get; set; }


        public GroupEntity(Group group)
        {
            GroupId = group.GroupId;
            Name = group.Name;
            InvId = group.InvId;       
            foreach(GroupUser groupUser in group.GroupUsers)
            {
                Users.Add(new UserEntity(groupUser.User));
            }
        }
    }
}
