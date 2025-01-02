namespace CakeStoreBE.Application.DTOs.PayloadDTOs
{
    public class PayloadDTO
    {
        public string UserId {get; set;}
        public string Email { get; set;}
        public string IsAdmin { get; set;}
        public string Role { get; set;}
        public DateTime Expiration { get; set;}
    }
}
