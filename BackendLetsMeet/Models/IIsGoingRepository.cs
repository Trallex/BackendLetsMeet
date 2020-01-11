using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    interface IIsGoingRepository
    {
        IsGoing Create(IsGoing isGoing);
        List<IsGoing> FindUsers(int eventId);
        
    }
}
