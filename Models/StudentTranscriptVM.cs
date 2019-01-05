using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentApplication.Models
{
    public class StudentTranscriptVM
    {
        public string CourseNum { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string DepartmentName { get; set; }        
        public string Grade { get; set; }
        public string SemeterId { get; set; }
        public string Name { get; set; }
    }
}