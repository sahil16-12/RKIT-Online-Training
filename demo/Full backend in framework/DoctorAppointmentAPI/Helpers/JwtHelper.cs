using DoctorAppointmentAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoctorAppointmentAPI.Helpers
{
    /// <summary>
    /// JWT tojen creator
    /// </summary>
    public class JwtHelper
    {
        string SecretKey = ConfigurationManager.AppSettings["JwtSecret"];

        /// <summary>
        /// Token generating method
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateToken(TBL01 user)
        {
            byte[] key = Encoding.UTF8.GetBytes(SecretKey);

            Claim[] claims =
            {
                new Claim(ClaimTypes.Name, user.L01F02), // Username
                new Claim(ClaimTypes.Role, ((UserRole)user.L01F06).ToString()), // Role
                new Claim("userId", user.L01F01.ToString()) // Id
            };

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
