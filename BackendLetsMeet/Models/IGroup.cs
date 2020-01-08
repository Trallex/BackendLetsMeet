using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    interface IGroup
    {
        Group GetGroup(string id);
        IEnumerable<Group> GetGroups();
        Group Add(Group myGroup);
        Group Update(Group groupChanges);
        Group Delete(string id);
    }
}

