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
using System.Web.Security;

namespace PIMS.Controllers
{
    public class BulletinsController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();

        // GET: Bulletins
        public ActionResult Index()
        {
            var bulletins = db.Bulletins.OrderByDescending(a => a.DateOfBulletin).ToList();
            return View(bulletins);
        }

        // GET: Bulletins/Details/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bulletins bulletins = db.Bulletins.Find(id);
            if (bulletins == null)
            {
                return HttpNotFound();
            }
            return View(bulletins);
        }

        // GET: Bulletins/Create
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Create()
        {
            string username = Membership.GetUser().UserName;

            var getAdmin = (from a in db.Admins
                            where username == a.AdminUsername
                            select a.AdministratorName).SingleOrDefault();

            ViewBag.AdminName = getAdmin;
            Bulletins model = new Bulletins();
            model.DateOfBulletin = DateTime.Now;

            return View(model);

            //ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName");
            
        }

        // POST: Bulletins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BulletinsID,DetailsOfBulletin,AdminPosting, DateOfBulletin")] Bulletins bulletins)
        {
            if (ModelState.IsValid)
            {
                string username = Membership.GetUser().UserName;


                var getAdmin = (from a in db.Admins
                                where username == a.AdminUsername
                                select a.AdministratorName).SingleOrDefault();

                ViewBag.AdminName = getAdmin;

                db.Bulletins.Add(bulletins);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", bulletins.AdministrationId);
            return View(bulletins);
        }

        // GET: Bulletins/Edit/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bulletins bulletins = db.Bulletins.Find(id);
            if (bulletins == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", bulletins.AdministrationId);
            return View(bulletins);
        }

        // POST: Bulletins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Edit([Bind(Include = "BulletinsID,DetailsOfBulletin,AdminPosting,DateOfBulletin")] Bulletins bulletins)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bulletins).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", bulletins.AdministrationId);
            return View(bulletins);
        }

        // GET: Bulletins/Delete/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bulletins bulletins = db.Bulletins.Find(id);
            if (bulletins == null)
            {
                return HttpNotFound();
            }
            return View(bulletins);
        }

        // POST: Bulletins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Bulletins bulletins = db.Bulletins.Find(id);
            db.Bulletins.Remove(bulletins);
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
