using BankProjekt.DAL;
using BankProjekt.Helpers;
using BankProjekt.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BankProjekt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private BankContext db = new BankContext();



        // GET: CreditProposals
        public ActionResult Index(string nameS, string lastNameS, string emailS, string peselS, string nameFilter, string lastNameFilter, string peselFilter, string emailFilter, string UserType, int? page)
        {

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var roles = roleManager.Roles.Select(r => new
            {
                Id = r.Id,
                Name = r.Name
            });

            ViewBag.UserType = new SelectList(roles, "Id", "Name");

            var profiles = db.Profiles.Include(p => p.Address);


            if (nameS != null)
            {
                page = 1;
            }
            else
            {
                nameS = nameFilter;
            }

            ViewBag.nameFilter = nameS;

            if (lastNameS != null)
            {
                page = 1;
            }
            else
            {
                lastNameS = lastNameFilter;
            }

            ViewBag.lastNameFilter = lastNameS;

            if (emailS != null)
            {
                page = 1;
            }
            else
            {
                emailS = emailFilter;
            }

            ViewBag.emailFilter = emailS;

            if (peselS != null)
            {
                page = 1;
            }
            else
            {
                peselS = peselFilter;
            }

            ViewBag.peselFilter = peselS;


            if (!String.IsNullOrEmpty(nameS))
            {
                profiles = profiles.Where(p => p.Name.Equals(nameS));
            }
            if (!String.IsNullOrEmpty(lastNameS))
            {
                profiles = profiles.Where(p => p.LastName.Equals(lastNameS));
            }
            if (!String.IsNullOrEmpty(emailS))
            {
                profiles = profiles.Where(p => p.Email.Equals(emailS));
            }
            if (!String.IsNullOrEmpty(peselS))
            {
                profiles = profiles.Where(p => p.Pesel.Equals(peselS));
            }

            profiles = profiles.OrderBy(p => p.LastName);


            List<Profile> profileList = new List<Profile>();

            if (!String.IsNullOrEmpty(UserType))
            {
                var role = roleManager.FindById(UserType);
                
                foreach (var user in profiles)
                {
                    if (userManager.FindByName(user.Email).Roles.Any(u => u.RoleId.Equals(role.Id)))
                    {
                        profileList.Add(user);
                    }
                }
            }
            else
            {
                profileList = profiles.ToList();
            }

           

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(profileList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Transfers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel model)
        {
            

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            if (ModelState.IsValid)
            {
                var worker = new ApplicationUser { UserName = model.Email };

                userManager.Create(worker, model.Password);
                userManager.AddToRoleAsync(worker.Id, "Worker");

                Address address = new Address { HouseNumber = model.HouseNumber, Street = model.Street, PostCode = model.PostalCode, City = model.City };
                db.Addresses.Add(address);
                Profile profile = new Profile { Address = address, Name = model.Name, LastName = model.LastName, Email = model.Email, Pesel = model.PESEL, BirthDate = model.BirthDate, MothersName = model.MothersName };
                db.Profiles.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Profiles/Delete/5
        public ActionResult DeleteWorker(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("DeleteWorker")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
            Profile profile = db.Profiles.Find(id);
            Address address = db.Addresses.Find(profile.Address.Id);
            db.Addresses.Remove(address);
            db.Profiles.Remove(profile);
            userManager.Delete(userManager.FindByName(profile.Email));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}