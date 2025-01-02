 namespace CakeStoreBE.Application.DTOs.UsersDTOs
{
    public class UserDTO
    {
        public string UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
    
        public string Address {  get; set; } = string.Empty;

        public string RoleId { get; set; }
    }

    public class CreateUserDTO
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string RoleId { get; set; }
    }
}
