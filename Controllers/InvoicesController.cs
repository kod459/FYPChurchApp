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
using PIMS.ViewModel;
using System.IO;

namespace PIMS.Controllers
{
    public class InvoicesController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();


        // GET: Invoices
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Index()
        {
            var invoices = db.Invoices.Include(i => i.Church);
            return View(invoices.ToList());
        }

        // GET: Invoices/Details/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Create()
        {
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Create([Bind(Include = "InvoiceId,Company,Description,Amount,PictureOfInvoice,DateReceived,ChurchId")] Invoice invoice,HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {
                if (File != null && File.ContentLength > 0)
                {
                    using (MemoryStream ms1 = new MemoryStream())
                    {
                        invoice.PictureOfInvoice = new byte[File.ContentLength];
                        File.InputStream.Read(invoice.PictureOfInvoice, 0, File.ContentLength);
                    }
                }
                else
                {
                    TempData["Error"] = "Upload an Image";
                }


                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", invoice.ChurchId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice oldInvoice = db.Invoices.Find(id);
            if (oldInvoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", oldInvoice.ChurchId);
            return View(oldInvoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceId,Company,Description,Amount,PictureOfInvoice,DateReceived,ChurchId")] Invoice invoice, int? id)
        {

            if (ModelState.IsValid)
            {
                Invoice oldInvoice = db.Invoices.Find(id);
                oldInvoice.Amount = invoice.Amount;
                oldInvoice.Company = invoice.Company;
                oldInvoice.Description = invoice.Description;
                oldInvoice.Amount = invoice.Amount;
                oldInvoice.ChurchId = invoice.ChurchId;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", invoice.ChurchId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
