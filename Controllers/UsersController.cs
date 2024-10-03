using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppApi.Data;
using WebAppApi.Models;

namespace WebAppApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsersController(JwtOptions jwtOptions,AppDbContext dbContext) :ControllerBase
    {

        [HttpPost]
        [Route("auth")]
        public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
        {
            var user = dbContext.Set<User>().SingleOrDefault(u=>u.UserName==request.UserName && u.Password == request.Password);
            if (user == null)
                return Unauthorized(); 
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
                , SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,"Admin"),
                    new Claim(ClaimTypes.Role,"SuperUser"),
                    new Claim("UserType","Employee"),
                    new("DateOfBirth","2002-12-2")
                })
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return Ok(accessToken);
        }
    }
}
