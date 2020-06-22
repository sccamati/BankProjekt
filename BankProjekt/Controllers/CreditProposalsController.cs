using BankProjekt.DAL;
using BankProjekt.Models;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BankProjekt.Controllers
{
    public class CreditProposalsController : Controller
    {
        private BankContext db = new BankContext();

        // GET: CreditProposals
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                var creditProposalsW = db.CreditProposals.Include(c => c.BankAccount).Where(b => b.BankAccount.Profile.Id == id);
                return View(creditProposalsW.ToList());
            }

            var creditProposals = db.CreditProposals.Include(c => c.BankAccount).Where(b => b.BankAccount.Profile.Email.Equals(User.Identity.Name));
            return View(creditProposals.ToList());
        }

        // GET: CreditProposals/Details/5
        public ActionResult Details(int? id)
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

        // GET: CreditProposals/Create
        public ActionResult Create()
        {
            var bankAccounts = db.Profiles.Single(u => u.Email == User.Identity.Name).BankAccounts.Select(b => new
            {
                BankAccount = b.Id,
                Info = $"Number: {b.Number} \n Balance: {b.Balance} "
            });

            ViewBag.BankAccounts = new SelectList(bankAccounts, "BankAccount", "Info");
            ViewBag.ProfileId = new SelectList(db.Profiles, "Id", "Name");
            return View();
        }

        // POST: CreditProposals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BankAccountId,Cash,NumberOfMonths")] CreditProposal creditProposal)
        {
            var bankAccounts = db.Profiles.Single(u => u.Email == User.Identity.Name).BankAccounts.Select(b => new
            {
                Id = b.Id,
                Info = $"Number: {b.Number} \n Balance: {b.Balance} "
            });

            ViewBag.BankAccounts = new SelectList(bankAccounts, "BankAccount", "Info");

            if (ModelState.IsValid)
            {
                

                creditProposal.ProposalStatus = CreditProposalStatus.Waiting;
                db.CreditProposals.Add(creditProposal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(creditProposal);
        }

        // GET: CreditProposals/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: CreditProposals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProfileId,BankAccountId,Cash,NumberOfMonths,Picture")] CreditProposal creditProposal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(creditProposal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(creditProposal);
        }

        // GET: CreditProposals/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: CreditProposals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CreditProposal creditProposal = db.CreditProposals.Find(id);
            db.CreditProposals.Remove(creditProposal);
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