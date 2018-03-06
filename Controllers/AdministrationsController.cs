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

namespace PIMS.Controllers
{
    public class AdministrationsController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();

        // GET: Administrations
        public ActionResult Index()
        {
            var admins = db.Admins.Include(a => a.AdminChurch);
            return View(admins.ToList());
        }

        // GET: Administrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administration administration = db.Admins.Find(id);
            if (administration == null)
            {
                return HttpNotFound();
            }
            return View(administration);
        }

        // GET: Administrations/Create
        public ActionResult Create()
        {
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name");
            return View();
        }

        // POST: Administrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdministrationId,AdministratorName,AdminUsername,Password,Position,PhoneNumber,EmailAddress,ChurchId")] Administration administration)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(administration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", administration.ChurchId);
            return View(administration);
        }

        // GET: Administrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administration administration = db.Admins.Find(id);
            if (administration == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", administration.ChurchId);
            return View(administration);
        }

        // POST: Administrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdministrationId,AdministratorName,AdminUsername,Password,Position,PhoneNumber,EmailAddress,ChurchId")] Administration administration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(administration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", administration.ChurchId);
            return View(administration);
        }

        // GET: Administrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administration administration = db.Admins.Find(id);
            if (administration == null)
            {
                return HttpNotFound();
            }
            return View(administration);
        }

        // POST: Administrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Administration administration = db.Admins.Find(id);
            db.Admins.Remove(administration);
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

        public Administration ValidateEmail(Administration up)
        {
            if (up.EmailAddress == null)
            {
                TempData["ErrorEmail"] = "A valid email must be entered";
            }
            else
            {
                try
                {
                    string domain = up.EmailAddress.ToString().ToLower().Split('@').ElementAt(1);
                    if (domain != "gmail.come" || domain != "hotmail.com")
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
