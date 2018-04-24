using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Volunteer
    {
        [Key]
        public int VolunteerId { get; set; }

        [Required(ErrorMessage = "Volunteer Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Volunteer Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Volunteer Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public bool GardaVetted { get; set; }

        
        public string VolunteerRole { get; set; }

        [Required(ErrorMessage = "Volunteer Phone Number is required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Phone Number must be 10 digits")]
        public string VolunteerPhoneNumber { get; set; }

        [ForeignKey("Church")]
        public int ChurchId { get; set; }

        //Foreign Key
        public virtual Church Church { get; set; }

        public virtual ICollection<Appointments> Appointments { get; set; }

        public Volunteer()
        {
            this.Appointments = new HashSet<Appointments>();
        }

    }
}