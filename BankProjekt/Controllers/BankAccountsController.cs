using BankProjekt.DAL;
using BankProjekt.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BankProjekt.Controllers
{
    public class BankAccountsController : Controller
    {
        private BankContext db = new BankContext();

        // POST: BankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      
        public ActionResult Create()
        {
            var profile = db.Profiles.Single(p => p.Email.Equals(User.Identity.Name));
            var bankAccounts = db.BankAccounts.Where(b => b.ProfileId == profile.Id).ToList();
            if(bankAccounts.Count < 3)
            {
                var number = db.BankAccounts.Max(n => n.Number);

                BankAccount bankAccount = new BankAccount() { Number = (Int32.Parse(number) + 1).ToString(), Balance = 0, Profile = profile };
                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();
                ViewBag.Error("");
                return RedirectToAction("Index", "Profiles");
            }
            else
            {
                ViewBag.Error("You cant have more than 3 accounts");
                return RedirectToAction("Index", "Profiles");
            }
        }
           

        // GET: BankAccounts/Delete/5
       /* public ActionResult Delete(int? id)
        {
            db.BankAccounts.Remove(db.BankAccounts.Single(b => b.Id == id));
            db.SaveChanges();
            
            return RedirectToAction("Index", "Profiles");
        }
        */
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