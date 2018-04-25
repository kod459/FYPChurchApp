using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PIMS.Entities
{
    public class Appointments
    {
        [Key]
        public int AppointmentId { get; set; }

        public string DetailsOfAppointment { get; set; }

        public int? Fee { get; set; }

        public string RoomType { get; set; }


        [Required(ErrorMessage = "Name of Applicant is required")]
        public string NameOfApplicant { get; set; }

        [Required(ErrorMessage = "Phone Number of Applicant is required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Phone Number must be 10 digits")]
        public string ApplicantPhoneNumber { get; set; }

        [Required(ErrorMessage = "Email of Applicant is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string ApplicantEmail { get; set; }

        public int Slots { get; set; }

        [Required(ErrorMessage = "Date of Appointment is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DateOfAppointment { get; set; }

        public string ThemeColour { get; set; }

        public Boolean Confirmed { get; set; }

        [ForeignKey("Admins")]
        public int AdministrationId { get; set; }

        [ForeignKey("Church")]
        public int ChurchId { get; set; }

        public virtual Church Church { get; set; }

        public virtual Administration Admins { get; set; }

        public ICollection <Volunteer> Volunteers { get; set; }

        public Appointments()
        {
            this.Volunteers = new HashSet<Volunteer>();
        }
    }
}