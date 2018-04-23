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
using System.Text.RegularExpressions;

namespace PIMS.Controllers
{
    public class AdministrationsController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();


        [Authorize(Roles = "Administrator, Parish Admin")]
        // GET: Administrations
        public ActionResult Index()
        {
            var admins = db.Admins.Include(a => a.AdminChurch);
            return View(admins.ToList());
        }

        public ActionResult AdministrationsPage()
        {
            return View();
        }


        [Authorize(Roles = "Administrator, Parish Admin")]
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
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(string email, string name, string phone, string username)
        {
            ViewBag.Role = new SelectList(new[] { "Secretary", "Parish Admin", "Priest", "Catechist"});
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name");
            ViewBag.email = email;
            ViewBag.name = name;
            ViewBag.phone = phone;
            ViewBag.username = username;
            return View();
        }

        // POST: Administrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdministrationId,AdministratorName,AdminUsername,Password,Position,PhoneNumber,EmailAddress,ChurchId")]string email, string name, string phone, string username, Administration administration)
        {
            if (ModelState.IsValid)
            {
                ViewBag.email = email;
                ViewBag.name = name;
                ViewBag.phone = phone;
                ViewBag.username = username;

                db.Admins.Add(administration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Role = new SelectList(new[] { "Secretary", "Parish Admin", "Priest", "Catechist" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", administration.ChurchId);
            return View(administration);
        }

        // GET: Administrations/Edit/5
        [Authorize(Roles = "Administrator")]
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
            ViewBag.Role = new SelectList(new[] { "Secretary", "Parish Admin", "Priest", "Catechist" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", administration.ChurchId);
            return View(administration);
        }

        // POST: Administrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
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
            ViewBag.Role = new SelectList(new[] { "Secretary", "Parish Admin", "Priest", "Catechist" });
            ViewBag.ChurchId = new SelectList(db.Churches, "ChurchId", "Name", administration.ChurchId);
            return View(administration);
        }

        // GET: Administrations/Delete/5
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
                    if (domain != "gmail.com" || domain != "hotmail.com")
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

        public Administration ValidatePhone(Administration up)
        {
            if (up.PhoneNumber == null)
            {
                TempData["ErrorPhone"] = "A valid phone number must be entered";
            }
            else
            {
                try
                {
                
                    if (!IsPhoneNumber(up.PhoneNumber))
                    {
                        TempData["ErrorPhone"] = "A valid phone number must be entered";
                    }
                }
                catch (Exception e)
                {
                    TempData["ErrorEmail"] = "A valid phone number must be entered";
                }

            }
            return up;
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }
    }
}
