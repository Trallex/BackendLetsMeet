using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class FreeTimeRepository : IFreeTimeRepository
    {
        private readonly AppDBContext context;
        public FreeTimeRepository(AppDBContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();
        }

        public FreeTime Add(FreeTime freeTime)
        {
            context.FreeTime.Add(freeTime);
            context.SaveChanges();
            return freeTime;
        }

        public FreeTime Delete(string id)
        {
            FreeTime freeTime = context.FreeTime.Find(id);
            if (freeTime != null)
            {
                context.FreeTime.Remove(freeTime);
                context.SaveChanges();
            }
            
            return freeTime;
        }

        public FreeTime GetById(string id)
        {
            return context.FreeTime.Find(id);
        }

        public List<FreeTime> GetUserFreeTime(string userId)
        {
            DateTime now = DateTime.Now;
            return context.FreeTime.Where( u => u.UserId == userId).Where(ft => ft.EndTime > now).ToList();
        }

        public List<FreeTime> GetGroupFreeTime(string groupId)
        {
            DateTime now = DateTime.Now;
            return context.FreeTime.Where(g => g.GroupId == groupId).Where(ft => ft.EndTime > now).ToList();
        }
    }
}
