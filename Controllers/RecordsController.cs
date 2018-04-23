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

        public ActionResult IndexOfBaptisms(string SearchStringName, string SearchStringYearUploaded)
        {
            IQueryable<Record> allBaptismRecords = (from r in db.Records
                                                    where (r.DocumentType.Equals("Baptism Cert"))
                                                    select r);
            if(!String.IsNullOrEmpty(SearchStringName) && String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                                        where (r.DocumentType.Equals("Baptism Cert") && r.NameOnRecord.Contains(SearchStringName))
                                                        select r);

                if(records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }

            else if(!String.IsNullOrEmpty(SearchStringName) && !String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Baptism Cert") 
                                              && r.NameOnRecord.Contains(SearchStringName)
                                              && r.UploadDate.Year.ToString().Equals(SearchStringYearUploaded))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }

            else if (String.IsNullOrEmpty(SearchStringName) && !String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Baptism Cert")
                                              && r.UploadDate.Year.ToString().Equals(SearchStringYearUploaded))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }

            else
            {
                return View(allBaptismRecords.ToList());
            }
        }

        public ActionResult IndexOfConfirmations(string SearchStringName, string SearchStringYearUploaded)
        {
            if (!String.IsNullOrEmpty(SearchStringName) && String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Confirmation Cert") && r.NameOnRecord.Contains(SearchStringName))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }
            else if (!String.IsNullOrEmpty(SearchStringName) && !String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Confirmation Cert")
                                              && r.NameOnRecord.Contains(SearchStringName)
                                              && r.UploadDate.Year.ToString().Equals(SearchStringYearUploaded))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }
            else if (String.IsNullOrEmpty(SearchStringName) && !String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Confirmation Cert")
                                              && r.UploadDate.Year.ToString().Equals(SearchStringYearUploaded))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }
            else
            {
                IQueryable<Record> allConfirmations = (from r in db.Records
                                                       where (r.DocumentType.Equals("Confirmation Cert"))
                                                       select r);
                return View(allConfirmations);
            }
        }

        public ActionResult IndexOfFunerals(string SearchStringName, string SearchStringYearUploaded)
        {
            if (!String.IsNullOrEmpty(SearchStringName) && String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Confirmation Cert") && r.NameOnRecord.Contains(SearchStringName))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }
            else if (!String.IsNullOrEmpty(SearchStringName) && !String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Funeral Cert")
                                              && r.NameOnRecord.Contains(SearchStringName)
                                              && r.UploadDate.Year.ToString().Equals(SearchStringYearUploaded))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }
            else if (String.IsNullOrEmpty(SearchStringName) && !String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Funeral Cert")
                                              && r.UploadDate.Year.ToString().Equals(SearchStringYearUploaded))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }
            else
            {
                IQueryable<Record> allFunerals = (from r in db.Records
                                                       where (r.DocumentType.Equals("Funeral Cert"))
                                                       select r);
                return View(allFunerals);
            }
        }

        public ActionResult IndexOfWeddings(string SearchStringName, string SearchStringYearUploaded)
        {
            if (!String.IsNullOrEmpty(SearchStringName) && String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Wedding Cert") && r.NameOnRecord.Contains(SearchStringName))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }
            else if (!String.IsNullOrEmpty(SearchStringName) && !String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Wedding Cert")
                                              && r.NameOnRecord.Contains(SearchStringName)
                                              && r.UploadDate.Year.ToString().Equals(SearchStringYearUploaded))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }
            else if (String.IsNullOrEmpty(SearchStringName) && !String.IsNullOrEmpty(SearchStringYearUploaded))
            {
                IQueryable<Record> records = (from r in db.Records
                                              where (r.DocumentType.Equals("Wedding Cert")
                                              && r.UploadDate.Year.ToString().Equals(SearchStringYearUploaded))
                                              select r);

                if (records.Count() == 0)
                {
                    TempData["Error"] = "No Records found";
                    return View();
                }

                return View(records.ToList());
            }
            else
            {
                IQueryable<Record> allConfirmations = (from r in db.Records
                                                       where (r.DocumentType.Equals("Wedding Cert"))
                                                       select r);
                return View(allConfirmations);
            }
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

            if (record.DocumentType.Equals("Wedding Cert"))
            {
                return RedirectToAction("IndexOfWeddings");
            }
            else if (record.DocumentType.Equals("Baptism Cert"))
            {
                return RedirectToAction("IndexOfBaptisms");
            }
            else if (record.DocumentType.Equals("Funeral Cert"))
            {
                return RedirectToAction("IndexOfFunerals");
            }
            else if (record.DocumentType.Equals("Confirmation Cert"))
            {
                return RedirectToAction("IndexOfConfirmations");
            }


            return View(record);
        }


        public FileContentResult ViewRecords(int? id)
        {
            if (id == 0) { return null; }
            Record record = new Record();
            ChurchDBContext db = new ChurchDBContext();
            record = db.Records.Where(a => a.RecordId == id).SingleOrDefault();
            Response.AppendHeader("content-disposition", "inline; filename="+ record.NameOnRecord + record.UploadDate.ToString() +".jpg"); //this will open in a new tab.. remove if you want to open in the same tab.
            return File(record.Document, "application/jpg");
        }


        // GET: Records/Create
        public ActionResult Create()
        {
            string username = Membership.GetUser().UserName;

            var getAdmin = (from a in db.Admins
                            where username == a.AdminUsername
                            select a.AdministratorName).SingleOrDefault();

            ViewBag.AdminName = getAdmin;

            ViewBag.DocumentType = new SelectList(new[] { "Baptism Cert", "Confirmation Cert", "Wedding Cert", "Funeral Cert" });

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
                ViewBag.DocumentType = new SelectList(new[] { "Baptism Cert", "Confirmation Cert", "Wedding Cert", "Funeral Cert" });

                if (File != null && File.ContentLength > 0)
                {
                    record.Document = new byte[File.ContentLength];
                    File.InputStream.Read(record.Document, 0, File.ContentLength);
                }

                db.Records.Add(record);
                db.SaveChanges();

                if(record.DocumentType.Equals("Wedding Cert"))
                {
                    return RedirectToAction("IndexOfWeddings");
                }
                else if(record.DocumentType.Equals("Baptism Cert"))
                {
                    return RedirectToAction("IndexOfBaptisms");
                }
                else if(record.DocumentType.Equals("Funeral Cert"))
                {
                    return RedirectToAction("IndexOfFunerals");
                }
                else if(record.DocumentType.Equals("Confirmation Cert"))
                {
                    return RedirectToAction("IndexOfConfirmations");
                }
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
            ViewRecords(record.RecordId);
            if (record == null)
            {
                return HttpNotFound();
            }
            ViewBag.Type = new SelectList(new[] { "Wedding Cert", "Confirmation Cert", "Baptism Cert", "Funeral Cert" });
            return View(record);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecordId,NameOnRecord,DocumentType,UploadDate,Document,UploadedBy")] Record record, int? id)
        {
            if (ModelState.IsValid)
            {
                ViewRecords(record.RecordId);
                Record oldRecord = db.Records.Find(id);
                oldRecord.NameOnRecord = record.NameOnRecord;
                oldRecord.DocumentType = record.DocumentType;
                oldRecord.UploadDate = record.UploadDate;
                oldRecord.UploadedBy = record.UploadedBy;


                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
                ViewRecords(record.RecordId);
                if (record.DocumentType.Equals("Wedding Cert"))
                {
                    return RedirectToAction("IndexOfWeddings");
                }
                else if (record.DocumentType.Equals("Baptism Cert"))
                {
                    return RedirectToAction("IndexOfBaptisms");
                }
                else if (record.DocumentType.Equals("Funeral Cert"))
                {
                    return RedirectToAction("IndexOfFunerals");
                }
                else if (record.DocumentType.Equals("Confirmation Cert"))
                {
                    return RedirectToAction("IndexOfConfirmations");
                }
            }

            ViewBag.Type = new SelectList(new[] { "Baptism Cert", "Confirmation Cert", "Wedding Cert", "Funeral Cert" });
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

        public ActionResult DownloadCert(int? id)
        {
            Record record = db.Records.Find(id);

            ViewRecords(record.RecordId);

            if (record.DocumentType.Equals("Wedding Cert"))
            {
                return RedirectToAction("IndexOfWeddings");
            }
            else if (record.DocumentType.Equals("Baptism Cert"))
            {
                return RedirectToAction("IndexOfBaptisms");
            }
            else if (record.DocumentType.Equals("Funeral Cert"))
            {
                return RedirectToAction("IndexOfFunerals");
            }
            else if (record.DocumentType.Equals("Confirmation Cert"))
            {
                return RedirectToAction("IndexOfConfirmations");
            }

            return View();


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
