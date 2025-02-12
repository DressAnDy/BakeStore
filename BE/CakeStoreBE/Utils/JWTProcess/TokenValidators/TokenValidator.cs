using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecrectKey));

            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ClockSkew = TimeSpan.Zero
            };

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var claimsPrincipal = handler.ValidateToken(token, parameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken)
                {
                    Console.WriteLine("Validated token is not a JwtSecurityToken.");
                    return null;
                }

                // Kiểm tra thuật toán của token
                if (!jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("Invalid token algorithm. Expected: HS256.");
                    return null;
                }

                // Đảm bảo rằng không có lỗi khi xác thực Token
                Console.WriteLine($"Token is valid. User claims: {claimsPrincipal.Identity.Name}");

                return claimsPrincipal;
            }
            catch (SecurityTokenExpiredException)
            {
                Console.WriteLine("Token has expired.");
                return null;
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                Console.WriteLine("Token signature is invalid.");
                return null;
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during token validation: {ex.Message}");
                return null;
            }
        }
    }
}
