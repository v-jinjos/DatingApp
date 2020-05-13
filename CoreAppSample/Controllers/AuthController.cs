using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreAppSample.Data;
using CoreAppSample.Dtos;
using CoreAppSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using test = Microsoft.IdentityModel.Tokens;

namespace CoreAppSample.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo,IConfiguration config )
       {
            _repo = repo;
            _config = config;
        }
    
       [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userDto)
        {
            userDto.UserName= userDto.UserName.ToLower();
          
           if (await _repo.UserExists(userDto.UserName))
           {
               return BadRequest("Username already Exists");
           }
           var userTocreate = new User{  Username = userDto.UserName };
           var createdUser = _repo.Register(userTocreate,userDto.Password);
           return StatusCode(201);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto loginDto)
        {
          
              var userFromRepo = await _repo.Login(loginDto.UserName.ToLower(), loginDto.Password);
            if (userFromRepo == null)
                return Unauthorized();
            var claims = new[] {
                        new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                        new Claim(ClaimTypes.Name, userFromRepo.Username.ToString())
            };

            var key = new test.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new test.SigningCredentials(key, test.SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new test.SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
          
             var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new {
                   token = tokenHandler.WriteToken(token)}); 
          
            
        }
       
    }
        
}