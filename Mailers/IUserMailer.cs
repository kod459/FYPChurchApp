using Mvc.Mailer;
using PIMS.Entities;

namespace PIMS.Mailers
{ 
    public interface IUserMailer
    {
		MvcMailMessage Welcome();
		MvcMailMessage PasswordReset();
        MvcMailMessage AppointmentDetails(Appointments appointments, string church, string admin, string email);
        MvcMailMessage AppointmentUpdate(Appointments appointments, string church, string admin, string email);
        MvcMailMessage VolunteerCeremony(Appointments appointments, string church, string volunteer);

    }
}