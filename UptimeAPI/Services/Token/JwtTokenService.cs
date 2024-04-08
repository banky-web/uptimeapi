using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UptimeAPI.Services.Token
{
    public class JwtTokenService
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public string GenerateToken(ApplicationUser user, JwtSettings jwtSettings)
        {
            try
            {
                //        var claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                //};

                //        // Use a recommended key size for HmacSha256 (256 bits / 32 bytes)
                //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));

                //        // Ensure that the algorithm matches the key size
                //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //        var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings.ExpirationInDays));

                //        var token = new JwtSecurityToken(jwtSettings.Issuer, jwtSettings.Issuer, claims, null, expires, creds);

                //        return new JwtSecurityTokenHandler().WriteToken(token);


                //we have to generate JWT Token
                
                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {

                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays( jwtSettings.ExpirationInDays),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
