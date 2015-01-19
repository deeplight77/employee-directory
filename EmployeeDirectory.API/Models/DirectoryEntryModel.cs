using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeDirectory.API.Models
{
    public class DirectoryEntryModel
    {
        [Key]
        [Required]
        public string UserName { get; set; }

        [Display(Name="Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Office Location")]
        public string OfficeLocation { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Office Phone Number")]
        [Phone(ErrorMessage = "Please add a correct phone number in the form (xxx) xxx-xxxx.")]
        public string OfficePhoneNumber { get; set; }

        [Display(Name = "Personal Phone Number")]
        public string PersonalPhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }
    }
}