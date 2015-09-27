using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCConfigurator.UI.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [StringLength(32,MinimumLength=6)]
        public string Password { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
        public UserDetailsModel UserDetails { get; set; }
    }

    public class UserDetailsModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public string Company { get; set; }
    }
}