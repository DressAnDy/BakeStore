using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Logging;
using System.Text;

namespace CakeStoreBE.Utils.JWTProcess.TokenValidators
{
    public class TokenValidator
    {
        private readonly JwtServices _options;

        public TokenValidator(IOptions<JwtServices> options)
        {
            _options = options.Value;
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            IdentityModelEventSource.ShowPII = true;
            if(string.IsNullOrEmpty(token)){
                Console.WriteLine("Token validation failed: Token is empty or null.");
                return null;
            }
        
            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecrectKey));
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_options.SecrectKey);

            try{
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ClockSkew = TimeSpan.Zero
            };

            var handler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;
            var principal = handler.ValidateToken(token, parameters, out securityToken);

            if(principal == null){
                Console.WriteLine("Token validation failed: principal is null.");
            }

            return principal;

            }catch(Exception ex){
                Console.WriteLine($"Unexpected error during token validation: {ex.Message}");
                return null;
            }
        }   
    }
}
