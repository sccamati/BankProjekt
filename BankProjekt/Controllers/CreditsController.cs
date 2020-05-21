using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankProjekt.DAL;
using BankProjekt.Models;

namespace BankProjekt.Controllers
{
    public class CreditsController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Credits
        public ActionResult Index()
        {
            var credits = db.Credits.Include(c => c.BankAccount).Where(c => c.BankAccount.Profile.Email.Equals(User.Identity.Name));
            return View(credits.ToList());
        }

        // GET: Credits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Credit credit = db.Credits.Find(id);
            if (credit == null)
            {
                return HttpNotFound();
            }
            return View(credit);
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
