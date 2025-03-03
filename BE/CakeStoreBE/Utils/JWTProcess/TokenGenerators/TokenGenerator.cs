using CakeStoreBE.Application.DTOs.UsersDTOs;
using CakeStoreBE.Infrastructure.Database;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CakeStoreBE.Utils.JWTProcess.TokenGenerators
{
    public class TokenGenerator
    {
       private readonly JwtServices _jwtOptions;
       private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;
       private readonly BakeStoreDbContext _context;


        public TokenGenerator(IOptions<JwtServices> _options, BakeStoreDbContext context)
        {
            _jwtOptions = _options.Value;
            jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _context = context;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecrectKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes),
                signingCredentials: credentials);

            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public PasswordResetTokenDTO? GeneratePasswordResetToken(string email){
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiryDate = DateTime.UtcNow.AddMinutes(120);

            return new PasswordResetTokenDTO{
                Token = token,
                ExpiryDate = expiryDate,
                Email = email
            };
        }
    }
}