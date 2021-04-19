using BankProjekt.DAL;
using BankProjekt.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BankProjekt.Controllers
{
    public class ProfilesController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Profiles
        public ActionResult Index(int? id)
        {

            ViewBag.definedRecipients = db.DefinedRecipients.Where(dr => dr.Profile.Email.Equals(User.Identity.Name)).ToList();
            if (id != null)
            {
                var profileW = db.Profiles.Include(p => p.Address).Single(p => p.Id == id);
                return View(profileW);
            }
            var profile = db.Profiles.Include(p => p.Address).Single(p => p.Email.Equals(User.Identity.Name));
            return View(profile);
        }

        // GET: Profiles/Details/5
        public ActionResult Details(int? id)
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


        // GET: Profiles/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LastName,Email,Pesel,BirthDate,MothersName")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                
            }
            return RedirectToAction("Index", new { id = profile.Id });
        }

        // GET: Profiles/Edit/5
        public ActionResult EditAddress(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Profiles.Find(id).Address;
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAddress([Bind(Include = "Id, HouseNumber, Street, PostCode, City")] Address address)
        {
            Profile profile = null;
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                profile = db.Profiles.Single(p => p.Address.Id.Equals(address.Id));
                db.SaveChanges();
                
            }
            return RedirectToAction("Index", new { id = profile.Id });
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profile profile = db.Profiles.Find(id);
            db.Profiles.Remove(profile);
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