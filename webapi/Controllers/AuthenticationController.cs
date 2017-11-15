using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using webapi.Models;

namespace webapi.Controllers{

    [Route("api/authentication")]
    public class AuthenticationController : Controller{

        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserManager<DutchUser> _userManager;
        private readonly SignInManager<DutchUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthenticationController(
            UserManager<DutchUser> userManager, 
            SignInManager<DutchUser> signInManager,
            ILogger<AuthenticationController> logger,
            IConfiguration config)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpGet("sayhello")]
        public IActionResult sayHello(){
            return Ok("helloooooo");
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody] LoginModel tokenRequestBody){
            
            //need to get the email from the body
            var user = await _userManager.FindByEmailAsync(tokenRequestBody.UserName);

            if (user != null)
            {
                //password from the body
                var result = await _signInManager.CheckPasswordSignInAsync(user, tokenRequestBody.Password, false);
                if (result.Succeeded)
                {

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                    _config["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(8),
                    signingCredentials: creds);

                    return Ok(new { 
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration= token.ValidTo 
                        });
                }
            }
            return BadRequest("Could not create token");
        }

    }
}