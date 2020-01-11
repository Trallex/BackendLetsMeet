using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendLetsMeet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendLetsMeet.Controllers
{
    [Route("[controller]/[action]/{userId}")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        public GroupController()
        {

        }
        [HttpGet]
        public IActionResult List(string userId)
        {
            //group[uuid, nazwa]
            return StatusCode(200); 
        }

        [HttpPost]
        public IActionResult Create(string userId, string groupName)
        {

            //create and return group
            return StatusCode(200);
        }

        [HttpGet]
        public IActionResult Info(string userId, string groupId)
        {
            //return group
            return StatusCode(200);
        }   

        [HttpGet]
        public IActionResult Join(string userId, string idInv)
        {
            //ADD USER TO GROUP
            return StatusCode(200);
        }

        [HttpGet]
        public IActionResult Leave(string userId, string groupId)
        {
            //REMOVE USER FROM GROUP
            return StatusCode(200);
        }

        [HttpGet]
        public IActionResult Days(string userId, string groupId, DateTime start, DateTime end)
        {
            //return users and their free days
            return StatusCode(200);
        }

        [HttpPost]
        public IActionResult AddDays(string userId, string groupId, DateTime start, DateTime end)
        {
            //add days
            return StatusCode(200);
        }

        [HttpGet]
        public IActionResult MyDays(string userId, string groupId, DateTime start, DateTime end)
        {
            //return users and their free days
            return StatusCode(200);
        }
        [HttpGet]
        public IActionResult Event(string userId)
        {
            //return list of events for user
            return StatusCode(200);
        }

        [HttpGet]
        public IActionResult Event(string userId, string groupId)
        {
            //return list of events for group
            return StatusCode(200);
        }
        [HttpPost]
        public IActionResult Event(string userId, string groupId, Event newEvent)
        {
            //create event
            return StatusCode(200);
        }

       [HttpGet]
        public IActionResult EventInfo(string userId, string eventId)
        {
            //return event
            return StatusCode(200);
        }
    }
}
