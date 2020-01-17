using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class IsGoingRepository : IIsGoingRepository
    {

        private readonly AppDBContext context;
        public IsGoingRepository(AppDBContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();
        }

        public IsGoing Create(IsGoing isGoing)
        {
            context.IsGoings.Add(isGoing);
            context.SaveChanges();
            return isGoing;
        }

        public IsGoing FindRecord(string userId, string eventId)
        {
            return context.IsGoings.Where(e => e.EventId == eventId).Where(u => u.UserId == userId).FirstOrDefault();
        }

        public List<IsGoing> FindUsers(string eventId)
        {
            return context.IsGoings.Where(s => s.EventId == eventId).ToList();
        }

        public IsGoing Upadte(IsGoing isGoing)
        {
            context.IsGoings.Update(isGoing);
            context.SaveChanges();
            return isGoing;
        }
    }
}
