using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    interface IDaysRepository
    {
        List<Days> GetDays(string userId, string groupId);
        List<Days> GetGroupDays(string groupId);
        Days Add(Days days);
        Days Delete(string id);
    }
}
