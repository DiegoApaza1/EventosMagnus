using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Magnus.Application.Interfaces;
using Magnus.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;

namespace Magnus.Infrastructure.Services  
{  
    public class JwtTokenService : ITokenService  
    {  
        private readonly string _issuer;  
        private readonly string _audience;  
        private readonly string _key;  
        private readonly int _expirationMinutes;  
  
        public JwtTokenService(IConfiguration configuration)  
        {  
            _issuer = configuration["Jwt:Issuer"] ?? "EventosMagnus";  
            _audience = configuration["Jwt:Audience"] ?? "EventosMagnus.Client";  
            _key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key no configurado");  
            _expirationMinutes = int.TryParse(configuration["Jwt:ExpirationMinutes"], out var mins) ? mins : 60;  
        }  

        public string CreateToken(Usuario user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Nombre),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GeneratePasswordResetToken()
        {
            // Generar un token seguro de 32 bytes (256 bits)
            byte[] tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            return Convert.ToBase64String(tokenBytes)
                .Replace('+', '-')  // URL-safe
                .Replace('/', '_')  // URL-safe
                .TrimEnd('=');      // Remove padding
        }
    }  
}