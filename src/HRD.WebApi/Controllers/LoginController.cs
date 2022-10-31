using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using HRD.WebApi.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using HRD.WebApi.Data;
using HRD.WebApi.Authorization;
using HRD.WebApi.Data.Entities;
using System.Collections.Generic;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private HRDContext _context;

        public LoginController(IConfiguration config, HRDContext context)
        {
            _config = config;
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLoginViewModel userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }
            return NotFound("user not found");
        }

        private UserViewModel Authenticate(UserLoginViewModel userLogin)
        {
            var isValid = AuthorizationHelper.ValidateUsernameAndPassword(userLogin.UserName, userLogin.Password);
            if (isValid)
            {
                var query = _context.Users.Where(s => s.UserId.ToLower() == userLogin.UserName.ToLower()).FirstOrDefault();

                var roles = (from ur in _context.UserRoles
                             join r in _context.Roles on ur.RoleId equals r.Id
                             where ur.UserId == query.Id
                             select new Role
                             {
                                 Name = r.Name
                             }).ToList();

                if (query != null)
                {
                    return new UserViewModel { UserId = query.UserId, Name = query.Name};
                }
            }
            return null;
        }

        private string Generate(UserViewModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("name", user.Name),
            };

            var claimsList = new List<Claim>(claims);
            //foreach (var role in user.UserRoles)
            //{
            //    claimsList.Add(new Claim("role", role));
            //}
            claims = claimsList.ToArray();

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, 
                                    expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
