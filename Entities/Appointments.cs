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

        public string NameOfApplicant { get; set; }

        public string ApplicantPhoneNumber { get; set; }

        public string ApplicantEmail { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DateOfAppointment { get; set; }

        public string ThemeColour { get; set; }

        public Boolean Confirmed { get; set; }

        [ForeignKey("Admins")]
        public int AdministrationId { get; set; }

        [ForeignKey("Church")]
        public int ChurchId { get; set; }

        //[ForeignKey("Volunteers")]
        //public int VolunteerId { get; set; }

        public virtual Church Church { get; set; }

        public virtual Administration Admins { get; set; }

        public ICollection <Volunteer> Volunteers { get; set; }

        public Appointments()
        {
            this.Volunteers = new HashSet<Volunteer>();
        }
    }
}