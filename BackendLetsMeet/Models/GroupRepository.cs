using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class GroupRepository : IGroupRepository
    {

        private readonly AppDBContext context;

        public GroupRepository(AppDBContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();
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

        public Group GetGropByInvLink(string invLink)
        {
            return context.Groups.Where(g => g.InvId == invLink).FirstOrDefault();
        }

        public Group GetGroup(string id)
        {
            return context.Groups.Find(id);
        }
        
        public IEnumerable<Group> GetGroups()
        {
            return context.Groups;
        }

        public IEnumerable<Group> GetUserGroups(string userId)
        {
            List<Group> result = new List<Group>();
            var userGroups = context.GroupUsers.Where(gu => gu.UserId == userId).ToList();
            foreach(GroupUser userGroup in userGroups)
            {
                result.Add(context.Groups.Where(g => g.GroupId == userGroup.GroupId).FirstOrDefault());
            }
            return result;
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
