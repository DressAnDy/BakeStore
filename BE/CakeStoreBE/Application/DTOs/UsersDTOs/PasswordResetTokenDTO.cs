namespace CakeStoreBE.Application.DTOs.UsersDTOs
{
    public class PasswordResetTokenDTO
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime ExpiryDate {get; set;}
    }
}