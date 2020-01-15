using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendLetsMeet.Models
{
    public class GroupUserRepository : IGroupUserRepository
    {

        private readonly AppDBContext context;

        public GroupUserRepository(AppDBContext context)
        {
            this.context = context;
            context.Database.EnsureCreated();
        }

        public GroupUser Add(GroupUser groupUser)
        {
            context.GroupUsers.Add(groupUser);
            context.SaveChanges();
            return groupUser;
        }

        public void DeleteGroup(string groupId)
        {
            foreach(GroupUser groupUser in context.GroupUsers.Where(ug => ug.GroupId == groupId))
            {
                context.GroupUsers.Remove(groupUser);
            }
        }

        public GroupUser DeleteUser(string userId)
        {
            var groupUser = context.GroupUsers.Where(ug => ug.UserId == userId).FirstOrDefault();
            if(groupUser != null)
            {
                context.GroupUsers.Remove(groupUser);
            }
            return groupUser;
        }

        public IEnumerable<GroupUser> GetGroupUsers(string groupId)
        {
           return context.GroupUsers.Where(gu => gu.GroupId == groupId).ToList();
        }
    }
}
