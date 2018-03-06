﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PIMS.DataAccess;
using PIMS.Entities;
using Mvc.Mailer;
using Google.Apis.Calendar.v3;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Calendar.v3.Data;
using PIMS.Mailers;
using PIMS.ViewModel;

namespace PIMS.Controllers
{
    public class AppointmentsController : Controller
    {
        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }

        //static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        //static string ApplicationName = "ChurchWebApp";
        private ChurchDBContext db = new ChurchDBContext();

        // GET: Appointments
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Church);
            return View(appointments.ToList());
        }

        // GET: Appointments
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult IndexOfCeremonies()
        {
            var appointments = db.Appointments.Include(a => a.Church);
            return View(appointments.ToList());
        }

        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Calendar()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            //var events = db.Appointments.ToList();

            //return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            using (var context = new ChurchDBContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                return new JsonResult { Data = context.Appointments.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        [HttpPost]
        public JsonResult SaveVolunteerToAppointment(Appointments apps)
        {
            var status = false;
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName");
            ViewBag.Details = new SelectList(new[] { "Confession", "Baptism Meeting", "Wedding Meeting", "Personal Meeting" });

            if (apps.AppointmentId > 0)
            {
                //Update the event
                var v = db.Appointments.Where(a => a.AppointmentId == apps.AppointmentId).FirstOrDefault();
                if (v != null)
                {
                    v.DetailsOfAppointment = apps.DetailsOfAppointment;
                    v.DateOfAppointment = apps.DateOfAppointment;
                    v.Admins.AdministratorName = apps.Admins.AdministratorName;
                }
            }
            else
            {
                db.Appointments.Add(apps);
            }

            db.SaveChanges();
            status = true;


            return new JsonResult { Data = new { status = status } };
        }

        // GET: Appointments/Details/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }

        // GET: Appointments/Details/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult DetailsOfCeremony(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }

        //this method receives the postback when the "Confirm Appointment" button is pressed, and updated the appointment details. Then it returns to the user to the same screen, where this time they should see the "Appointment Confirmed" message instead of the button
        [HttpPost]
        public ActionResult ConfirmAppointment(int? id)
        {
            var appointments = db.Appointments.Find(id);
            appointments.Confirmed = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Appointments");
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {

            ViewBag.Rooms = new SelectList(new[] { "Sacristy", "Confession Room", "GP Room", "Parish Office" });

            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name");

            ViewBag.Details = new SelectList(new[] { "Confession", "Baptism Meeting", "Wedding Meeting", "Personal Meeting" });

            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName");
            return View();
        }

        
        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,DetailsOfAppointment,RoomType,DateOfAppointment,Confirmed,NameOfApplicant, ThemeColour, ApplicantPhoneNumber, ApplicantEmail,AdministrationId,ChurchId")] Appointments appointments)
        {
            if (ModelState.IsValid)
            { 
                //Pick Time
                //Show Calender to Priest
                //Pick slot to be not available

                bool checkUserLoggedIn = (System.Web.HttpContext.Current.User != null) 
                    && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;


                appointments = ValidateEmail(appointments);

                
                //var currentBooking = db.Appointments
                //    .Any(b => (appointments.TimeOfAppointment == b.TimeOfAppointment
                //     && appointments.DateOfAppointment == b.DateOfAppointment
                //     && appointments.AdministrationId == b.AdministrationId));

                var currentBooking = db.Appointments
                    .Any(b => (appointments.DateOfAppointment == b.DateOfAppointment
                     && appointments.AdministrationId == b.AdministrationId));

                //var checkCeremony = db.Ceremonies
                //    .Any(b => (appointments.DateOfAppointment == b.DateOfCeremony
                //    && appointments.AdministrationId == b.AdministrationId));

                var checkPriestHolidays = db.PriestLeave
                    .Any(b => (appointments.DateOfAppointment >= b.StartDate && appointments.DateOfAppointment <= b.EndDate));

                if (currentBooking)// && checkCeremony)
                {
                    TempData["Error"] = "Room, Date and Admin booked for appointment";
                }
                else if(checkPriestHolidays)
                {
                    TempData["Error"] = "Priest on Leave";
                }
                else
                {
                    db.Appointments.Add(appointments);
                    db.SaveChanges();

                    var app = (from a in db.Appointments
                                             where appointments.AppointmentId == a.AppointmentId
                                             select a).FirstOrDefault();

                    string churchName = (from c in db.Churches
                                      where c.ChurchId == appointments.ChurchId
                                      select c.Name).FirstOrDefault();

                    string adminName = (from a in db.Admins
                                     where a.AdministrationId == appointments.AdministrationId
                                     select a.AdministratorName).FirstOrDefault();

                    string adminEmail = (from a in db.Admins
                                         where a.AdministrationId == appointments.AdministrationId
                                         select a.EmailAddress).FirstOrDefault();



                    UserMailer.AppointmentDetails(app, churchName, adminName, adminEmail).Send(); //Send() extension method: using Mvc.Mailer

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Rooms = new SelectList(new[] { "Sacristy", "Confession Room", "GP Room", "Parish Office" });
            ViewBag.Details = new SelectList(new[] { "Confession", "Baptism Meeting", "Wedding Meeting", "Personal Meeting" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", appointments.ChurchId);
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", appointments.AdministrationId);
            return View(appointments);
        }


        // GET: Appointments/Create
        public ActionResult CreateCeremony()
        {

            //ViewBag.Rooms = new SelectList(new[] { "Sacristy", "Confession Room", "GP Room", "Parish Office" });

            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name");

            //ViewBag.Volunteer = new SelectList(db.Volunteers, "VolunteerId", "Name");

            ViewBag.Details = new SelectList(new[] { "Confession", "Baptism", "Wedding", "Communion", "Confirmation"});

            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName");

            return View();
        }


        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCeremony([Bind(Include = "AppointmentId,DetailsOfAppointment,DateOfAppointment,Confirmed,Fee,ThemeColour,AdministrationId,ChurchId")] Appointments appointments)
        {
            if (ModelState.IsValid)
            {
                //Pick Time
                //Show Calender to Priest
                //Pick slot to be not available - Could do a LINQ query to a new table called Priest Holidays????

                bool checkUserLoggedIn = (System.Web.HttpContext.Current.User != null)
                    && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;


                appointments = ValidateEmail(appointments);


                //var currentBooking = db.Appointments
                //    .Any(b => (appointments.TimeOfAppointment == b.TimeOfAppointment
                //     && appointments.DateOfAppointment == b.DateOfAppointment
                //     && appointments.AdministrationId == b.AdministrationId));

                var currentBooking = db.Appointments
                    .Any(b => (appointments.DateOfAppointment == b.DateOfAppointment
                     && appointments.AdministrationId == b.AdministrationId));

                //var checkCeremony = db.Ceremonies
                //    .Any(b => (appointments.DateOfAppointment == b.DateOfCeremony
                //    && appointments.AdministrationId == b.AdministrationId));

                var checkPriestHolidays = db.PriestLeave
                    .Any(b => (appointments.DateOfAppointment >= b.StartDate && appointments.DateOfAppointment <= b.EndDate));

                if (currentBooking)// && checkCeremony)
                {
                    TempData["Error"] = "Room, Date and Admin booked for appointment";
                }
                else if (checkPriestHolidays)
                {
                    TempData["Error"] = "Priest on Leave";
                }
                else
                {
                    db.Appointments.Add(appointments);
                    db.SaveChanges();

                    var app = (from a in db.Appointments
                               where appointments.AppointmentId == a.AppointmentId
                               select a).FirstOrDefault();

                    string churchName = (from c in db.Churches
                                         where c.ChurchId == appointments.ChurchId
                                         select c.Name).FirstOrDefault();

                    string adminName = (from a in db.Admins
                                        where a.AdministrationId == appointments.AdministrationId
                                        select a.AdministratorName).FirstOrDefault();

                    string adminEmail = (from a in db.Admins
                                         where a.AdministrationId == appointments.AdministrationId
                                         select a.EmailAddress).FirstOrDefault();



                    UserMailer.AppointmentDetails(app, churchName, adminName, adminEmail).Send(); //Send() extension method: using Mvc.Mailer

                    return RedirectToAction("Index", "Home");
                }
            }


            ViewBag.Rooms = new SelectList(new[] { "Sacristy", "Confession Room", "GP Room", "Parish Office" });
            ViewBag.Details = new SelectList(new[] { "Confession", "Baptism Meeting", "Wedding Meeting", "Personal Meeting" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", appointments.ChurchId);
            //ViewBag.VolunteerId = new SelectList(db.Volunteers, "VolunteerId", "Name", appointments.VolunteerId);
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", appointments.AdministrationId);
            return View(appointments);
        }



        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            ViewBag.Rooms = new SelectList(new[] { "Sacristy", "Confession Room", "GP Room", "Parish Office" });
            ViewBag.Details = new SelectList(new[] { "Confession", "Baptism Meeting", "Wedding Meeting", "Personal Meeting" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", appointments.ChurchId);
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", appointments.AdministrationId);
            return View(appointments);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentId,DetailsOfAppointment,RoomType,DateOfAppointment,Confirmed,NameOfApplicant, ApplicantPhoneNumber, ApplicantEmail,AdministrationId,ChurchId")] Appointments appointments)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(appointments).State = EntityState.Modified;
                db.SaveChanges();

                var app = (from a in db.Appointments
                           where appointments.AppointmentId == a.AppointmentId
                           select a).FirstOrDefault();

                string churchName = (from c in db.Churches
                                     where c.ChurchId == appointments.ChurchId
                                     select c.Name).FirstOrDefault();

                string adminName = (from a in db.Admins
                                    where a.AdministrationId == appointments.AdministrationId
                                    select a.AdministratorName).FirstOrDefault();

                string adminEmail = (from a in db.Admins
                                     where a.AdministrationId == appointments.AdministrationId
                                     select a.EmailAddress).FirstOrDefault();



                UserMailer.AppointmentUpdate(app, churchName, adminName, adminEmail).Send(); //Send() extension method: using Mvc.Mailer
                return RedirectToAction("Details", new { id = appointments.AppointmentId });
            }

            ViewBag.Rooms = new SelectList(new[] { "Sacristy", "Confession Room", "GP Room", "Parish Office" });
            ViewBag.Details = new SelectList(new[] { "Confession", "Baptism Meeting", "Wedding Meeting", "Personal Meeting" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", appointments.ChurchId);
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", appointments.AdministrationId);
            return View(appointments);
        }

        // GET: Appointments/Delete/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }

        // POST: Appointments/Delete/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointments appointments = db.Appointments.Find(id);
            db.Appointments.Remove(appointments);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        public Appointments ValidateEmail(Appointments up)
        {
            if (up.ApplicantEmail == null)
            {
                TempData["ErrorEmail"] = "A valid email must be entered";
            }
            else
            {
                try
                {
                    string domain = up.ApplicantEmail.ToString().ToLower().Split('@').ElementAt(1);
                    if (domain != "gmail.com" || domain!= "hotmail.com")
                    {
                        TempData["ErrorEmail"] = "A valid Email address must be entered";
                    }
                }
                catch (Exception e)
                {
                    TempData["ErrorEmail"] = "A valid Email address must be entered";
                }

            }
            return up;
        }


    }
}
