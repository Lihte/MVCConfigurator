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
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public UserDetailsModel UserDetails { get; set; }
    }

    public class UserDetailsModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
    }
}