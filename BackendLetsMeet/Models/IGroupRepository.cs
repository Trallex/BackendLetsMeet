using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public interface IGroupRepository
    {
        Group GetGroup(string id);
        Group GetGropByInvLink(string invLink);
        IEnumerable<Group> GetGroups();
        IEnumerable<Group> GetUserGroups(string userId);
        Group Add(Group myGroup);
        Group Update(Group groupChanges);
        Group Delete(string id);
    }
}

