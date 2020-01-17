using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackendLetsMeet.DTOs;
using BackendLetsMeet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BackendLetsMeet.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
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
                    string securityKey = "super_long_security_key_pls_work_2137";
                    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
                    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                    var token = new JwtSecurityToken(
                          issuer: "smieszek",
                          audience: "readers",
                          signingCredentials: signingCredentials,
                          claims: claims
                        );
                    string tokenString = new JwtSecurityTokenHandler().WriteToken(token).ToString();
                    var resultOutput = JsonConvert.SerializeObject(new UserTokenEntity(tokenString, user));
                    return Ok(resultOutput);
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

                // return Ok();

                string securityKey = "super_long_security_key_pls_work_2137";
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                var token = new JwtSecurityToken(
                      issuer: "smieszek",
                      audience: "readers",
                      signingCredentials: signingCredentials,
                      claims: claims
                    );
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token).ToString();
                var resultOutput = JsonConvert.SerializeObject(new UserTokenEntity(tokenString, user));
                return Ok(resultOutput);
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
            string x = DateTime.Now.ToString();
            return "Terms of service.";
        }
    }
}
