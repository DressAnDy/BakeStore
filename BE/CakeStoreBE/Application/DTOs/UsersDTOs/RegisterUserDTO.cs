﻿namespace CakeStoreBE.Application.DTOs.UsersDTOs
{
    public class RegisterUserDTO
    {
        public string UserName { get; set; }
        public string Email {get; set; }
        public string Password { get; set; }
        public string ConfirmPassword {  get; set; }
    }
}
