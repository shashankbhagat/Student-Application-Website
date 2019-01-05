using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentApplication.Models
{
    public class RegisterCourseVM
    {
        public List<CoursesEnrolled> course { get; set; }
        public List<SelectListItem> semList { get; set; }
        public string semselected { get; set; }
        public List<SelectListItem> CList { get; set; }
        public string CSelected { get; set; }
    }
}