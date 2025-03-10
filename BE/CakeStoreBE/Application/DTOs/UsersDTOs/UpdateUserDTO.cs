﻿namespace CakeStoreBE.Application.DTOs.UsersDTOs
{
    public class UpdateUserDTO
    {
        public string UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Phone {  get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address {  get; set; } = string.Empty;
    }
}
