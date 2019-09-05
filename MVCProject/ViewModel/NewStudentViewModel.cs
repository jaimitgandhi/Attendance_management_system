using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCProject.Models;
namespace MVCProject.ViewModel
{
    public class NewStudentViewModel
    {
        public IEnumerable<Department> department { get; set; }
        public Student student { get; set; }
    }
}