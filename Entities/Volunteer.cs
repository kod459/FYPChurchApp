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

        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool GardaVetted { get; set; }

        public string VolunteerRole { get; set; }

        public string VolunteerPhoneNumber { get; set; }

        [ForeignKey("Church")]
        public int ChurchId { get; set; }

        //Foreign Key
        public virtual Church Church { get; set; }

        public virtual ICollection<Appointments> Appointments { get; set; }

        //public virtual Appointments app { get; set; }

        public Volunteer()
        {
            this.Appointments = new HashSet<Appointments>();
        }

    }
}