using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Administration
    {
        [Key]
        public int AdministrationId { get; set; }

        [Required(ErrorMessage = "Name of Admin is required")]
        public string AdministratorName { get; set; }

        [Required(ErrorMessage = "Username of Admin is required")]
        public string AdminUsername { get; set; }
        
        public string Position { get; set; }

        [Required(ErrorMessage = "Phone Number of Applicant is required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Phone Number must be 10 digits")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Email of Applicant is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [ForeignKey("AdminChurch")]
        public int ChurchId { get; set; }

        //For FK I think
        public virtual Church AdminChurch { get; set; }

    }
}