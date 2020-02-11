using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auto.VehicleCatalog.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Auto.VehicleCatalog.API.Helpers
{
    public static class Extensions
    {
        public static string ToPtBrFormat(this decimal instance)
        {
            // Gets a NumberFormatInfo associated with the pt-BR culture.
            NumberFormatInfo nfi = new CultureInfo("pt-BR", false).NumberFormat;
            return instance.ToString("C2", nfi);
        }

        public static async Task<string> GenerateJwtToken(this User user, 
            UserManager<User> userManager, 
            string tokenSecurityKey, 
            double hoursUntilExpire)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                // Put the user role in the token claim
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecurityKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(hoursUntilExpire),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
