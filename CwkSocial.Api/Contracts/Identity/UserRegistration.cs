﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CwkSocial.Api.Contracts.Identity
{
    public class UserRegistration
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)] 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string CurrentCity { get; set; }
    }
}
