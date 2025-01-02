using CakeStoreBE.Application.DTOs.UsersDTOs;
using CakeStoreBE.Application.Services;
using CakeStoreBE.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;

namespace CakeStoreBE.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BakeStoreDbContext _context;
        private readonly IUserServices _userServices;

        public UserController(BakeStoreDbContext context, IUserServices userServices)
        {
            _context = context;
            _userServices = userServices;
        }

        [HttpGet("Get All User")]
        public async Task<IActionResult> GetAllUser()
        {
            return await _userServices.HandleGetAllUser();
        }

        [HttpGet("Get User By Id")]
        public async Task<IActionResult> GetUserById([FromQuery]string userId)
        {
            return await _userServices.HandleGetUserById(userId);
        }

        [HttpPost("Create User")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO userDTO)
        {
            return await _userServices.HandleCreateUser(userDTO);
        }

        [HttpPut("Update User")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO _updateUserDTO)
        {
            return await _userServices.HandleUpdateUser(_updateUserDTO);
        }

        [HttpDelete("Delete User By Id")]
        public async Task<IActionResult> DeleteUserById(string userId)
        {
            return await _userServices.HandleRemoveUserById(userId);
        }


    }
}
