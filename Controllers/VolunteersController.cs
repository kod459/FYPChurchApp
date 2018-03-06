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
using PIMS.Controllers;
using PIMS.Models;
using PIMS.ViewModel;
using System.Data.Entity.Infrastructure;
using WebMatrix.WebData;
using PIMS.Mailers;

namespace PIMS.Controllers
{
    public class VolunteersController : Controller
    {
        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }
        private ChurchDBContext db = new ChurchDBContext();

        // GET: Volunteers
        public ActionResult Index()
        {
            var volunteers = db.Volunteers.Include(v => v.Church);
            var test = this.User;
            return View(volunteers.ToList());
            
        }

        public ActionResult VolunteerHub()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            var events = (from a in db.Appointments
                          where a.Fee != null
                          select a).ToList();

            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: Volunteers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // GET: Volunteers/Create
        public ActionResult Create(string email, string name, string phone, string username)
        {
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name");
            ViewBag.Role = new SelectList(new[] { "Cleaner", "Baptism Team", "Money Counting", "Collector", "Parish Team", });
            ViewBag.email = email;
            ViewBag.name = name;
            ViewBag.phone = phone;
            ViewBag.username = username;
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VolunteerId,Name,Email,GardaVetted,VolunteerRole,VolunteerPhoneNumber,ChurchId")] Volunteer volunteer, string email, string name, string phone, string username)
        {
            if (ModelState.IsValid)
            {
                ViewBag.email = email;
                ViewBag.name = name;
                ViewBag.phone = phone;
                ViewBag.username = username;

                ViewBag.Role = new SelectList(new[] { "Cleaner", "Baptism Team", "Money Counting", "Collector", "Parish Team", });

                db.Volunteers.Add(volunteer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", volunteer.ChurchId);
            return View(volunteer);
        }

        // GET: Volunteers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", volunteer.ChurchId);
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VolunteerId,Name,UserName,Email,GardaVetted,VolunteerRole,VolunteerPhoneNumber,ChurchId")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volunteer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", volunteer.ChurchId);
            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            db.Volunteers.Remove(volunteer);
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



        private void PopulateAssignedCeremonyData(Volunteer volunteer)
        {
            var allCeremonies = (from a in db.Appointments
                                 where a.Fee != null
                                 && a.DateOfAppointment > DateTime.Now
                                 select a);


            var ceremonyVolunteer = new HashSet<int>(volunteer.Appointments.Select(b => b.AppointmentId));
            var viewModelAvailable = new List<VolunteerCeremonyVM>();
            var viewModelSelected = new List<VolunteerCeremonyVM>();
            foreach (var item in allCeremonies)
            {

                if (ceremonyVolunteer.Contains(item.AppointmentId))
                {
                    viewModelSelected.Add(new VolunteerCeremonyVM
                    {
                        AppointmentId = item.AppointmentId,
                        DetailsOfAppointment = item.DetailsOfAppointment + " @ " + item.DateOfAppointment.ToShortDateString() + " in " + item.Church.Name,
                        //Assigned = true
                    });
                }
                else
                {
                    viewModelAvailable.Add(new VolunteerCeremonyVM
                    {
                        AppointmentId = item.AppointmentId,
                        //DateOfAppointment = item.DateOfAppointment,
                        DetailsOfAppointment = item.DetailsOfAppointment + " @ " + item.DateOfAppointment.ToShortDateString() + " in " + item.Church.Name,
                        //Assigned = false
                    });
                }
            }

            ViewBag.AvailCeremonies = new MultiSelectList(viewModelAvailable, "AppointmentId", "DetailsOfAppointment");
            ViewBag.SelectedCeremonies = new MultiSelectList(viewModelSelected, "AppointmentId", "DetailsOfAppointment");
        }


        // GET: 
        public ActionResult VolunteerCeremony(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            string userName = string.Empty;

            var getVolunteerId = (from u in db.Volunteers
                                  where WebSecurity.CurrentUserName == u.Username
                                  select u.VolunteerId).SingleOrDefault();


            //Volunteer v = (Volunteer)(from k in db.Volunteers
            //               where getVolunteerId == k.VolunteerId
            //               select k).SingleOrDefault();

            
            Volunteer v = db.Volunteers
                .Include(p => p.Appointments)
                .Where(i => i.VolunteerId == id)
                .Single();


            if (v == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedCeremonyData(v);


            //var app = (from a in db.Appointments
            //           where v.app.AppointmentId == a.AppointmentId
            //           select a).FirstOrDefault();

            //string churchName = (from c in db.Churches
            //                     where c.ChurchId == v.Church.ChurchId
            //                     select c.Name).FirstOrDefault();

            //string volunteerName = (from a in db.Volunteers
            //                    where a.VolunteerId == v.VolunteerId
            //                    select a.Name).FirstOrDefault();

            //string volunteerEmail = (from a in db.Volunteers
            //                     where a.VolunteerId == v.VolunteerId
            //                     select a.Email).FirstOrDefault();





            //UserMailer.VolunteerCeremony(app, churchName, volunteerEmail).Send(); //Send() extension method: using Mvc.Mailer

            return View(v);
        }


        // POST: /Player/VolunteerCeremony/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VolunteerCeremony(int? id, string[] selectedOptions)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var getVolunteerId = (from u in db.Volunteers
                                  where WebSecurity.CurrentUserName == u.Username
                                  select u.VolunteerId).SingleOrDefault();


            //var v = (Volunteer)(from k in db.Volunteers
            //                          where getVolunteerId == k.VolunteerId
            //                          select k).SingleOrDefault();

            Volunteer v = db.Volunteers
                .Include(p => p.Appointments)
                .Where(i => i.VolunteerId == id)
                .Single();

            try
            {
                UpdateVolunteerCeremonies(selectedOptions, v);

                db.Entry(v).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            

            PopulateAssignedCeremonyData(v);
            return View(v);
        }


        private void UpdateVolunteerCeremonies(string[] selectedOptions, Volunteer updateVolunteerWithCeremony)
        {
            if (selectedOptions == null)
            {
                updateVolunteerWithCeremony.Appointments = new List<Appointments>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var playerBats = new HashSet<int>
                (updateVolunteerWithCeremony.Appointments.Select(b => b.AppointmentId));
            foreach (var item in db.Appointments)
            {
                if (selectedOptionsHS.Contains(item.AppointmentId.ToString()))
                {
                    if (!playerBats.Contains(item.AppointmentId))
                    {
                        updateVolunteerWithCeremony.Appointments.Add(item);
                    }
                }
                else
                {
                    if (playerBats.Contains(item.AppointmentId))
                    {
                        updateVolunteerWithCeremony.Appointments.Remove(item);
                    }
                }
            }
        }

    }
}
