using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class DaysRepository : IDaysRepository
    {
        private readonly AppDBContext context;
        public DaysRepository(AppDBContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();
        }

        public Days Add(Days days)
        {
            context.Days.Add(days);
            context.SaveChanges();
            return days;
        }

        public Days Delete(string id)
        {
            Days days = context.Days.Find(id);
            if (days != null)
            {
                context.Days.Remove(days);
                context.SaveChanges();
            }
            
            return days;
        }

        public List<Days> GetDays(string userId, string groupId)
        {
            return context.Days.Where(g => g.GroupId == Int32.Parse(groupId)).Where( u => u.UserId == userId).ToList();
        }

        public List<Days> GetGroupDays(string groupId)
        {
            return context.Days.Where(g => g.GroupId == Int32.Parse(groupId)).ToList();
        }
    }
}
