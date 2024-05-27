﻿using Microsoft.AspNetCore.Identity;

namespace Yugioh.Server.Model
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
    }
}