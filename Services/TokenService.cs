using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testeBitzen.Models;
using Microsoft.IdentityModel.Tokens;

namespace testeBitzen.Services
{
    public static class TokenService
    {
        public static string GenerateToken(User user)
        {
            DotNetEnv.Env.Load();
            var tokenHanddler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(System.Environment.GetEnvironmentVariable("SECRET"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHanddler.CreateToken(tokenDescriptor);
            Console.WriteLine(tokenHanddler.WriteToken(token));
            Console.WriteLine(tokenHanddler.WriteToken(token));
            Console.WriteLine(tokenHanddler.WriteToken(token));
            Console.WriteLine(tokenHanddler.WriteToken(token));
            return tokenHanddler.WriteToken(token);
        }
    }
}

