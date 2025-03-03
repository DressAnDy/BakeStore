namespace CakeStoreBE.Application.DTOs.UsersDTOs
{
    public class PasswordRequest {
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string NewPassword { get; set; }
    }
}