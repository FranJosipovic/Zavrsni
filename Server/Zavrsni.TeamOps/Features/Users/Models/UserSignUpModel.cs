﻿namespace Zavrsni.TeamOps.Features.Users.Models
{
    public class UserSignUpModel
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;    
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
