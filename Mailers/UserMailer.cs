using Mvc.Mailer;
using PIMS.Entities;
using PIMS.Controllers;
using System;

namespace PIMS.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage Welcome()
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "Welcome";
				x.ViewName = "Welcome";
				x.To.Add("some-email@example.com");
			});
		}
 
		public virtual MvcMailMessage PasswordReset()
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "PasswordReset";
				x.ViewName = "PasswordReset";
				x.To.Add("some-email@example.com");
			});
		}

        public virtual MvcMailMessage AppointmentDetails(Appointments appointment, string church, string admin, string adminEmail)
        {
            ViewBag.Details = appointment.DetailsOfAppointment;
            ViewBag.Date = appointment.DateOfAppointment;
            ViewBag.Room = "Not Confirmed";
            ViewBag.NameOfApplicant = appointment.NameOfApplicant;
            ViewBag.PhoneNumber = appointment.ApplicantPhoneNumber;
            ViewBag.Email = appointment.ApplicantEmail;
            ViewBag.Admin = admin;
            ViewBag.Church = church;
            ViewBag.Confirmed = appointment.Confirmed;
            if(ViewBag.Confirmed == false)
            {
                ViewBag.Confirmed = "Not Confirmed";
            }
            else
            {
                ViewBag.Confirmed = "Confirmed";
            }
            return Populate(x =>
            {
                x.Subject = "Appointment Detail";
                x.ViewName = "AppointmentDetails";
                //x.To.Add(appointment.ApplicantEmail);
                x.To.Add(adminEmail);
            });
        }

        public virtual MvcMailMessage AppointmentUpdate(Appointments appointment, string church, string admin, string adminEmail)
        {
            ViewBag.Details = appointment.DetailsOfAppointment;
            var date = appointment.DateOfAppointment;
            //var time = appointment.TimeOfAppointment;
            ViewBag.Date = appointment.DateOfAppointment;
            //ViewBag.Time = time.ToShortTimeString();
            ViewBag.Room = appointment.RoomType;
            ViewBag.NameOfApplicant = appointment.NameOfApplicant;
            ViewBag.PhoneNumber = appointment.ApplicantPhoneNumber;
            ViewBag.Email = appointment.ApplicantEmail;
            ViewBag.Admin = admin;
            ViewBag.Church = church;
            ViewBag.Confirmed = appointment.Confirmed;
            if (ViewBag.Confirmed == false)
            {
                ViewBag.Confirmed = "Not Confirmed";
            }
            else
            {
                ViewBag.Confirmed = "Confirmed";
            }
            return Populate(x =>
            {
                x.Subject = "Appointment Update";
                x.ViewName = "AppointmentUpdate";
                //x.To.Add(appointment.ApplicantEmail);
                x.To.Add(adminEmail);
            });
        }

        public virtual MvcMailMessage VolunteerCeremony(Appointments appointments, string church, string volunteer)
        {
            ViewBag.Details = appointments.DetailsOfAppointment;
            
            ViewBag.Date = appointments.DateOfAppointment;
            
            
            ViewBag.NameOfVolunteer = volunteer;


            ViewBag.Church = church;
            
            return Populate(x =>
            {
                x.Subject = "Ceremony Update";
                x.ViewName = "CeremonyUpdate";
                //x.To.Add(appointment.ApplicantEmail);
                x.To.Add(volunteer);
            });
        }
    }
}