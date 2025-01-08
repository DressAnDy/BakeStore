using CakeStoreBE.Application.DTOs.LoginUserDTOs;
using CakeStoreBE.Application.DTOs.UsersDTOs;
using CakeStoreBE.Constants.Enums;
using CakeStoreBE.Domain;
using CakeStoreBE.Utils.Hash;
using CakeStoreBE.Utils.JWTProcess.TokenGenerators;
using CakeStoreBE.Utils.JWTProcess.TokenValidators;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace CakeStoreBE.Application.Services
{
    public class AuthServices : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenGenerator _tokenGenerator;
        private readonly List<User> _users;
        private readonly TokenValidator _tokenValidator;


        public AuthServices(TokenGenerator tokenGenerator, IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache; 
            _tokenGenerator = tokenGenerator;
        }   

        public async Task<string> HandleRegister(RegisterUserDTO registerUserDTO)
        {
            if (_users.Any(u => u.Username == registerUserDTO.UserName))
            {
                throw new Exception("Username already in use");
            }

            var rawData = $"{registerUserDTO.UserName}-{DateTime.UtcNow.Ticks}";
            var hashUserID = Hasher.HashWithSHA256(rawData);

            var hashPassword = Hasher.HashWithSHA256(registerUserDTO.Password);

            var user = new User
            {
                UserId = hashUserID,
                Username = registerUserDTO.UserName,
                Password = hashPassword,
                RoleId = registerUserDTO.RoleId,
                CreatedAt = DateTime.UtcNow,
            };

            _users.Add(user);
            return "User Register Successfully.";
        }

        public async Task<IActionResult> HandleLogin(LoginUserDTO _login)
        {
            try
            {
                var user = _users.FirstOrDefault(u => u.Username == _login.UserName);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid Username Or Password" });
                }

                var password = Hasher.HashWithSHA256(_login.Password);
                if (password != user.Password)
                {
                    return Unauthorized("Invalid Username Or Password");
                }

                var claims = new List<Claim>
            {
                //Claim là mẫu thông tin về một Entity ( thường là user ) được gắn vào Identity ( Nhận dạng người dùng )


                //Một claim có thể chứa thông tin như sau
                new Claim(ClaimTypes.NameIdentifier, user.UserId), // UserId
                new Claim(ClaimTypes.Name, user.Username), // Username          
            };

                var userRole = user.RoleId.ToString();
                claims.Add(new Claim("role", userRole));

                var accessToken = _tokenGenerator.GenerateAccessToken(claims);

                var refreshToken = _tokenGenerator.GenerateRefreshToken();

                _cache.Set($"RefreshToken_{user.UserId}", refreshToken, TimeSpan.FromDays(7));

                _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                var userInfo = new
                {
                    user.UserId,
                    user.Username,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Phone,
                    user.Address,
                    user.RoleId,
                    user.CreatedAt,
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //public async Task<IActionResult> HandleRefreshToken()
        //{
        //    var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];

        //    if (string.IsNullOrEmpty(refreshToken))
        //    {
        //        return Unauthorized("RefreshToken is not provide by system");
        //    }

        //    ClaimsPrincipal principal;
        //    try
        //    {
        //        principal = _tokenValidator.ValidateToken(refreshToken);
        //    }catch (Exception ex)
        //    {
        //        return Unauthorized($"Lỗi khác thực Refresh Token: {ex.Message}");
        //    }

        //    var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if(userId == null)
        //    {
        //        return Unauthorized("UserID not found in RefreshToken");
        //    }

        //    //if(!)
        //}
    }
}
