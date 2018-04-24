using System;
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
using PIMS.Mailers;
using PIMS.ViewModel;
using System.Web.Security;
using System.Web.UI;
using System.Text.RegularExpressions;

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

        private ChurchDBContext db = new ChurchDBContext();

        // GET: Appointments
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Index()
        {
            string username = Membership.GetUser().UserName;

            var getAdmin = (from a in db.Admins
                            where username == a.AdminUsername
                            select a.AdministrationId).SingleOrDefault();


            var apps = (from a in db.Appointments
                          where getAdmin == a.AdministrationId
                          select a).Include(a=> a.Church).ToList().OrderByDescending(a=> a.DateOfAppointment);

            return View(apps);
        }

        // GET: Appointments
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult IndexOfCeremonies()
        {

            string username = Membership.GetUser().UserName;

            var getAdmin = (from a in db.Admins
                            where username == a.AdminUsername
                            select a.AdministrationId).SingleOrDefault();


            var apps = (from a in db.Appointments
                        where getAdmin == a.AdministrationId
                        select a).Include(a => a.Church).ToList().OrderByDescending(a => a.DateOfAppointment);


            //var appointments = db.Appointments.Include(a => a.Church);
            return View(apps);
        }

        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Calendar()
        {
            string username = Membership.GetUser().UserName;

            var getAdmin = (from a in db.Admins
                            where username == a.AdminUsername
                            select a.AdministrationId).SingleOrDefault();


            //var events = (from a in db.Appointments
            //              where getAdmin == a.AdministrationId
            //              select a).First();

            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", Location = OutputCacheLocation.None)]
        public JsonResult GetEvents()
        {
            string username = Membership.GetUser().UserName;

            var getAdmin = (from a in db.Admins
                            where username == a.AdminUsername
                            select a.AdministrationId).SingleOrDefault();

            
            var events = (from a in db.Appointments
                          where getAdmin == a.AdministrationId
                          select a).ToList();

            if(events == null)
            {
                return new JsonResult { Data = "No Appointments or Ceremonies for User", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            { 
             return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


        }

        [HttpPost]
        public JsonResult SaveAppointment(Appointments a)
        {
            var status = false;
            using (ChurchDBContext db = new ChurchDBContext())
            {

                db.Appointments.Add(a);
                db.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }


        public JsonResult UnAvailableSlots()
        {
            var events = (from a in db.Appointments
                          select a).ToList();

            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult HomePageAppointments()
        {
            return View();
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
        [Authorize(Roles = "Parish Admin, Priest, Administrator, Volunteer")]
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
        [Authorize(Roles = "Parish Admin, Priest, Administrator,Volunteer")]
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
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
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

                appointments = ValidateEmail(appointments);
                appointments = ValidatePhone(appointments);
                ValidateApplicantEmail(appointments);
                ValidateApplicantName(appointments);
                ValidateApplicantPhoneNumber(appointments);
               

                var currentBooking = db.Appointments
                    .Any(b => (appointments.DateOfAppointment == b.DateOfAppointment
                     && appointments.AdministrationId == b.AdministrationId));

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
                //else if(appointments.ApplicantPhoneNumber == null)
                //{
                //    TempData["Error"] = "Phone Number Required for Applicant";
                //}
                //else if(appointments.ApplicantEmail == null)
                //{
                //    TempData["Error"] = "Email Required for Applicant";
                //}
                //else if (appointments.NameOfApplicant == null)
                //{
                //    TempData["Error"] = "Name Required for Applicant";
                //}
                //else if (appointments.DateOfAppointment == null)
                //{
                //    TempData["Error"] = "Date Required for Appointment";
                //}
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

            if (appointments.DetailsOfAppointment.Equals("Confession"))
            {
                ViewBag.Rooms = new SelectList(new[] { "Sacristy", "Confession Room" });
            }

            else if (appointments.DetailsOfAppointment.Equals("Baptism Meeting") || appointments.DetailsOfAppointment.Equals("Wedding Meeting") || appointments.DetailsOfAppointment.Equals("Personal Meeting"))
            {
                ViewBag.Rooms = new SelectList(new[] { "GP Room", "Parish Office" });
            }
            ViewBag.Details = new SelectList(new[] { "Confession", "Baptism Meeting", "Wedding Meeting", "Personal Meeting" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", appointments.ChurchId);
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", appointments.AdministrationId);
            

            return View(appointments);
        }


        // GET: Appointments/Create
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult CreateCeremony()
        {

            //ViewBag.Rooms = new SelectList(new[] { "Sacristy", "Confession Room", "GP Room", "Parish Office" });

            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name");

            //ViewBag.Volunteer = new SelectList(db.Volunteers, "VolunteerId", "Name");

            ViewBag.Details = new SelectList(new[] {"Baptism", "Wedding", "Communion", "Confirmation"});

            ViewBag.AdminId = new SelectList(db.Admins.Where(o => o.Position == "Priest"), "AdministrationId", "AdministratorName");

            return View();
        }


        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCeremony([Bind(Include = "AppointmentId,DetailsOfAppointment,DateOfAppointment,Confirmed,Fee,ThemeColour,AdministrationId,ChurchId, Slots")] Appointments appointments)
        {
            if (ModelState.IsValid)
            {
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
            ViewBag.Details = new SelectList(new[] { "Baptism Meeting", "Wedding Meeting", "Personal Meeting" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", appointments.ChurchId);
            //ViewBag.VolunteerId = new SelectList(db.Volunteers, "VolunteerId", "Name", appointments.VolunteerId);
            ViewBag.AdminId = new SelectList(db.Admins.Where(o => o.Position == "Priest"), "AdministrationId", "AdministratorName", appointments.AdministrationId);
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
            ViewBag.AdminId = new SelectList(db.Admins.Where(o => o.Position == "Priest"), "AdministrationId", "AdministratorName", appointments.AdministrationId);
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
                appointments = ValidateEmail(appointments);
                appointments = ValidatePhone(appointments);
                ValidateApplicantEmail(appointments);
                ValidateApplicantName(appointments);
                ValidateApplicantPhoneNumber(appointments);

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
            ViewBag.AdminId = new SelectList(db.Admins.Where(o => o.Position == "Priest"), "AdministrationId", "AdministratorName", appointments.AdministrationId);
            return View(appointments);
        }


        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        // GET: Appointments/Edit/5
        public ActionResult EditCeremony(int? id)
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
        public ActionResult EditCeremony([Bind(Include = "AppointmentId,DetailsOfAppointment,RoomType,DateOfAppointment,Confirmed,NameOfApplicant, ApplicantPhoneNumber, ApplicantEmail,AdministrationId,ChurchId, Slots")] Appointments appointments)
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

        public Appointments ValidatePhone(Appointments up)
        {
            if (up.ApplicantPhoneNumber == null)
            {
                TempData["ErrorPhone"] = "A valid phone number must be entered";
            }
            else
            {
                try
                {

                    if (!IsPhoneNumber(up.ApplicantPhoneNumber))
                    {
                        TempData["ErrorPhone"] = "A valid phone number must be entered";
                    }
                }
                catch (Exception e)
                {
                    TempData["ErrorEmail"] = "A valid phone number must be entered";
                }

            }
            return up;
        }

        public void ValidateApplicantName(Appointments apps)
        {
            if(apps.NameOfApplicant == null)
            {
                TempData["ErrorName"] = "The Applicant Name must be entered";
            }

        }

        public void ValidateApplicantEmail(Appointments apps)
        {
            if (apps.ApplicantEmail == null)
            {
                TempData["ErrorEmail"] = "The Applicant Email must be entered";
            }

        }

        public void ValidateApplicantPhoneNumber(Appointments apps)
        {
            if (apps.NameOfApplicant == null)
            {
                TempData["ErrorPhone"] = "The Applicant Phone Number must be entered";
            }
            else if(!(IsPhoneNumber(apps.ApplicantPhoneNumber)))
            {
                TempData["ErrorPhone"] = "The Applicant Phone Number must be a number";
            }
        }


        public static bool IsPhoneNumber(string number)
        {
            if (number == null)
            {
                Console.WriteLine("Hi");
                return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
            }
            else
            {
                return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
            }

        }


    }
}