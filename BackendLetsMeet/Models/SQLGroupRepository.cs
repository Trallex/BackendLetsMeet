using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class SQLGroupRepository : IGroup
    {

        private readonly AppDBContext context;

        public SQLGroupRepository(AppDBContext context)
        {
            this.context = context;
        }
        public Group Add(Group group)
        {
            context.Groups.Add(group);
            context.SaveChanges();
            return group;
        }

        public Group Delete(string id)
        {
            Group group = context.Groups.Find(id);
            if (group != null)
            {
                context.Groups.Remove(group);
                context.SaveChanges();
            }
            return group;
        }

        public Group GetGroup(string id)
        {
            return context.Groups.Find(id);
        }

        public IEnumerable<Group> GetGroups()
        {
            return context.Groups;
        }

        public Group Update(Group groupChanges)
        {
            var group = context.Groups.Attach(groupChanges);
            group.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return groupChanges;
        }
    }
}
