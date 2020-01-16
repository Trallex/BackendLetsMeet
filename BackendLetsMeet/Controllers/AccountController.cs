using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendLetsMeet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BackendLetsMeet.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string userName, string password)
        {

            var user = await userManager.FindByNameAsync(userName);
            if(user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, password, isPersistent: false, false);
                if(result.Succeeded)
                {
                    return Ok();
                }
            }
            return BadRequest("Wrong username or password.");
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(string username, string mail, string password)
        {
            var user = new User
            {
                UserName = username,
                Email = mail
            };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                var x = await userManager.GetUserAsync(User);
                //var userId = JsonConvert.SerializeObject(x.Id);
                return Ok(x);

              // var 
            }
            return BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user.Id == userId)
            {
                var action = await userManager.DeleteAsync(user);
                if (action.Succeeded)
                {
                    return Ok();
                }
            }            
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
            
        }
        [HttpGet]
        public ActionResult<string> Tos()
        {
            return "Terms of service.";
        }
    }
}
