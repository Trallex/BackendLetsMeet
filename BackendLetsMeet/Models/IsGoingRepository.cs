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

        public List<IsGoing> FindUsers(int eventId)
        {
            return context.IsGoings.Where(s => s.EventId == eventId).ToList();
        }
    }
}
