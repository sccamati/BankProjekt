using BankProjekt.DAL;
using BankProjekt.Helpers;
using BankProjekt.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

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

        public ActionResult ProfilesList(string nameS, string lastNameS, string emailS, string peselS, string nameFilter, string lastNameFilter, string peselFilter, string emailFilter, int? page)
        {
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

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(profiles.ToPagedList(pageNumber, pageSize));
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