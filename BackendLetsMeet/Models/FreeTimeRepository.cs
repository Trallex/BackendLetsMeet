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
            FreeTime freeTime = context.FreeTime.Where(ft => ft.Id == id).FirstOrDefault(); ;
            if (freeTime != null)
            {
                context.FreeTime.Remove(freeTime);
                context.SaveChanges();
            }
            
            return freeTime;
        }

        public FreeTime GetById(string id)
        {
            return context.FreeTime.Where(ft => ft.Id == id).FirstOrDefault();
        }

        public List<FreeTime> GetUserFreeTime(string userId)
        {
            DateTime now = DateTime.Now;
            return context.FreeTime.Where( u => u.UserId == userId).Where(ft => ft.EndTime.CompareTo(now) > 0).ToList();
        }

        public List<FreeTime> GetGroupUserFreeTime(string groupId, string userId)
        {
            DateTime now = DateTime.Now;
            var x = context.FreeTime.Where(g => g.GroupId == groupId).Where(u => u.UserId == userId);
            var y = x.Where(ft => ft.EndTime.CompareTo(now) > 0).ToList();
            return y;
        }
    }
}
