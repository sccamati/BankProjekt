using BankProjekt.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankProjekt.Models;
using BankProjekt.Helpers;

namespace BankProjekt.Controllers
{
    //[Authorize(Roles = "Admin, Worker")]
    public class WorkerController : Controller
    {
        private BankContext db = new BankContext();
        // GET: Worker
        public ActionResult CreditProposalsList()
        {
            var creditProposals = db.CreditProposals.Include(c => c.BankAccount);
            return View(creditProposals.ToList());
        }

        public ActionResult ProfilesList()
        {
            var profile = db.Profiles.Include(p => p.Address);
            return View(profile);
        }

        public ActionResult CreditProposalDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditProposal creditProposal = db.CreditProposals.Find(id);
            if (creditProposal == null)
            {
                return HttpNotFound();
            }
            return View(creditProposal);
        }

        public ActionResult CreditProposalAccept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditProposal cr = db.CreditProposals.Find(id);
            if (cr == null)
            {
                return HttpNotFound();
            }
            else
            {
                

                Credit credit = new Credit()
                {
                    BankAccount = cr.BankAccount,
                    Cash = cr.Cash,
                    MoneyToPay = cr.Cash + Decimal.Multiply(cr.Cash, (decimal)0.1),
                    MonthlyRepayment = cr.Cash / cr.NumberOfMonths,
                    NumberOfMonthsLeft = cr.NumberOfMonths,
                    NumberOfMonths = cr.NumberOfMonths,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(cr.NumberOfMonths)
                };
                cr.ProposalStatus = CreditProposalStatus.Approved;
                db.Credits.Add(credit);
                cr.BankAccount.Balance += cr.Cash;

                Transfer transfer = new Transfer()
                {
                    TransferType = TransferType.Credit,
                    ReceiversName = $"{cr.BankAccount.Profile.Name} {cr.BankAccount.Profile.LastName}",
                    ReceiversNumber = cr.BankAccount.Number,
                    Title = "Credit",
                    Cash = cr.Cash,
                    ReceiverBalance = cr.BankAccount.Balance,
                    Date = DateTime.Now
                };
                db.Transfers.Add(transfer);
                db.SaveChanges();
                Email.SendMail("pzprojektbank@gmail.com", "Credit Proposal Status", "Hello Yours credit proposal was accepted");
            }
            return RedirectToAction("CreditProposalsList");
        }

        public ActionResult CreditProposalReject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditProposal cr = db.CreditProposals.Find(id);
            if (cr == null)
            {
                return HttpNotFound();
            }
            else
            {
                Email.SendMail("pzprojektbank@gmail.com", "Credit Proposal Status", "Hello Yours credit proposal was rejected");
                cr.ProposalStatus = CreditProposalStatus.Rejected;
                db.SaveChanges();
            }
            return RedirectToAction("CreditProposalsList");
        }

        public ActionResult ProfileDetails()
        {
            
            return RedirectToRoute("Profile/Details");
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