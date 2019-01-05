using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentApplication.Models
{
    public class CoursesEnrolled
    {
        public string CourseNum { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string DepartmentName { get; set; }
        public int Credits { get; set; }
        public string Grade { get; set; }
        public string Status { get; set; }
    }
}