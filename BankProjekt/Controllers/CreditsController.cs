using BankProjekt.DAL;
using BankProjekt.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BankProjekt.Controllers
{
    public class CreditsController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Credits
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                var creditsW = db.Credits.Include(c => c.BankAccount).Where(c => c.BankAccount.Profile.Id == id);
                return View(creditsW.ToList());
            }
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
        // GET: Credits/Details/5
        public ActionResult CreditPayment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var credit = db.Credits.Find(id);
            var profile = db.Profiles.Single(p => p.Email.Equals(User.Identity.Name));
            if(credit.MonthlyRepayment <= credit.BankAccount.Balance)
            {
                credit.BankAccount.Balance -= credit.MoneyToPay;
                Transfer transfer = new Transfer()
                {
                    
                    AddressesName = profile.Name,
                    AddressesNumber = credit.BankAccount.Number,
                    AddresseBalance = credit.BankAccount.Balance,
                    Title = "Credit Repayment",
                    Cash = credit.MonthlyRepayment,
                    TransferType = TransferType.CreditPayment,
                    Date = DateTime.Now
                };

                db.Transfers.Add(transfer);

                credit.MoneyToPay -= credit.MonthlyRepayment;
                credit.NumberOfMonthsLeft--;

                if(credit.NumberOfMonths == 0)
                {
                    credit.Status = CreditStatus.Repayed;
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult TotalCreditPayment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var credit = db.Credits.Find(id);
            var profile = db.Profiles.Single(p => p.Email.Equals(User.Identity.Name));
            if (credit.MoneyToPay <= credit.BankAccount.Balance)
            {
                credit.BankAccount.Balance -= credit.MoneyToPay;
                Transfer transfer = new Transfer()
                {
                    AddressesName = profile.Name,
                    AddressesNumber = credit.BankAccount.Number,
                    AddresseBalance = credit.BankAccount.Balance,
                    Title = "Credit Repayment",
                    Cash = credit.MoneyToPay,
                    TransferType = TransferType.CreditPayment,
                    Date = DateTime.Now
                };

                db.Transfers.Add(transfer);

                credit.MoneyToPay = 0;
                credit.NumberOfMonthsLeft = 0;
                credit.Status = CreditStatus.Repayed;
                db.SaveChanges();
            }
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