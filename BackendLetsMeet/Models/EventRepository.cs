using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDBContext context;

        public EventRepository(AppDBContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();
        }
        public Event Add(Event myEvent)
        {
            context.Events.Add(myEvent);
            context.SaveChanges();
            return myEvent;
        }

        public Event Delete(string id)
        {
            Event myEvent = context.Events.Find(id);
            if(myEvent != null)
            {
                context.Events.Remove(myEvent);
                context.SaveChanges();
            }
            return myEvent;
        }

        public Event GetEvent(string id)
        {
            return context.Events.Find(id);
        }

        public IEnumerable<Event> GetGroupEvents(string groupId)
        {
            return context.Events.Where(e => e.GroupId == groupId).ToList();
        }

        public IEnumerable<Event> GetUserEvents(string userId)
        {
            return null;//context.Events.Where(e => e. == groupId).ToList();
        }

        public Event Update(Event eventChanges)
        {
            var myEvent = context.Events.Attach(eventChanges);
            myEvent.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return eventChanges;
        }
    }
}

