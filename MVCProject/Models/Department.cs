using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCProject.Models
{
    public class Department
    {
        [Key]
        public int Dept_Id { get; set; }
        [Display(Name ="Department Name")]
        public string Name { get; set; }

       public virtual ICollection<Faculty> factulty { get; set; }

        public virtual ICollection<Student> student { get; set; }
    }
}