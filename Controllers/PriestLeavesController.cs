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
    public class PriestLeavesController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();

        // GET: PriestLeaves
        public ActionResult Index()
        {
            var priestLeave = db.PriestLeave.Include(p => p.Admins);
            return View(priestLeave.ToList());
        }

        // GET: PriestLeaves/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriestLeave priestLeave = db.PriestLeave.Find(id);
            if (priestLeave == null)
            {
                return HttpNotFound();
            }
            return View(priestLeave);
        }

        // GET: PriestLeaves/Create
        public ActionResult Create()
        {
            ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName");
            return View();
        }

        // POST: PriestLeaves/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DetailsOfLeave,StartDate,EndDate,AdministrationId")] PriestLeave priestLeave)
        {
            if (ModelState.IsValid)
            {
                db.PriestLeave.Add(priestLeave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", priestLeave.AdministrationId);
            return View(priestLeave);
        }

        // GET: PriestLeaves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriestLeave priestLeave = db.PriestLeave.Find(id);
            if (priestLeave == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", priestLeave.AdministrationId);
            return View(priestLeave);
        }

        // POST: PriestLeaves/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DetailsOfLeave,StartDate,EndDate,AdministrationId")] PriestLeave priestLeave)
        {
            if (ModelState.IsValid)
            {
                db.Entry(priestLeave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", priestLeave.AdministrationId);
            return View(priestLeave);
        }

        // GET: PriestLeaves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriestLeave priestLeave = db.PriestLeave.Find(id);
            if (priestLeave == null)
            {
                return HttpNotFound();
            }
            return View(priestLeave);
        }

        // POST: PriestLeaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PriestLeave priestLeave = db.PriestLeave.Find(id);
            db.PriestLeave.Remove(priestLeave);
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
