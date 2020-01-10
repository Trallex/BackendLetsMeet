using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendLetsMeet.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BackendLetsMeet.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IEventRepository eventRepository;

        public AccountController(IEventRepository _eventRepository)
        {
            eventRepository = _eventRepository;
        }

        [HttpGet]
        public IActionResult Login(string id, string password)
        {
            return StatusCode(200);
        }
        
        [HttpGet]
        public IActionResult Register(string username, string mail, string password)
        {
            return StatusCode(200);
        }

        [HttpGet]
        public ActionResult<string> Tos()
        {
            return "Terms of service.";
        }
    }
}
