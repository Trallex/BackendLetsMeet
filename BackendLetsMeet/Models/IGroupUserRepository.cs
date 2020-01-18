using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public interface IGroupUserRepository
    {
        GroupUser Add(GroupUser groupUser);
        GroupUser DeleteUser(string userId, string groupId);
        IEnumerable<GroupUser> GetGroupUsers(string groupId);
        void DeleteGroup(string groupId);
    }
}
