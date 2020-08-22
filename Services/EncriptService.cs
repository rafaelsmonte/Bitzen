using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testeBitzen.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace testeBitzen.Services
{
    public static class EncriptService
    {
        public static string GenerateEcriptionSHA256(string senha)
        {
            DotNetEnv.Env.Load();
            string secret = System.Environment.GetEnvironmentVariable("SECRET");
            string str = senha + secret;
            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));  
  
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)  
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                return builder.ToString();  
            }

        }
    }
}