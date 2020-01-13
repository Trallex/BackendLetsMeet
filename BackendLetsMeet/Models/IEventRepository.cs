using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public interface IEventRepository
    {
        Event GetEvent(string id);
        IEnumerable<Event> GetGroupEvents(string groupId);
        Event Add(Event myEvent);
        Event Update(Event eventChanges);
        Event Delete(string id);
    }
}
