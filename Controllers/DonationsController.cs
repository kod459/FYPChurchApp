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
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Index(string SearchStringAddress, int? SearchStringHouseNumber, string sortOrder)
        {

            ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
           
            IQueryable<Donation> allDonations = (from d in db.Donations
                                                 select d);

            
            if (!String.IsNullOrEmpty(SearchStringAddress) && SearchStringHouseNumber != null)
            {
                IQueryable<Donation> donations = (from d in db.Donations
                                                  where (d.House.AddressLine1.Equals(SearchStringAddress) && d.HouseId == SearchStringHouseNumber)
                                                  select d);
                switch (sortOrder)
                {
                    case "Date":
                        donations = donations.OrderBy(s => s.DateRecieved);
                        break;
                    case "date_desc":
                        donations = donations.OrderByDescending(s => s.DateRecieved);
                        break;
                    default:
                        donations = donations.OrderBy(s => s.DateRecieved);
                        break;
                }

                if (donations.Count() == 0)
                {
                    TempData["Error"] = "No donations found";
                    return View();
                }

                return View(donations.ToList());

            }
            else if (String.IsNullOrEmpty(SearchStringAddress) && SearchStringHouseNumber != null)
            {
                IQueryable<Donation> donations = (from d in db.Donations
                                                  where (d.HouseId == SearchStringHouseNumber)
                                                  select d);

                if (donations.Count() == 0)
                {
                    TempData["Error"] = "No matching houses found";
                    return View();
                }

                switch (sortOrder)
                {
                    case "Date":
                        donations = donations.OrderBy(s => s.DateRecieved);
                        break;
                    case "date_desc":
                        donations = donations.OrderByDescending(s => s.DateRecieved);
                        break;
                    default:
                        donations = donations.OrderBy(s => s.DateRecieved);
                        break;
                }

                return View(donations.ToList());
            }
            else if (!String.IsNullOrEmpty(SearchStringAddress) && SearchStringHouseNumber == null)
            {
                IQueryable<Donation> donations = (from d in db.Donations
                                                  where (d.House.AddressLine1.Equals(SearchStringAddress))
                                                  select d);
                if (donations.Count() == 0)
                {
                    TempData["Error"] = "No matching houses found";
                    return View();
                }

                switch (sortOrder)
                {
                    case "Date":
                        donations = donations.OrderBy(s => s.DateRecieved);
                        break;
                    case "date_desc":
                        donations = donations.OrderByDescending(s => s.DateRecieved);
                        break;
                    default:
                        donations = donations.OrderBy(s => s.DateRecieved);
                        break;
                }

                return View(donations.ToList());
            }

            else
            {
                switch (sortOrder)
                {
                    case "Date":
                        allDonations = allDonations.OrderBy(s => s.DateRecieved);
                        break;
                    case "date_desc":
                        allDonations = allDonations.OrderByDescending(s => s.DateRecieved);
                        break;
                    default:
                        allDonations = allDonations.OrderBy(s => s.DateRecieved);
                        break;
                }
                return View();
            }

        }

        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult SearchHouse(string SearchStringAddress, int? SearchStringHouseNumber)
        {
            if (!String.IsNullOrEmpty(SearchStringAddress) && SearchStringHouseNumber != null)
            {
                IQueryable<Donation> donations = (from d in db.Donations
                                                  where (d.House.AddressLine1.Equals(SearchStringAddress) &&  d.HouseId == SearchStringHouseNumber)
                                                  select d);

                if (donations.Count() == 0)
                {
                    TempData["Error"] = "No matching houses found";
                    return View();
                }

                return View(donations.ToList());

            }
            else if (String.IsNullOrEmpty(SearchStringAddress) && SearchStringHouseNumber != null)
            {
                IQueryable<Donation> donations = (from d in db.Donations
                                                  where (d.HouseId == SearchStringHouseNumber)
                                                  select d);

                if (donations.Count() == 0)
                {
                    TempData["Error"] = "No matching houses found";
                    return View();
                }

                return View(donations.ToList());
            }
            else if(!String.IsNullOrEmpty(SearchStringAddress) && SearchStringHouseNumber == null)
            {
                IQueryable<Donation> donations = (from d in db.Donations
                                                  where (d.House.AddressLine1.Equals(SearchStringAddress))
                                                  select d);
                if (donations.Count() == 0)
                {
                    TempData["Error"] = "No matching houses found";
                    return View();
                }

                return View(donations.ToList());
            }

            else
            {
                return View();
            }
        }

        // GET: Donations/Details/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
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
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Create()
        {
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name");
            ViewBag.Type = new SelectList(new[] { "Envelope", "In Person", "Standing Order", "Direct Debit" });
            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult Create([Bind(Include = "DonationId,HouseId,TypeOfDonation,DateRecieved,Amount,ChurchId")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                db.Donations.Add(donation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", donation.ChurchId);
            ViewBag.Type = new SelectList(new[] { "Envelope", "In Person", "Standing Order", "Direct Debit" });
            return View(donation);
        }

        public JsonResult GetAddress(string term)
        {
            var items = db.Houses
                .Where(x => x.AddressLine1.Contains(term))
                .Select(x => new { Label = x.AddressLine1, Value = x.AddressLine1, RealValue = x.HouseId })
                .Take(10);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        // GET: Donations/Edit/5
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
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
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DonationId,HouseId,TypeOfDonation,DateRecievedAmount,ChurchId")] Donation donation)
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
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
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
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
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