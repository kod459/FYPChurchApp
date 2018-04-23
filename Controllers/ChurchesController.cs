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
    public class ChurchesController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();

        // GET: Churches
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Index()
        {
            return View(db.Churches.ToList());
            
        }

        // GET: Churches/Details/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Church church = db.Churches.Find(id);
            if (church == null)
            {
                return HttpNotFound();
            }
            return View(church);
        }

        // GET: Churches/Create
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Churches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Create([Bind(Include = "ChurchId,Address,Name,PhoneNumber,Diocese")] Church church)
        {
            if (ModelState.IsValid)
            {
                db.Churches.Add(church);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(church);
        }

        // GET: Churches/Edit/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Church church = db.Churches.Find(id);
            if (church == null)
            {
                return HttpNotFound();
            }
            return View(church);
        }

        // POST: Churches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChurchId,Address,Name,PhoneNumber,Diocese")] Church church)
        {
            if (ModelState.IsValid)
            {
                db.Entry(church).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(church);
        }

        // GET: Churches/Delete/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Church church = db.Churches.Find(id);
            if (church == null)
            {
                return HttpNotFound();
            }
            return View(church);
        }

        // POST: Churches/Delete/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Church church = db.Churches.Find(id);
            db.Churches.Remove(church);
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
