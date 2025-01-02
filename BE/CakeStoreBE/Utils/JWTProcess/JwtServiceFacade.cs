using CakeStoreBE.Application.DTOs.PayloadDTOs;
using CakeStoreBE.Utils.JWTProcess.TokenGenerators;
using CakeStoreBE.Utils.JWTProcess.TokenValidators;
using CakeStoreBE.Utils.Payload;
using System.Security.Claims;

namespace CakeStoreBE.Utils.JWTProcess
{
    public class JwtServiceFacade
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly TokenValidator _tokenValidator;

        public JwtServiceFacade(TokenGenerator tokenGenerator, TokenValidator tokenValidator)
        {
            _tokenGenerator = tokenGenerator;
            _tokenValidator = tokenValidator;
        }

        public string GenerateAccessToken(PayloadDTO payload)
        {
            var claims = PayloadConverter.ToClaims(payload);
            return _tokenGenerator.GenerateAccessToken(claims);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            return _tokenValidator.ValidateToken(token);
        }
    }
}
