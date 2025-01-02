using CakeStoreBE.Application.DTOs.PayloadDTOs;
using System.Security.Claims;

namespace CakeStoreBE.Utils.Payload
{
    public class PayloadConverter
    {
        public static IEnumerable<Claim> ToClaims(PayloadDTO payload)
        {
            return new[]
            {
            new Claim("userID", payload.UserId),
            new Claim("email", payload.Email),
            new Claim("isAdmin", payload.IsAdmin.ToString())
        };
        }

        public static PayloadDTO FromClaims(ClaimsPrincipal principal)
        {
            return new PayloadDTO
            {
                UserId = principal.Claims.First(c => c.Type == "userID").Value,
                Email = principal.Claims.First(c => c.Type == "email").Value,
                IsAdmin = principal.Claims.First(c => c.Type == "isAdmin").Value
            };
        }
    }
}
