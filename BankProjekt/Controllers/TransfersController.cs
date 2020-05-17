using BankProjekt.DAL;
using BankProjekt.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web;


namespace BankProjekt.Controllers
{
    public class TransfersController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Transfers
        public ActionResult Index(int? id, string sortOrder, string searchString, int? page, string currentFilter)
        {
            var bankAccounts = db.Profiles.Single(u => u.Email == User.Identity.Name).BankAccounts;
            var transfers = db.Transfers.Select(t => t);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.CashSortParam = sortOrder == "Cash" ? "cash_desc" : "Cash";

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                transfers = transfers.Where(s => s.Title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Date":
                    transfers = transfers.OrderBy(s => s.Date);
                    break;

                case "date_desc":
                    transfers = transfers.OrderByDescending(s => s.Date);
                    break;

                case "Cash":
                    transfers = transfers.OrderBy(s => s.Cash);
                    break;

                case "cash_desc":
                    transfers = transfers.OrderByDescending(s => s.Cash);
                    break;

                default:
                    transfers = transfers.OrderByDescending(s => s.Date);
                    break;
            }

            if (id == null)
            {
                List<Transfer> list = new List<Transfer>();

                foreach (var item in bankAccounts)
                {
                    list.AddRange(transfers.Where(t => t.AddressesNumber.Equals(item.Number) || t.ReceiversNumber.Equals(item.Number)));
                }
                list.OrderByDescending(t => t.Date);

                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var bankAccountsNumber = bankAccounts.Single(b => b.Id.Equals(id)).Number;
                transfers = transfers.Where(t => t.AddressesNumber.Equals(bankAccountsNumber) || t.ReceiversNumber.Equals(bankAccountsNumber));

                return View(transfers.ToPagedList(pageNumber, pageSize));
            }
        }

        // GET: Transfers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transfer transfer = db.Transfers.Find(id);
            if (transfer == null)
            {
                return HttpNotFound();
            }
            return View(transfer);
        }

        // GET: Transfers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transfers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TransferType,AddressesNumber,ReceiversName,AddressesName,ReceiversNumber,Title,Cash,Date")] Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                db.Transfers.Add(transfer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transfer);
        }

        // GET: Transfers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transfer transfer = db.Transfers.Find(id);
            if (transfer == null)
            {
                return HttpNotFound();
            }
            return View(transfer);
        }

        // POST: Transfers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TransferType,AddressesNumber,ReceiversName,AddressesName,ReceiversNumber,Title,Cash,Date")] Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transfer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transfer);
        }

        // GET: Transfers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transfer transfer = db.Transfers.Find(id);
            if (transfer == null)
            {
                return HttpNotFound();
            }
            return View(transfer);
        }

        // POST: Transfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transfer transfer = db.Transfers.Find(id);
            db.Transfers.Remove(transfer);
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

        public ActionResult About()
        {
            IQueryable<TransferDateGroup> data = db.Transfers.GroupBy(t => t.Date).Select( t => new TransferDateGroup
            {
                Date = t.Key,
                TransferCount = t.Count()
            });

      

            return View(data.ToList());
        }
    }
}