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
using System.Web.Security;
using System.Text.RegularExpressions;

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

            bool isEmpty = !volunteers.Any();

            if (isEmpty)
            {
                TempData["Error"] = "No volunteers available";
                return View();
            }
            else
            {
                return View(volunteers.ToList());
            }

        }

        public ActionResult VolunteerHub()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            string username = Membership.GetUser().UserName;

            var getVolunteer = (from a in db.Volunteers
                                where username == a.Username
                                select a.VolunteerId).SingleOrDefault();

            var events = (from a in db.Appointments
                          where a.Fee != null
                          select a).ToList();

            //var volunteersInCeremony = db.Appointments
            //          .Where(c => c.CategoryId == categoryId)
            //          .SelectMany(c => c.Products);

            var vEvents = from a in db.Appointments
                          where a.Volunteers.Any(c => c.VolunteerId == getVolunteer) && a.Fee != null
                          select a;



            //var events = (from a in db.Appointments
            //              where a.Volunteers.Contains(getVolunteer) && a.Fee != null
            //              select a).ToList();

            //var events =
            //    db.Appointments.Where(a =>
            //    a.Volunteers.All(b =>
            //    getVolunteer.Equals(b.VolunteerId)));

            return new JsonResult { Data = vEvents, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddVolunteerToCeremony(Volunteer v, Appointments apps)
        {
            var status = false;
            string username = Membership.GetUser().UserName;

            var getVolunteer = (from vol in db.Volunteers
                                where username == vol.Username
                                select vol).SingleOrDefault();

            apps.Volunteers = new List<Volunteer>();

            apps.Volunteers.Add(getVolunteer);
            db.SaveChanges();
            status = true;

            return new JsonResult { Data = new { status = status } };

        }

        public ActionResult ViewAvailableCeremonies()
        {
            string username = Membership.GetUser().UserName;

            var getVolunteer = (from vol in db.Volunteers
                                where username == vol.Username
                                select vol).SingleOrDefault();


            var ceremonies = (from a in getVolunteer.Appointments
                              where a.Fee != null
                              && a.Slots != 0
                              && !(getVolunteer.Appointments.Any(c => c.AppointmentId == a.AppointmentId))
                              select a);

            bool isEmpty = !ceremonies.Any();

            if(!isEmpty)
            {
                TempData["Error"] = "No ceremonies available";
                return View();
            }
            else
            {
                return View(ceremonies.ToList());
            }

            
        }

        public ActionResult VolunteerCeremonies(Appointments apps)
        {
            string username = Membership.GetUser().UserName;

            var getVolunteer = (from vol in db.Volunteers
                                where username == vol.Username
                                select vol).FirstOrDefault();

            if (getVolunteer == null)
            {
                TempData["Error"] = "No Volunteer";
                return View();
            }
            else
            {


                var ceremonies = (from a in getVolunteer.Appointments
                                  where a.Fee != null && a.Slots != 0
                                  select a);

                bool isEmpty = !ceremonies.Any();


                if (isEmpty)
                {
                    TempData["Error"] = "No ceremonies for Volunteer";
                    return View();
                }
                else
                {
                    return View(ceremonies.ToList());
                }
            }

        }

        public ActionResult ViewSpecificDetails()
        {
            string username = Membership.GetUser().UserName;

            var getVolunteerId = (from u in db.Volunteers
                                  where username == u.Username
                                  select u).ToList();

            return View(getVolunteerId);
        }


        //GET Ceremony thats been clicked
        public ActionResult JoinCeremony(int? id)
        {
            string username = Membership.GetUser().UserName;

            var getVolunteerName = (from u in db.Volunteers
                                    where username == u.Username
                                    select u.Name);

            var getVolunteerRole = (from u in db.Volunteers
                                    where username == u.Username
                                    select u.VolunteerRole);

            var getVolunteerID = (from u in db.Volunteers
                                    where username == u.Username
                                    select u.VolunteerId).FirstOrDefault();

            ViewBag.Name = getVolunteerName;
            ViewBag.Role = getVolunteerRole;
            ViewBag.VolunteerID = getVolunteerID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments ceremonies = db.Appointments.Find(id);
            if (ceremonies == null)
            {
                return HttpNotFound();
            }

            ViewBag.Details = new SelectList(new[] { "Confession", "Baptism", "Wedding", "Communion", "Confirmation" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", ceremonies.ChurchId);
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", ceremonies.AdministrationId);
            return View(ceremonies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinCeremony(Appointments apps, int? id)
        {
            string username = Membership.GetUser().UserName;

            var getVolunteerName = (from u in db.Volunteers
                                    where username == u.Username
                                    select u.Name);

            var getVolunteerRole = (from u in db.Volunteers
                                    where username == u.Username
                                    select u.VolunteerRole);

            var getVolunteerID = (from u in db.Volunteers
                                  where username == u.Username
                                  select u.VolunteerId);

            var getVolunteer = (from u in db.Volunteers
                                where username == u.Username
                                select u).SingleOrDefault();

            ViewBag.Name = getVolunteerName;
            ViewBag.Role = getVolunteerRole;
            ViewBag.VolunteerID = getVolunteerID;

            ViewBag.Details = new SelectList(new[] { "Confession", "Baptism", "Wedding", "Communion", "Confirmation" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", apps.ChurchId);
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", apps.AdministrationId);




            if (ModelState.IsValid)
            {
                apps.Slots = apps.Slots - 1;
                db.Entry(apps).State = EntityState.Modified;

                apps.Volunteers.Add(getVolunteer);
                db.SaveChanges();
                return RedirectToAction("VolunteerCeremonies");
            }

            return View(apps);
        }


        // GET: Volunteers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string username = Membership.GetUser().UserName;

            Volunteer getVolunteerId = (from u in db.Volunteers
                                  where username == u.Username
                                  select u).SingleOrDefault();

            Volunteer volunteer = db.Volunteers.Find(id);
            if (getVolunteerId == null)
            {
                return HttpNotFound();
            }
            return View(getVolunteerId);
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
        public ActionResult Create([Bind(Include = "VolunteerId,Name,Email,GardaVetted,VolunteerRole,Username,VolunteerPhoneNumber,ChurchId")] Volunteer volunteer, string email, string name, string phone, string username)
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
            ViewBag.Role = new SelectList(new[] { "Cleaner", "Baptism Team", "Money Counting", "Collector", "Parish Team", });
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
            ViewBag.Role = new SelectList(new[] { "Cleaner", "Baptism Team", "Money Counting", "Collector", "Parish Team", });
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


            string username = Membership.GetUser().UserName;

            var getVolunteerId = (from u in db.Volunteers
                                  where username == u.Username
                                  select u.VolunteerId).SingleOrDefault();

            if(getVolunteerId == id)
            {
                id = getVolunteerId;
            }


            Volunteer v = db.Volunteers.Find(id);


            if (v == null)
            {
                return HttpNotFound();
            }
            PopulateAssignedCeremonyData(v);
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

            string username = Membership.GetUser().UserName;

            var getVolunteerId = (from u in db.Volunteers
                                  where username == u.Username
                                  select u.VolunteerId).SingleOrDefault();

            if (getVolunteerId == id)
            {
                id = getVolunteerId;
            }


            Volunteer v = (Volunteer)(from u in db.Volunteers
                                      where id == u.VolunteerId
                                      select u).Single();

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

        public Volunteer ValidatePhone(Volunteer up)
        {
            if (up.VolunteerPhoneNumber == null)
            {
                TempData["ErrorPhone"] = "A valid phone number must be entered";
            }
            else
            {
                try
                {

                    if (!IsPhoneNumber(up.VolunteerPhoneNumber))
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

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }

    }
}