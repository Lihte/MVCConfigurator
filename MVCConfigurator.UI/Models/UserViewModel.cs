using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCConfigurator.UI.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}