using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Magnus.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Magnus.Infrastructure.Adapters.Services
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

        public string GenerateToken(Guid userId, string nombre, string email, IEnumerable<KeyValuePair<string, string>>? extraClaims = null, DateTime? nowUtc = null)
        {
            var now = nowUtc ?? DateTime.UtcNow;
            var expires = now.AddMinutes(_expirationMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, nombre),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (extraClaims != null)
            {
                foreach (var kv in extraClaims)
                {
                    claims.Add(new Claim(kv.Key, kv.Value));
                }
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
