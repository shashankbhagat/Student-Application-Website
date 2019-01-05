using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentApplication.Models
{
    public class StudentVM
    {
        public long StudentId { get; set; }

        [Required(ErrorMessage ="{0} required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} required")]        
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} required")]
        public string Street { get; set; }

        [Required(ErrorMessage = "{0} required")]
        public string City { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [MaxLength(2,ErrorMessage ="{0} must be 2 characters only")]
        [MinLength(2)]
        public string State { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address...")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="{0} invalid")]
        public string Telephone { get; set; }        
    }
}