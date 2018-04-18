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
    public class BulletinsController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();

        // GET: Bulletins
        public ActionResult Index()
        {
            var bulletins = db.Bulletins.Include(b => b.Admins);
            return View(bulletins.ToList());
        }

        // GET: Bulletins/Details/5
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
        public ActionResult Create()
        {
            ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName");
            return View();
        }

        // POST: Bulletins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BulletinsID,DetailsOfBulletin,AdministrationId")] Bulletins bulletins)
        {
            if (ModelState.IsValid)
            {
                db.Bulletins.Add(bulletins);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", bulletins.AdministrationId);
            return View(bulletins);
        }

        // GET: Bulletins/Edit/5
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
            ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", bulletins.AdministrationId);
            return View(bulletins);
        }

        // POST: Bulletins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BulletinsID,DetailsOfBulletin,AdministrationId")] Bulletins bulletins)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bulletins).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", bulletins.AdministrationId);
            return View(bulletins);
        }

        // GET: Bulletins/Delete/5
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
