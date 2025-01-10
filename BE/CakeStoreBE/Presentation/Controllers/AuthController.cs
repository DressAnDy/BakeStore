using CakeStoreBE.Application.DTOs.LoginUserDTOs;
using CakeStoreBE.Application.DTOs.UsersDTOs;
using CakeStoreBE.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CakeStoreBE.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            return await _authServices.HandleRegister(registerUserDTO);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            return await _authServices.HandleLogin(loginUserDTO);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            return await _authServices.HandleRefreshToken();
        }

        [HttpGet("check-token")]
        public async Task<IActionResult> CheckToken(string token)
        {
            return await _authServices.HandleCheckToken(token);
        }
    }   
}
