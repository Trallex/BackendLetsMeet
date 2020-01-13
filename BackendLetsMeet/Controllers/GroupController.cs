using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendLetsMeet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace BackendLetsMeet.Controllers
{
    [Route("[controller]/[action]/{userId}")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IEventRepository eventRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IIsGoingRepository isGoingRepository;
        private readonly IDaysRepository daysRepository;
        private readonly IGroupUserRepository groupUserRepository;

        public GroupController(UserManager<User> userManager,
                               IEventRepository eventRepository,
                               IGroupRepository groupRepository,
                               IIsGoingRepository isGoingRepository,
                               IDaysRepository daysRepository,
                               IGroupUserRepository groupUserRepository)
        {
            this.userManager = userManager;
            this.eventRepository = eventRepository;
            this.groupRepository = groupRepository;
            this.isGoingRepository = isGoingRepository;
            this.daysRepository = daysRepository;
            this.groupUserRepository = groupUserRepository;
        }

        [HttpGet]
        public IActionResult List(string userId)
        {
            //group[uuid, nazwa]
            var groups = groupRepository.GetUserGroups(userId);
            var result = JsonConvert.SerializeObject(groups);
            return Ok(result);
                       
        }

        [HttpPost]
        public async  Task<IActionResult> Create(string userId, string groupName)
        {

            //create and return group

            User user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                Group group = new Group
                {
                    GroupId = Guid.NewGuid().ToString(),
                    Events = new List<Event>(),
                    Days = new List<Days>(),
                    GroupUsers = new List<GroupUser>(),
                    Name = groupName,
                    InvId = Guid.NewGuid().ToString()
                };
                GroupUser groupUser = new GroupUser
                {
                    User = user,
                    UserId = user.Id,
                    GroupId = group.GroupId,
                    Group = group
                };
                group.GroupUsers.Add(groupUser);


                groupRepository.Add(group);
                groupUserRepository.Add(groupUser);
                var result = JsonConvert.SerializeObject(group);
                return Ok(result);
            }

            return BadRequest("User not found.");
            
        }

        [HttpGet]
        public IActionResult Info(string userId, string groupId)
        {
            //return group
            Group group = groupRepository.GetGroup(groupId);
            if(group != null)
            {
                var result = JsonConvert.SerializeObject(group);
                return Ok(result);
            }
            return BadRequest("Group not found.");
        }   

        [HttpPost]
        public async Task<IActionResult> Join(string userId, string idInv)
        {
            //ADD USER TO GROUP
            Group group = groupRepository.GetGropByInvLink(idInv);
            User user = await userManager.FindByIdAsync(userId);
            if(user != null && group != null)
            {
                GroupUser groupUser = new GroupUser
                {
                    User = user,
                    UserId = user.Id,
                    Group = group,
                    GroupId = group.GroupId
                };
                groupUserRepository.Add(groupUser);
                return Ok();
            }

            return BadRequest("Wrong User or idInv.");
        }

        [HttpGet]
        public async Task<IActionResult> Leave(string userId, string groupId)
        {
            //REMOVE USER FROM GROUP

            Group group = groupRepository.GetGroup(groupId);
            User user = await userManager.FindByIdAsync(userId);
            if (user != null && group != null)
            {
                if(groupUserRepository.DeleteUser(user.Id) != null) return Ok();

            }
            return BadRequest("User or Group not found.");
        }

        [HttpGet]
        public IActionResult Days(string userId, string groupId, DateTime start, DateTime end)
        {
            //return users and their free days
            return StatusCode(200);
        }

        [HttpPost]
        public async Task<IActionResult> AddDays(string userId, string groupId, DateTime start, DateTime end)
        {
            //add busy days

            User user = await userManager.FindByIdAsync(userId);
            Group group = groupRepository.GetGroup(groupId);
            
            if (user != null && group != null)
            {
                Days days = new Days
                {
                    EndTime = end,
                    StartTime = start,
                    User = user,
                    UserId = user.Id,
                    Group = group,
                    GroupId = group.GroupId
                };
                daysRepository.Add(days);
                return Ok();
            }

            return BadRequest("User or Group not found.");
        }

        [HttpGet]
        public IActionResult MyDays(string userId, string groupId, DateTime start, DateTime end)
        {
            //return users and their free days
            Group group = groupRepository.GetGroup(groupId);
            if(group != null)
            {
                var days = daysRepository.GetGroupDays(groupId);

                var result = JsonConvert.SerializeObject(days);
                return Ok(result);
            }
            return BadRequest("Group not found.");
        }
        [HttpGet]
        public async Task<IActionResult> Event(string userId)
        {
            //return list of events for user
            List<Event> events = new List<Event>();

            User user = await userManager.FindByIdAsync(userId);

            if(user != null)
            {
                var groups = groupRepository.GetUserGroups(userId);
                foreach(Group group in groups)
                {
                    var groupEvents = eventRepository.GetGroupEvents(group.GroupId);
                    events.AddRange(groupEvents);
                }

                var result = JsonConvert.SerializeObject(events);
                return Ok(result);
            }
            return BadRequest("User not found.");
        }

        [HttpGet]
        public async Task<IActionResult> Event(string userId, string groupId)
        {
            //return list of events for group
            List<Event> events = new List<Event>();

            Group group = groupRepository.GetGroup(groupId);

            if (group != null)
            {
                var groupEvents = eventRepository.GetGroupEvents(groupId);
                events.AddRange(groupEvents);

                var result = JsonConvert.SerializeObject(events);
                return Ok(result);
            }
            return BadRequest("Group not found.");
        }
        
        [HttpPost]
        public async Task<IActionResult> Event(string userId, string groupId, Event newEvent)
        {
            //create event
            User user = await userManager.FindByIdAsync(userId);
            Group group = groupRepository.GetGroup(groupId);
            
            if (user != null && group != null)
            {
                eventRepository.Add(newEvent);
                var groupUsers = groupUserRepository.GetGroupUsers(groupId);
                foreach(GroupUser groupUser in groupUsers)
                {
                    User currentUser = await userManager.FindByIdAsync(groupUser.UserId);
                    IsGoing isGoing;
                    if (currentUser.Id == userId)
                    {
                        isGoing = new IsGoing
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = currentUser.Id,
                            User = currentUser,
                            Event = newEvent,
                            EventId = newEvent.Id,
                            Response = true
                        };
                    }
                    else
                    {
                        isGoing = new IsGoing
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = currentUser.Id,
                            User = currentUser,
                            Event = newEvent,
                            EventId = newEvent.Id
                        };
                    }
                    isGoingRepository.Create(isGoing);
                }
                return Ok();
            }
            return BadRequest("Group or User not found.");
        }

       [HttpGet]
        public IActionResult EventInfo(string userId, string eventId)
        {
            //return event
           Event myEvent  = eventRepository.GetEvent(eventId);
            if(myEvent != null)
            {
                var result = JsonConvert.SerializeObject(myEvent);
                return Ok(result);
            }
            return BadRequest("Event not found.");
        }
    }
}
