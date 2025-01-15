using CakeStoreBE.Application.DTOs.UsersDTOs;
using CakeStoreBE.Domain;
using CakeStoreBE.Infrastructure.Database;
using CakeStoreBE.Utils.Hash;
using CakeStoreBE.Constants.Enums;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;



namespace CakeStoreBE.Application.Services
{
    public interface IUserServices
    {
        //LING: là một công cụ cho phép bạn làm việc với cơ sở dữ liệu SQL Server, thông qua các truy vấn LINQ, thay vì viết mã SQL thủ công. Đây là một cách tiếp cận ORM ( Object-Relational Mapping ) gọn gàng

        //Task là đại diện cho một hoạt động bất đồng bộ ( asynchronous ) trong async / await

        public Task<IActionResult> HandleCreateUser(CreateUserDTO userDTO); //Create này chỉ sử dụng khi có quyền 
        public Task<IActionResult> HandleUpdateUser(UpdateUserDTO userDTO);
        public Task<IActionResult> HandleRemoveUserById(string userId);
        public Task<IActionResult> HandleGetUserById(string userId);
        public Task<IActionResult> HandleGetAllUser();
    }

    public class UserServices : ControllerBase, IUserServices
    {
        private readonly BakeStoreDbContext _context;
        
        public UserServices(BakeStoreDbContext context)
        {
            _context = context;
        }


        //Handle tạo User
        public async Task<IActionResult> HandleCreateUser(CreateUserDTO _userDTO)
        {
            var exsitingUser = _context.Users.Any(u => u.Username == _userDTO.Username || u.Email == _userDTO.Email);

            if(exsitingUser)
            {
                throw new Exception("Username or Email already exists.");
            }
            //xử lý UserId với mạng SHA256
            var rawUserId = Guid.NewGuid().ToString();
            var hashUserId = Hasher.HashWithSHA256(rawUserId);

            var newUser = new User
            {
                UserId = hashUserId,
                Username = _userDTO.Username,
                Email = _userDTO.Email,
                FirstName = _userDTO.FirstName,
                LastName = _userDTO.LastName,
                Phone = _userDTO.Phone,
                Address = _userDTO.Address,
                RoleId = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }

        public async Task<IActionResult> HandleUpdateUser(UpdateUserDTO updateUserDTO)
        {
            //Tìm User ( đã register rồi ) sau đó update lên với những thông tin mới


            //****************************************************************
            //FirstOrDefault: là phương thức của đồng bộ ( Synchronous )
            // Dùng để try vấn phần tử đầu tiên trong một tập hợp thỏa mãn điều kiện
            // Nó sẽ được dùng để try vấn data trên bộ nhớ ( in-memory) như List<T> hoặc là IEnumerable<T>
            // Và Không dùng tới await
            //****************************************************************

            //FirstOrDefaultAsync: là phương thức của bất đồng bộ ( Asynchronous )
            // Dùng để try vấn database qua EF Core hoặc nguồn dữ liệu hỗ trợ bất đồng bộ
            // Không chặn luồng chính khi thực hiện truy vấn, thay vào đó dùng cơ chế task và cần từ khóa await


            var exsitingUser = await _context.Users
                                             .FirstOrDefaultAsync(u => u.UserId == updateUserDTO.UserId);

            if(exsitingUser == null)
            {
                return NotFound("User not found");
            }
            //FirstName
            exsitingUser.FirstName = string.IsNullOrEmpty(updateUserDTO.FirstName) ? exsitingUser.FirstName : updateUserDTO.FirstName;
            //LastName
            exsitingUser.LastName = string.IsNullOrEmpty(updateUserDTO.LastName) ? exsitingUser.LastName : updateUserDTO.LastName;
            //Email
            exsitingUser.Email = string.IsNullOrEmpty(updateUserDTO.Email) ? exsitingUser.Email : updateUserDTO.Email;
            //Phone
            exsitingUser.Phone = string.IsNullOrEmpty(updateUserDTO.Phone) ? exsitingUser.Phone : updateUserDTO.Phone;
            //Address
            exsitingUser.Address = string.IsNullOrEmpty(updateUserDTO.Address) ? exsitingUser.Address : updateUserDTO.Address;

            //Cập nhật thời gian khi update
            exsitingUser.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok("User Update Successfull");
        }


        //Tìm User dựa vào id được cung cấp
        public async Task<IActionResult> HandleGetUserById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("User is required");
            }

            var user = await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new 
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = $"{u.FirstName} {u.LastName}".Trim(),
                    Phone = u.Phone,
                    Address = u.Address,
                    RoleId = u.RoleId,
                    CreateAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                })
                .FirstOrDefaultAsync();
                
             if (user == null)
            {
                return BadRequest("User Not Found");
            }
            
            return Ok(user);
        }

        public async Task<IActionResult> HandleGetAllUser()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = $"{u.FirstName} {u.LastName}".Trim(),
                    Phone = u.Phone,
                    Address = u.Address,
                    RoleId = u.RoleId,
                    CreateAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                })
                .ToListAsync();
            if(users == null || !users.Any())
            {
                return BadRequest("User Not Found");
            }

            return Ok(users);
        }

        public async Task<IActionResult> HandleRemoveUserById(string userId)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if(user == null)
                {
                    return BadRequest("User not found");
                }

                _context.Users.Remove(user);

                return Ok("Remove Successfull");
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
