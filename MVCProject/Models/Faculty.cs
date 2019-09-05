using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Models
{
    public class Faculty
    {
        [Key]
        public int Fact_Id { get; set; }
        [Required(ErrorMessage = "Email Must required")]
        //[Remote("IsUserExists", "Faculties", ErrorMessage = "Email already in use")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Name Must required")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Only Alphabets allowed.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password Must required")]
        public string Password { get; set; }
        [ForeignKey("dept")]
        [Required]
        public int Dept_Id { get; set; }

        public virtual Department dept { get; set; }
    }
}