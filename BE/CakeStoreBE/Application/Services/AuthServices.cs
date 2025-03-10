﻿using CakeStoreBE.Application.DTOs.LoginUserDTOs;
using CakeStoreBE.Application.DTOs.UsersDTOs;
using CakeStoreBE.Constants.Enums;
using CakeStoreBE.Domain;
using CakeStoreBE.Infrastructure.Database;
using CakeStoreBE.Utils.Hash;
using CakeStoreBE.Utils.JWTProcess.TokenGenerators;
using CakeStoreBE.Utils.JWTProcess.TokenValidators;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace CakeStoreBE.Application.Services
{

    public interface IAuthServices
    {
        public Task<IActionResult> HandleLogin(LoginUserDTO loginUserDTO);
        public Task<IActionResult> HandleRegister(RegisterUserDTO registerUserDTO);
        public Task<IActionResult> HandleRefreshToken();
        public Task<IActionResult> HandleCheckToken(string token);
        public PasswordResetTokenDTO? HandleGeneratePasswordResetToken(string email);
        public void HandleUpdatePassword(string newPassword);
    }
    public class AuthServices : ControllerBase, IAuthServices
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenGenerator _tokenGenerator;
        private List<User> _user = new List<User>();
        private readonly TokenValidator _tokenValidator;
        private readonly BakeStoreDbContext _context;

        public AuthServices(TokenGenerator tokenGenerator, IMemoryCache cache, IHttpContextAccessor httpContextAccessor, BakeStoreDbContext context, TokenValidator tokenValidator)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache; 
            _tokenGenerator = tokenGenerator;
            _user = new List<User>();
            _context = context;
            _tokenValidator = tokenValidator;
        }   

        public async Task<IActionResult> HandleRegister(RegisterUserDTO registerUserDTO)
        {
            if (_user != null && _user.Any(u => u.Username == registerUserDTO.UserName))
            {
                return BadRequest("Username already in use");
            }

            if (registerUserDTO.Password != registerUserDTO.ConfirmPassword)
            {
                return BadRequest("Password and ConfirmPassword do not match");
            }

            if(_user != null && _user.Any(u => u.Email == registerUserDTO.Email)){
                return BadRequest("Email already in user");
            }

            var rawData = $"{registerUserDTO.UserName}-{DateTime.UtcNow.Ticks}";
            var hashUserID = Hasher.HashWithSHA256(rawData);
            var hashPassword = Hasher.HashWithSHA256(registerUserDTO.Password);

            var user = new User
            {
                UserId = hashUserID,
                Username = registerUserDTO.UserName,
                Email = registerUserDTO.Email,
                Password = hashPassword,
                RoleId = 0,
                CreatedAt = DateTime.UtcNow,
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok("Register Successful");
        }

        public async Task<IActionResult> HandleLogin(LoginUserDTO _login)
        {
            try
            {
            var user = await _context.Users
                            .FirstOrDefaultAsync(u => u.Username == _login.Username);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid Username or Password" });
                }

                var password = Hasher.HashWithSHA256(_login.Password);
                if (password != user.Password)
                {
                    return Unauthorized("Invalid Username or Password");
                }

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId),
            new Claim(ClaimTypes.Name, user.Username),
        };

                // var userRole = user.RoleId.ToString();
                // claims.Add(new Claim("role", userRole));

                // foreach(var role in Role){
                //     claims.Add(new Claim(ClaimTypes.Role, role));
                // }

                //check role to Authorize

                var accessToken = _tokenGenerator.GenerateAccessToken(claims);
                var refreshToken = _tokenGenerator.GenerateRefreshToken();

                if (accessToken == null || refreshToken == null)
                {
                    return Unauthorized("Token generation failed");
                }

                _cache.Set($"RefreshToken_{user.UserId}", refreshToken, TimeSpan.FromDays(7));

                _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

                var accessTokenInfo = _tokenGenerator.GenerateAccessToken(claims);
                var refreshTokenInfo = _tokenGenerator.GenerateRefreshToken();


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
                    AccessToken = accessTokenInfo,
                    RefreshToken = refreshTokenInfo,
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> HandleRefreshToken()
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("RefreshToken is not provide by system");
            }

            ClaimsPrincipal principal;
            try
            {
                principal = _tokenValidator.ValidateToken(refreshToken);
            }
            catch (Exception ex)
            {
                return Unauthorized($"Error validation RefreshToken: {ex.Message}");
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("UserID not found in RefreshToken");
            }

            if(!_cache.TryGetValue($"RefreshToken_{userId}", out string storedRefreshToken)){
                return Unauthorized("Can't find RefreshToken in cache");
            }

            if(storedRefreshToken != refreshToken)
            {
                return Unauthorized("Refresh Token does not match storedRefreshToken");
            }

            var newAccessToken = _tokenGenerator.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

            _cache.Set($"RefreshToken_{userId}", newRefreshToken, TimeSpan.FromDays(7));

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Ok(new { accessToken = newAccessToken });

        }
        //Cant use!!!!!
        public async Task<IActionResult> HandleCheckToken(string token)
        {
            try
            {
                if (_tokenValidator == null)
                {
                    throw new NullReferenceException("_tokenValidator is null. Ensure it is properly injected.");
                }

                var claimsPrincipal = _tokenValidator.ValidateToken(token);
                if (claimsPrincipal == null)
                {
                    Console.WriteLine("Token validation failed: claimsPrincipal is null.");
                    return Unauthorized(new { message = "Invalid Token" });
                }

                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    throw new NullReferenceException("ClaimTypes.NameIdentifier is missing in token.");
                }

                var userIdString = userIdClaim.Value;
                if (string.IsNullOrEmpty(userIdString))
                {
                    return Unauthorized(new { message = "Invalid User ID format in token." });
                }

                if (_context == null || _context.Users == null)
                {
                    throw new NullReferenceException("_context or _context.Users is null. Check database connection.");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userIdString);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

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

                return Ok(new { message = "Token is valid", user = userInfo });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HandleCheckToken: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }


        public PasswordResetTokenDTO? HandleGeneratePasswordResetToken(string email){
            //Using email to validate User in database to generate token
            var user = _context.Users.FirstOrDefault(u => u.Email == email);                     
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var user_password_reset_token = _tokenGenerator.GeneratePasswordResetToken(email);
            if(user_password_reset_token == null || user_password_reset_token.ExpiryDate < DateTime.UtcNow){
                throw new Exception("Generate token failed");
            }

            var token_respone = _httpContextAccessor.HttpContext?.Response;
            if(token_respone != null){
                token_respone.Cookies.Append("PasswordResetToken", user_password_reset_token.Token, new CookieOptions {
                    HttpOnly = true, //prevent access from Javascript
                    Secure = true, //prevent access from http
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SameSite = SameSiteMode.None
                });

                token_respone.Cookies.Append("PasswordResetTokenEmail", email, new CookieOptions {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SameSite = SameSiteMode.None
                });

            }
            return user_password_reset_token;
        }

        public void HandleUpdatePassword(string newPassword)
        {
            //Check Context null
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new Exception("HTTP Context is null");
            }

            //Take Request and Response
            var request = httpContext.Request;
            var response = httpContext.Response;

            if (request == null || response == null)
            {
                throw new Exception("Request or Response is null");
            }

            // Take token from cookie
            if (!request.Cookies.TryGetValue("PasswordResetToken", out var token) ||
                !request.Cookies.TryGetValue("PasswordResetTokenEmail", out var email) ||
                string.IsNullOrWhiteSpace(token) ||
                string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Token or Email not found in cookies");
            }

            // Check User in database
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new Exception("User not found");
            }

           
            user.Password = Hasher.HashWithSHA256(newPassword);
            _context.Users.Update(user);
            _context.SaveChanges();

            // Delete Token
            response.Cookies.Delete("PasswordResetToken");
            response.Cookies.Delete("PasswordResetTokenEmail");
        }






    }
}
