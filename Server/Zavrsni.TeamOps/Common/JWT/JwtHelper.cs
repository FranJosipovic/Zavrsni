using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zavrsni.TeamOps.Entity.Models;

namespace Zavrsni.TeamOps.JWT
{
    public static class JwtHelper
    {
        public static string IssueNewToken(User userData, IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("id", userData.Id.ToString()),
                new Claim("name", userData.Username), 
                new Claim("surname",userData.Surname),
                new Claim("email", userData.Email), 
                new Claim("username",userData.Username),
            };

            var Sectoken = new JwtSecurityToken(issuer: configuration["Jwt:Issuer"],
              audience: configuration["Jwt:Issuer"],
              claims: claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(Sectoken);
        }
    }
}
