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
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Drive.v3.Data;
using System.Web.Security;

namespace PIMS.Controllers
{
    public class RecordsController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();

        // GET: Records
        public ActionResult Index()
        {
            var records = db.Records.ToList();
            return View(records);
        }

        // GET: Records/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // GET: Records/Create
        public ActionResult Create()
        {
            string username = Membership.GetUser().UserName;

            var getAdmin = (from a in db.Admins
                            where username == a.AdminUsername
                            select a.AdministratorName).SingleOrDefault();

            ViewBag.AdminName = getAdmin;

            ViewBag.DocumentType = new SelectList(new[] { "Wedding Cert", "Confirmation Cert", "Baptism Cert", "Funeral Cert" });

            Record model = new Record();
            model.UploadDate = DateTime.Now;

            return View(model);
        }

        // POST: Records/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecordId,NameOnRecord,DocumentType,UploadDate,Document,UploadedBy")] Record record, HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {
                string username = Membership.GetUser().UserName;


                var getAdmin = (from a in db.Admins
                                where username == a.AdminUsername
                                select a.AdministratorName).SingleOrDefault();

                ViewBag.AdminName = getAdmin;
                ViewBag.DocumentType = new SelectList(new[] { "Wedding Cert", "Confirmation Cert", "Baptism Cert", "Funeral Cert" });

                if (File != null && File.ContentLength > 0)
                {
                    record.Document = new byte[File.ContentLength];
                    File.InputStream.Read(record.Document, 0, File.ContentLength);
                }

                db.Records.Add(record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           
            return View(record);
        }

        // GET: Records/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            ViewBag.Type = new SelectList(new[] { "Wedding Cert", "Confirmation Cert", "Baptism Cert", "Funeral Cert" });
            // ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", record.AdministrationId);
            return View(record);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecordId,NameOnRecord,DocumentType,UploadDate,Document,UploadedBy")] Record record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Type = new SelectList(new[] { "Wedding Cert", "Confirmation Cert", "Baptism Cert", "Funeral Cert" });
            // ViewBag.AdministrationId = new SelectList(db.Admins, "AdministrationId", "AdministratorName", record.AdministrationId);
            return View(record);
        }

        // GET: Records/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Records.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Record record = db.Records.Find(id);
            db.Records.Remove(record);
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
