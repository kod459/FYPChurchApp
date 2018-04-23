using System;

namespace PIMS.ViewModel
{
    public class VolunteerCeremonyVM
    {
        public int AppointmentId { get; set; }
        public string DetailsOfAppointment { get; set; }
        public int Slots { get; set; }
        public DateTime DateOfAppointment { get; set; }
        public bool Assigned { get; set; }
    }
}