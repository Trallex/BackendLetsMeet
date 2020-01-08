using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public interface IEvent
    {
        Event GetEvent(string id);
        IEnumerable<Event> GetEvents();
        Event Add(Event myEvent);
        Event Update(Event eventChanges);
        Event Delete(string id);
    }
}
