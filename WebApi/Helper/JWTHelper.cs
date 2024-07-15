using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Helper
{
    public class JWTHelper
    {
        public static string KEY = "b19c16a8e0b59f976d3b7666074a0e05670d021e5582f739f5b9038f082248b27cf2f78c24681549a429745e0e39e2e8fd622fc99e1c8e8f06f45818211ef76f";
        public static string Generate(int id, string role)
        {
            //init claims payload
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, id.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            //set jwt config
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY)), SecurityAlgorithms.HmacSha512)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
             
            return jwtToken;
        }
    }
}
