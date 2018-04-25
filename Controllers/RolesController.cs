using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using PIMS.Filters;
using PIMS.Models;
using PIMS.Entities;
using PIMS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace PIMS.Controllers
{
    public class RolesController : Controller
    {
        private ChurchDBContext db = new ChurchDBContext();


        //
        // GET: /Roles/

        //GET
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult UsersNoRoles()
        {
            var users = (from u in db.UserProfiles
                         select u);

            var listOfUsers = users.ToList();

            var usersNoRoles = new List<UserProfile>();

            foreach (var u in listOfUsers)
            {

                var selectList = new SelectList(Roles.GetRolesForUser(u.UserName));
                var checkList = selectList.ToList();

                if (checkList.Count() == 0)
                {
                    usersNoRoles.Add(u);

                }
            }


            return View(usersNoRoles);
        }

        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult AllUsers()
        {
            var allUsers = new SelectList(Roles.GetUsersInRole("Lay User"));
            var allUsersWithRole = allUsers.ToList();
            var usersInLayUser = new List<UserAndRoles>();

            foreach (var ur in allUsersWithRole)
            {
                var userDetails = (from u in db.UserProfiles
                                   where u.UserName == ur.Text
                                   select u);
                UserAndRoles userAndRoles = new UserAndRoles();
                userAndRoles.LayUser = "X";

                userAndRoles.UserId = userDetails.FirstOrDefault().UserId;
                userAndRoles.UserName = userDetails.FirstOrDefault().UserName.ToString();
                usersInLayUser.Add(userAndRoles);
            }

            return View(usersInLayUser.ToList());

        }

        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult UsersInLayUserRole()
        {
            var allUsers = new SelectList(Roles.GetUsersInRole("Lay User"));
            var allUsersWithRole = allUsers.ToList();
            var usersInLayUser = new List<UserAndRoles>();

            foreach (var ur in allUsersWithRole)
            {
                var userDetails = (from u in db.UserProfiles
                                   where u.UserName == ur.Text
                                   select u);
                UserAndRoles userAndRoles = new UserAndRoles();
                userAndRoles.LayUser = "X";

                userAndRoles.UserId = userDetails.FirstOrDefault().UserId;
                userAndRoles.UserName = userDetails.FirstOrDefault().UserName.ToString();
                usersInLayUser.Add(userAndRoles);
            }

            return View(usersInLayUser.ToList());
        }

        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult UsersInVolunteer()
        {
            var allUsers = new SelectList(Roles.GetUsersInRole("Volunteer"));
            var allUsersWithRole = allUsers.ToList();
            var usersWithVolnteer = new List<UserAndRoles>();

            foreach (var ur in allUsersWithRole)
            {
                var userDetails = (from u in db.UserProfiles
                                   where u.UserName == ur.Text
                                   select u);
                UserAndRoles userAndRoles = new UserAndRoles();
                userAndRoles.VolunteerRole = "X";

                userAndRoles.UserId = userDetails.FirstOrDefault().UserId;
                userAndRoles.UserName = userDetails.FirstOrDefault().UserName.ToString();
                usersWithVolnteer.Add(userAndRoles);
            }

            return View(usersWithVolnteer.ToList());
        }


        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult UsersInParishAdmins()
        {
            var allUsers = new SelectList(Roles.GetUsersInRole("Parish Admins"));
            var allUsersWithRole = allUsers.ToList();
            var usersWithParishAdmins = new List<UserAndRoles>();

            foreach (var ur in allUsersWithRole)
            {
                var userDetails = (from u in db.UserProfiles
                                   where u.UserName == ur.Text
                                   select u);
                UserAndRoles userAndRoles = new UserAndRoles();
                userAndRoles.ParishAdminRole = "X";

                userAndRoles.UserId = userDetails.FirstOrDefault().UserId;
                userAndRoles.UserName = userDetails.FirstOrDefault().UserName.ToString();
                usersWithParishAdmins.Add(userAndRoles);
            }

            return View(usersWithParishAdmins.ToList());
        }

        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult UsersInPriest()
        {
            var allUsers = new SelectList(Roles.GetUsersInRole("Priest"));
            var allUsersWithRole = allUsers.ToList();
            var usersWithPriests = new List<UserAndRoles>();

            foreach (var ur in allUsersWithRole)
            {
                var userDetails = (from u in db.UserProfiles
                                   where u.UserName == ur.Text
                                   select u);
                UserAndRoles userAndRoles = new UserAndRoles();
                userAndRoles.PriestRole = "X";

                userAndRoles.UserId = userDetails.FirstOrDefault().UserId;
                userAndRoles.UserName = userDetails.FirstOrDefault().UserName.ToString();
                usersWithPriests.Add(userAndRoles);
            }

            return View(usersWithPriests.ToList());
        }


        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult SearchUserInUserRoles(string SearchStringName)
        {
            if (!String.IsNullOrEmpty(SearchStringName))
            {
                IQueryable<UserProfile> userProfile = (from u in db.UserProfiles
                                                       where (u.UserName.Contains(SearchStringName))
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

        //GET
        [Authorize(Roles = "Parish Admin, Priest, Administrator")]
        public ActionResult UserDetails(int id = 0)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            ViewBag.RolesForThisUser = Roles.GetRolesForUser(userProfile.UserName);

            if (userProfile == null)
            {
                return HttpNotFound();
            }
            else
            {
                SelectList list = new SelectList(Roles.GetRolesForUser(userProfile.UserName));
                var orderList = new List<string>();
                var levelOfRole = "N"; 
                var role = "Lay User";
                foreach (var r in list.ToList())
                {
                    if (r.Text == "Administator")
                    { levelOfRole = "A"; }
                    if ((r.Text == "Lay User"))
                    { levelOfRole = "U"; }
                    if ((r.Text == "Parish Admin") &&
                        ((levelOfRole == "U") || (levelOfRole == "N")))
                    { levelOfRole = "PA"; }
                }
                switch (levelOfRole)
                {
                    case "PA":
                        role = "Administrator";
                        break;
                    case "P":
                        role = "Parish Admin";
                        break;
                    case "V":
                        role = "Parish Admin";
                        break;
                    case "U":
                        role = "Volunteer";
                        break;
                    default:
                        break;
                }

                ViewBag.Role = role;
            }


            return View(userProfile);
        }

        //POST
        [Authorize(Roles = "Administrator, Parish Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserDetails(string RoleName, string UserName, int id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            ViewBag.RolesForThisUser = Roles.GetRolesForUser(userProfile.UserName);

            var checkIfVolunteerAlreadyExists = db.Volunteers
                    .Any(b => (userProfile.Email == b.Email));


            if (Roles.IsUserInRole(UserName, RoleName))
            {
                ViewBag.ResultMessage = "This user already has the role specified!";
            }
            else
            {
                Roles.AddUserToRole(UserName, RoleName);
                ViewBag.RolesForThisUser = Roles.GetRolesForUser(userProfile.UserName);
                ViewBag.ResultMessage = "Username added to the role succesfully!";


            }
            if (RoleName.Equals("Volunteer") && !checkIfVolunteerAlreadyExists)
            {
                string email = userProfile.Email;
                string name = userProfile.Name;
                string phone = userProfile.PhoneNumber;
                string username = userProfile.UserName;
                Roles.RemoveUserFromRole(UserName, "Lay User");


                return RedirectToAction("Create", "Volunteers", new { email = email, name = name, phone = phone, username = username });
            }
            else
            {
                return View(userProfile);
            }

        }

        //GET
        [Authorize(Roles = "Administrator")]
        public ActionResult AddToParishAdmin(int id = 0)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            ViewBag.RolesForThisUser = Roles.GetRolesForUser(userProfile.UserName);

            if (userProfile == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.Role = "Parish Admin";
            }


            return View(userProfile);
        }

        //POST
        [Authorize(Roles = "Administrator, Parish Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToParishAdmin(string RoleName, string UserName, int id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            ViewBag.RolesForThisUser = Roles.GetRolesForUser(userProfile.UserName);

            var checkIfParishAdminExists = db.Admins
                    .Any(b => (userProfile.Email == b.EmailAddress));


            if (Roles.IsUserInRole(UserName, RoleName))
            {
                ViewBag.ResultMessage = "This user already has the role specified!";
            }
            else
            {
                Roles.AddUserToRole(UserName, RoleName);
                ViewBag.RolesForThisUser = Roles.GetRolesForUser(userProfile.UserName);
                ViewBag.ResultMessage = "Username added to the role succesfully!";


            }
            if (RoleName.Equals("Parish Admin") && !checkIfParishAdminExists)
            {
                string email = userProfile.Email;
                string name = userProfile.Name;
                string phone = userProfile.PhoneNumber;
                string username = userProfile.UserName;
                Roles.RemoveUserFromRole(UserName, "Lay User");


                return RedirectToAction("Create", "Administrations", new { email = email, name = name, phone = phone, username = username });
            }
            else
            {
                return View(userProfile);
            }

        }


        public void RedirectToDetails(int id)
        {
            RedirectToAction("Details/" + id.ToString());
        }
    }
}

