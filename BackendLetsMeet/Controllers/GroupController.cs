using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendLetsMeet.DTOs;
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
    //[Authorize]
    public class GroupController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IEventRepository eventRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IIsGoingRepository isGoingRepository;
        private readonly IFreeTimeRepository freeTimeRepository;
        private readonly IGroupUserRepository groupUserRepository;

        public GroupController(UserManager<User> userManager,
                               IEventRepository eventRepository,
                               IGroupRepository groupRepository,
                               IIsGoingRepository isGoingRepository,
                               IFreeTimeRepository freeTimeRepository,
                               IGroupUserRepository groupUserRepository)
        {
            this.userManager = userManager;
            this.eventRepository = eventRepository;
            this.groupRepository = groupRepository;
            this.isGoingRepository = isGoingRepository;
            this.freeTimeRepository = freeTimeRepository;
            this.groupUserRepository = groupUserRepository;
        }

        //get users groups
        [HttpGet]
        public IActionResult ListGroup(string userId)
        {
            var groups = groupRepository.GetUserGroups(userId);
            List<GroupEntity> groupEntities = new List<GroupEntity>();
            foreach(Group group in groups)
            {
                List<UserEntity> userEntities = new List<UserEntity>();
                var users = groupRepository.GetGroupUsers(group.GroupId);
                foreach(var user in users)
                {
                    var freeTimes = freeTimeRepository.GetGroupUserFreeTime(group.GroupId, user.Id);
                    userEntities.Add(new UserEntity(user, freeTimes));
                }

                groupEntities.Add(new GroupEntity(group, userEntities));
            }
            var result = JsonConvert.SerializeObject(groupEntities);
            return Ok(result);
                       
        }

        //create and return group
        [HttpPost]
        public async  Task<IActionResult> CreateGroup(string userId, string groupName)
        {
            User user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                Group group = new Group
                {
                    GroupId = Guid.NewGuid().ToString(),
                    Events = new List<Event>(),
                    FreeTimes = new List<FreeTime>(),
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
                // groupUserRepository.Add(groupUser);
                //GroupEntity groupEntity = new GroupEntity(group);
                //var result = JsonConvert.SerializeObject(groupEntity);
                return Ok();
            }

            return BadRequest("User not found.");
            
        }

        //return group
        [HttpGet]
        public IActionResult GetGroup(string userId, string groupId)
        {
            Group group = groupRepository.GetGroup(groupId);
            if(group != null)
            {
                var result = JsonConvert.SerializeObject(group);
                return Ok(result);
            }
            return BadRequest("Group not found.");
        }

        //ADD USER TO GROUP
        [HttpPost]
        public async Task<IActionResult> Join(string userId, string idInv)
        {
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
                try
                {
                    groupUserRepository.Add(groupUser);
                }
                catch(Exception e)
                {
                    return BadRequest("User already present in the group.");
                }
                
                return Ok();
            }

            return BadRequest("Wrong User or idInv.");
        }

        //REMOVE USER FROM GROUP
        [HttpPost]
        public async Task<IActionResult> Leave(string userId, string groupId)
        {

            Group group = groupRepository.GetGroup(groupId);
            User user = await userManager.FindByIdAsync(userId);
            if (user != null && group != null)
            {
                if(groupUserRepository.DeleteUser(user.Id) != null) return Ok();

            }
            return BadRequest("User or Group not found.");
        }

        //return users and their free time
        [HttpGet]
        public IActionResult GetFreeTime(string userId, string groupId)
        {
            var users = groupUserRepository.GetGroupUsers(groupId);
            if(users.Where(u => u.UserId == userId) != null)
            {
                // var freeTime = freeTimeRepository.GetGroupFreeTime(groupId);
                // var result = JsonConvert.SerializeObject(freeTime);
                return Ok();//result);
            }            
            return BadRequest("User not in th group");
        }

        //add free time AND RETURN object
        [HttpPost]
        public async Task<IActionResult> AddFreeTime(string userId, string groupId, DateTime start, DateTime end)
        {
            User user = await userManager.FindByIdAsync(userId);
            Group group = groupRepository.GetGroup(groupId);
                        
            if (user != null && group != null)
            {
                bool freeTimeEdited = false;
                if(start.CompareTo(end) >= 0)
                {
                    return BadRequest("Event ends before it starts.");
                }
                FreeTime freeTime = new FreeTime();
                var timesInRepo = freeTimeRepository.GetUserFreeTime(userId);//, groupId);

                if(timesInRepo != null)
                {
                    foreach(var time in timesInRepo)
                    {
                        if((time.StartTime < start && time.EndTime > start) 
                            || (time.StartTime < end && time.EndTime > end))
                        {
                            freeTime = time;
                            freeTime.EndTime = end;
                            freeTime.StartTime = start;
                            freeTimeRepository.Delete(time.Id);
                            freeTimeEdited = true;
                        }
                        else if(time.StartTime > start && time.EndTime < end)
                        {
                            freeTime = time;
                            freeTime.EndTime = end;
                            freeTime.StartTime = start;
                            freeTimeRepository.Delete(time.Id);
                            freeTimeEdited = true;
                        }
                        else if (time.EndTime == start)
                        {
                            freeTime = time;
                            freeTime.EndTime = end;
                            freeTimeRepository.Delete(time.Id);
                            freeTimeEdited = true;
                        }
                        else if (time.StartTime == end)
                        {
                            freeTime = time;
                            freeTime.StartTime = start;
                            freeTimeRepository.Delete(time.Id);
                            freeTimeEdited = true;
                        }
                    }
                }

                if(!freeTimeEdited)
                {
                    freeTime = new FreeTime
                    {
                        Id = Guid.NewGuid().ToString(),
                        EndTime = end,
                        StartTime = start,
                        User = user,
                        UserId = user.Id,
                        Group = group,
                        GroupId = group.GroupId
                    };
                }
                
                freeTimeRepository.Add(freeTime);
                FreeTimeEntity res = new FreeTimeEntity(freeTime);
                var result = JsonConvert.SerializeObject(res);
                return Ok(result);
            }

            return BadRequest("User or Group not found.");
        }

        //Delete free time if its the users one
        [HttpPost]
        public async Task<IActionResult> DeleteFreeTime(string userId, string freeTimeId)
        {
            User user = await userManager.FindByIdAsync(userId);
            if(user != null)
            {
                var freeTime = freeTimeRepository.GetById(freeTimeId);
                if(user.Id == freeTime.UserId)
                {
                    freeTimeRepository.Delete(freeTimeId);
                    return Ok();
                }
                return BadRequest("Could not found free time matching for this user.");
            }
            return BadRequest("User not found.");
        }

        //return users free time for group
        [HttpGet]
        public async Task<IActionResult> MyFreeTime(string userId)
        {
            User user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {

                List<GroupEntity> groupEntities = new List<GroupEntity>();

                var groups = groupRepository.GetUserGroups(userId);
                foreach (var group in groups)
                {

                    var groupFreeTime = freeTimeRepository.GetGroupUserFreeTime(group.GroupId, userId);
                    UserEntity userEntity = new UserEntity(user, groupFreeTime);

                    groupEntities.Add(new GroupEntity(group, new List<UserEntity> { userEntity }));
                }

                var result = JsonConvert.SerializeObject(groupEntities);
                return Ok(result);
            }
            return BadRequest("User not found.");
        }

        //return list of events for user
        [HttpGet]
        public async Task<IActionResult> GetUserEvents(string userId)
        {
            List<Event> events = new List<Event>();
            List<EventEntity> eventEntities = new List<EventEntity>();
            User user = await userManager.FindByIdAsync(userId);

            if(user != null)
            {
                var groups = groupRepository.GetUserGroups(userId);
                foreach(Group group in groups)
                {
                    var groupEvents = eventRepository.GetGroupEvents(group.GroupId);
                    events.AddRange(groupEvents);
                }
                foreach (var _event in events)
                {
                    var isGoingList = isGoingRepository.FindUsers(_event.Id);
                    List<IsGoingEntity> isGoingEntities = new List<IsGoingEntity>();
                    foreach(var isGoingListResponse in isGoingList)
                    {
                        isGoingEntities.Add(new IsGoingEntity(isGoingListResponse));
                    }
                    eventEntities.Add(new EventEntity(_event, isGoingEntities));
                }
                var result = JsonConvert.SerializeObject(eventEntities);
                return Ok(result);
            }
            return BadRequest("User not found.");
        }

        //return list of events for group
        [HttpGet]
        public IActionResult GetGroupEvents(string userId, string groupId)
        {
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

        //create event
        [HttpPost]
        public async Task<IActionResult> CreateEvent(string userId, string groupId, [FromBody]EventEntity newEvent)
        {
            User user = await userManager.FindByIdAsync(userId);
            Group group = groupRepository.GetGroup(groupId);
            
            if (user != null && group != null)
            {
                Event createdEvent = new Event
                {
                    Id = Guid.NewGuid().ToString(),
                    GroupId = groupId,
                    Group = group, 
                    Name = newEvent.Name,
                    Description = newEvent.Description,
                    StartTime = newEvent.StartTime,
                    EndTime = newEvent.EndTime,
                    Localiztion = newEvent.Localiztion

                };
                eventRepository.Add(createdEvent);
                var groupUsers = groupUserRepository.GetGroupUsers(groupId);
                foreach(GroupUser groupUser in groupUsers)
                {
                    User currentUser = await userManager.FindByIdAsync(groupUser.UserId);
                    IsGoing isGoing;
                    if (currentUser.Id == userId)
                    {
                        isGoing = new IsGoing
                        {
                            UserId = currentUser.Id,
                            User = currentUser,
                            Event = createdEvent,
                            EventId = createdEvent.Id,
                            Response = true
                        };
                    }
                    else
                    {
                        isGoing = new IsGoing
                        {
                            UserId = currentUser.Id,
                            User = currentUser,
                            Event = createdEvent,
                            EventId = createdEvent.Id
                        };
                    }
                    isGoingRepository.Create(isGoing);
                }
                return Ok();
            }
            return BadRequest("Group or User not found.");
        }

        //return event
        [HttpGet]
        public IActionResult EventInfo(string userId, string eventId)
        {
           Event myEvent  = eventRepository.GetEvent(eventId);
            if(myEvent != null)
            {
                var result = JsonConvert.SerializeObject(myEvent);
                return Ok(result);
            }
            return BadRequest("Event not found.");
        }
    
        //odpowiedzi w evencie
    }
}
