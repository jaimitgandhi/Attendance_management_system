using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Models
{
    public class Admin
    {
       [Key]
        public int Admin_Id { get; set; }
        [Required(ErrorMessage = "Email Must required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Must required")]
        public string Password { get; set; }
    }
}