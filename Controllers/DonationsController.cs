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
    public class DonationsController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();

        // GET: Donations
        public ActionResult Index()
        {
            var donations = db.Donations.Include(d => d.Church);
            return View(donations.ToList());
        }

        public ActionResult SearchHouseNumber(string SearchStringAddress)
        {
            if (!String.IsNullOrEmpty(SearchStringAddress))
            {
                IQueryable<UserProfile> userProfile = (from u in db.UserProfiles
                                                       where (u.UserName.Contains(SearchStringAddress))
                                                       select u);

                if (userProfile.Count() == 0)
                {
                    TempData["Error"] = "No matching users found";
                    return View();
                }
                if (userProfile.Count() == 1)
                {
                    return RedirectToAction("UserDetails", new { id = userProfile.FirstOrDefault().UserId });
                }

                return View(userProfile.ToList());

            }
            return View();
        }

        // GET: Donations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return HttpNotFound();
            }
            return View(donation);
        }

        // GET: Donations/Create
        public ActionResult Create()
        {
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name");
            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DonationId,HouseId,TypeOfDonation,Amount,ChurchId")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                db.Donations.Add(donation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", donation.ChurchId);
            return View(donation);
        }

        public JsonResult GetAddress(string term)
        {
            var items = db.Houses
                .Where(x => x.AddressLine1.Contains(term))
                .Select(x => new { Label = x.AddressLine1, Value = x.HouseId })
                .Take(10);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        // GET: Donations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", donation.ChurchId);
            return View(donation);
        }

        // POST: Donations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DonationId,HouseId,TypeOfDonation,Amount,ChurchId")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", donation.ChurchId);
            return View(donation);
        }

        // GET: Donations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return HttpNotFound();
            }
            return View(donation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Donation donation = db.Donations.Find(id);
            db.Donations.Remove(donation);
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