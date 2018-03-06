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

namespace PIMS.Controllers
{
    public class CeremoniesController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();

        // GET: Ceremonies
        public ActionResult Index()
        {
            var ceremonies = db.Ceremonies.Include(c => c.Church);
            return View(ceremonies.ToList());
        }

        public ActionResult Calendar()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            ChurchDBContext db = new ChurchDBContext();
            var events = db.Ceremonies.ToList();
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        // GET: Ceremonies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ceremony ceremony = db.Ceremonies.Find(id);
            if (ceremony == null)
            {
                return HttpNotFound();
            }
            return View(ceremony);
        }

        // GET: Ceremonies/Create
        public ActionResult Create()
        {
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name");
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName");
            ViewBag.VolunteerId = new SelectList(db.Volunteers, "VolunteerId", "Name");
            ViewBag.Volunteer1 = new SelectList(db.Volunteers, "VolunteerId", "Name");
            return View();
        }

        // POST: Ceremonies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CeremonyId,TypeOfCeremony,CeremonyFee,DateOfCeremony,TimeOfCeremony,ChurchId,VolunteerId,AdministrationId")] Ceremony ceremony)
        {
            if (ModelState.IsValid)
            {

                //Checking if Ceremony already booked that day
                var currentCeremony = db.Ceremonies
                .Any(b => (ceremony.DateOfCeremony == b.DateOfCeremony
                && ceremony.AdministrationId == b.AdministrationId));

                var appointment = db.Appointments
                    .Any(b => (ceremony.DateOfCeremony == b.DateOfAppointment
                    && ceremony.AdministrationId == b.AdministrationId));

                if(appointment)
                {
                    TempData["Error"] = "Parish Admin not available due to appointment on this date!";
                    if(currentCeremony)
                    {
                        TempData["Error"] = "Parish Admin not available due to ceremony on this date!";
                    }
                }
                else
                {
                    db.Ceremonies.Add(ceremony);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", ceremony.ChurchId);
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", ceremony.AdministrationId);
            ViewBag.VolunteerId = new SelectList(db.Volunteers, "VolunteerId", "Name", ceremony.VolunteerId);
            ViewBag.Volunteer1 = new SelectList(db.Volunteers, "VolunteerId", "Name", ceremony.VolunteerId);
            return View(ceremony);
        }

        // GET: Ceremonies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ceremony ceremony = db.Ceremonies.Find(id);
            if (ceremony == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", ceremony.ChurchId);
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", ceremony.AdministrationId);
            ViewBag.VolunteerId = new SelectList(db.Volunteers, "VolunteerId", "Name", ceremony.VolunteerId);
            ViewBag.Volunteer1 = new SelectList(db.Volunteers, "VolunteerId", "Name", ceremony.VolunteerId);
            return View(ceremony);
        }

        // POST: Ceremonies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CeremonyId,TypeOfCeremony,CeremonyFee,DateOfCeremony,TimeOfCeremony,ChurchId,VolunteerId,AdministrationId")] Ceremony ceremony)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ceremony).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", ceremony.ChurchId);
            ViewBag.AdminId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", ceremony.AdministrationId);
            ViewBag.VolunteerId = new SelectList(db.Volunteers, "VolunteerId", "Name", ceremony.VolunteerId);
            ViewBag.Volunteer1 = new SelectList(db.Volunteers, "VolunteerId", "Name", ceremony.VolunteerId);
            return View(ceremony);
        }

        // GET: Ceremonies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ceremony ceremony = db.Ceremonies.Find(id);
            if (ceremony == null)
            {
                return HttpNotFound();
            }
            return View(ceremony);
        }

        // POST: Ceremonies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ceremony ceremony = db.Ceremonies.Find(id);
            db.Ceremonies.Remove(ceremony);
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
    }
}
