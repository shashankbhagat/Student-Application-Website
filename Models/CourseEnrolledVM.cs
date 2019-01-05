using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentApplication.Models
{
    public class CourseEnrolledVM
    {
        public List<CoursesEnrolled> CEList { get; set; }        
        public List<SelectListItem> SList { get; set; }
        public string semSelected { get; set; } //semester selected
    }
}