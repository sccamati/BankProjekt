using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankProjekt.DAL;
using BankProjekt.Models;

namespace BankProjekt.Controllers
{
    public class CreditProposalsController : Controller
    {
        private BankContext db = new BankContext();

        // GET: CreditProposals
        public ActionResult Index()
        {
            var creditProposals = db.CreditProposals.Include(c => c.Profile);
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
            ViewBag.ProfileId = new SelectList(db.Profiles, "Id", "Name");
            return View();
        }

        // POST: CreditProposals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProfileId,Cash,NumberOfMonths,ProposalStatus,Picture")] CreditProposal creditProposal)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["File"];
                if (file != null && file.ContentLength > 0)
                {
                    creditProposal.Picture = file.FileName;
                    string filepath = Path.Combine(Server.MapPath("~/Pictures"), file.FileName);
                    file.SaveAs(filepath);
                }
                var Profile = db.Profiles.Single(p => p.Email.Equals(User.Identity.Name));
                creditProposal.Profile = Profile;
                creditProposal.ProposalStatus = CreditProposalStatus.Waiting;
                db.CreditProposals.Add(creditProposal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProfileId = new SelectList(db.Profiles, "Id", "Name", creditProposal.ProfileId);
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
            ViewBag.ProfileId = new SelectList(db.Profiles, "Id", "Name", creditProposal.ProfileId);
            return View(creditProposal);
        }

        // POST: CreditProposals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProfileId,Cash,NumberOfMonths,ProposalStatus,Picture")] CreditProposal creditProposal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(creditProposal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfileId = new SelectList(db.Profiles, "Id", "Name", creditProposal.ProfileId);
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
