using BankProjekt.DAL;
using BankProjekt.Helpers;
using BankProjekt.Models;
using BankProjekt.ViewModels;
using PagedList;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BankProjekt.Controllers
{
    public class TransfersController : Controller
    {
        private BankContext db = new BankContext();

        // GET: Transfers
        public ActionResult Index(int? id, string sortOrder, string searchString, int? page, string currentFilter, TransferType? TypeSort, string bankAccount)
        {
            List<BankAccount> bankAccounts = new List<BankAccount>();
            if (id != null)
            {
                bankAccounts = db.Profiles.Single(u => u.Id == id).BankAccounts.ToList();
            }
            else
            {
                bankAccounts = db.Profiles.Single(u => u.Email == User.Identity.Name).BankAccounts.ToList();
            }

            var transfers = db.Transfers.Select(t => t);
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            ViewBag.BankNumbers = bankAccounts.Select(b => b.Number);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.CashSortParam = sortOrder == "Cash" ? "cash_desc" : "Cash";

            var banksA = bankAccounts.Select(b => new
            {
                Id = b.Number,
                Info = $"Number: {b.Number} \n Balance: {b.Balance} "
            });

            ViewBag.BankAccounts = new SelectList(banksA, "Id", "Info");

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(bankAccount))
            {
                transfers = transfers.Where(t => t.AddressesNumber.Equals(bankAccount) || t.ReceiversNumber.Equals(bankAccount));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                transfers = transfers.Where(s => s.Title.Contains(searchString));
            }
            if (TypeSort != null)
            {
                transfers = transfers.Where(t => t.TransferType == TypeSort);
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
            ViewBag.BankNumbers = db.Profiles.Single(p => p.Email.Equals(User.Identity.Name)).BankAccounts.Select(b => b.Number);

            return View(transfer);
        }

        public ActionResult Confirmation(int? id)
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
            ViewBag.BankNumbers = db.Profiles.Single(p => p.Email.Equals(User.Identity.Name)).BankAccounts.Select(b => b.Number);

            return new ViewAsPdf("Confirmation", transfer);
        }

        // GET: Transfers/Create
        public ActionResult Create()
        {
            ViewBag.definedRecipients = db.DefinedRecipients.Where(dr => dr.Profile.Email.Equals(User.Identity.Name)).ToList();

            var bankAccounts = db.Profiles.Single(u => u.Email == User.Identity.Name).BankAccounts.Select(b => new
            {
                b.Id,
                Info = $"Number: {b.Number} \n Balance: {b.Balance} "
            });

            ViewBag.BankAccounts = new SelectList(bankAccounts, "Id", "Info");

            return View(new TransferCreateViewModel());
        }

        // POST: Transfers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, ReceiversName, ReceiversNumber, Title, Cash, Date")] TransferCreateViewModel transferCreate)
        {
            ViewBag.definedRecipients = db.DefinedRecipients.Where(dr => dr.Profile.Email.Equals(User.Identity.Name)).ToList();

            var bankAccounts = db.Profiles.Single(u => u.Email == User.Identity.Name).BankAccounts.Select(b => new
            {
                b.Number,
                Info = $"Number: {b.Number} \n Balance: {b.Balance} "
            });

            ViewBag.BankAccounts = new SelectList(bankAccounts, "Id", "Info");

            if (ModelState.IsValid)
            {
                int id = transferCreate.Id;
                BankAccount bankAccount = db.BankAccounts.Single(b => b.Id.Equals(transferCreate.Id));
                var profile = db.Profiles.Single(p => p.Email.Equals(User.Identity.Name));

                if (bankAccount.Balance < transferCreate.Cash)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    Transfer transfer = new Transfer
                    {
                        AddressesName = profile.Name + " " + profile.LastName,
                        AddressesNumber = bankAccount.Number,
                        TransferType = TransferType.Transfer,
                        ReceiversName = transferCreate.ReceiversName,
                        ReceiversNumber = transferCreate.ReceiversNumber,
                        Title = transferCreate.Title,
                        Cash = transferCreate.Cash,
                        Date = DateTime.Now
                    };

                    bankAccount.Balance -= transferCreate.Cash;

                    transfer.AddresseBalance = bankAccount.Balance;

                    if (db.BankAccounts.Any(b => b.Number.Equals(transferCreate.ReceiversNumber)))
                    {
                        db.BankAccounts.Single(b => b.Number.Equals(transferCreate.ReceiversNumber)).Balance += transferCreate.Cash;

                        transfer.ReceiverBalance = db.BankAccounts.Single(b => b.Number.Equals(transferCreate.ReceiversNumber)).Balance;
                    }

                    db.Transfers.Add(transfer);
                    db.SaveChanges();

                    var DRset = db.DefinedRecipients.Single(df => df.ReceiversName.Equals(transferCreate.ReceiversName) && df.ReceiversNumber.Equals(transferCreate.ReceiversNumber));
                    if (DRset == null)
                    {
                        DefinedRecipient definedRecipient = new DefinedRecipient { ProfileId = bankAccount.ProfileId, ReceiversName = transfer.ReceiversName, ReceiversNumber = transfer.ReceiversNumber };
                        return RedirectToAction("DefinedRecipient", definedRecipient);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                    
                }
            }

            return View();
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

        public ActionResult About()
        {
            IQueryable<TransferDateGroup> data = db.Transfers.GroupBy(t => t.Date).Select(t => new TransferDateGroup
            {
                Date = t.Key,
                TransferCount = t.Count()
            });

            return View(data.ToList());
        }

        // GET: Transfers/Payment
        public ActionResult PaymentAndPayOff()
        {
            var bankAccounts = db.Profiles.Single(u => u.Email == User.Identity.Name).BankAccounts.Select(b => new TransferPaymentAndPayOffViewModel
            {
                Id = b.Id,
                Info = $"Numer {b.Number} \n {b.Balance}zł",
            });
            @ViewBag.BankAccounts = new SelectList(bankAccounts, "Id", "Info");

            List<TransferPaymentAndPayOffViewModel> type = new List<TransferPaymentAndPayOffViewModel>();
            type.Add(new TransferPaymentAndPayOffViewModel { Type = 1, Info = "Payment" });
            type.Add(new TransferPaymentAndPayOffViewModel { Type = 2, Info = "PayOff" });
            @ViewBag.Type = new SelectList(type, "Type", "Info");

            return View(new TransferPaymentAndPayOffViewModel());
        }

        // POST: Transfers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentAndPayOff([Bind(Include = "Id, Cash, Type")] TransferPaymentAndPayOffViewModel transferCreate)
        {
            var bankAccounts = db.Profiles.Single(u => u.Email == User.Identity.Name).BankAccounts.Select(b => new TransferPaymentAndPayOffViewModel
            {
                Id = b.Id,
                Info = $"Numer {b.Number} \n {b.Balance}zł",
            });
            @ViewBag.BankAccounts = new SelectList(bankAccounts, "Id", "Info");

            List<TransferPaymentAndPayOffViewModel> type = new List<TransferPaymentAndPayOffViewModel>();
            type.Add(new TransferPaymentAndPayOffViewModel { Type = 1, Info = "Payment" });
            type.Add(new TransferPaymentAndPayOffViewModel { Type = 2, Info = "PayOff" });
            @ViewBag.Type = new SelectList(type, "Type", "Info");

            if (ModelState.IsValid)
            {
                BankAccount bankAccount = db.BankAccounts.Single(b => b.Id.Equals(transferCreate.Id));
                var profile = db.Profiles.Single(p => p.Email.Equals(User.Identity.Name));

                if (transferCreate.Type == 1)
                {
                    bankAccount.Balance += transferCreate.Cash;
                    Transfer transfer = new Transfer
                    {
                        TransferType = TransferType.Payment,
                        ReceiversName = profile.Name + " " + profile.LastName,
                        ReceiversNumber = bankAccount.Number,
                        Title = "Payment",
                        Cash = transferCreate.Cash,
                        Date = DateTime.Now,
                        ReceiverBalance = bankAccount.Balance
                    };
                    Email.SendMail("pzprojektbank@gmail.com", "Payment", "Payment was made");
                    db.Transfers.Add(transfer);
                    db.SaveChanges();
                }
                else
                {
                    if (bankAccount.Balance < transferCreate.Cash)
                    {
                        @ViewBag.Error = "Not enough money on this account";
                        return View("PaymentAndPayOff");
                    }
                    bankAccount.Balance -= transferCreate.Cash;
                    Transfer transfer = new Transfer
                    {
                        TransferType = TransferType.PayOff,
                        AddressesName = profile.Name + " " + profile.LastName,
                        AddressesNumber = bankAccount.Number,
                        Title = "PayOff",
                        Cash = transferCreate.Cash,
                        Date = DateTime.Now,
                        AddresseBalance = bankAccount.Balance
                    };
                    db.Transfers.Add(transfer);
                    db.SaveChanges();
                }
            }
            @ViewBag.Error = "";
            return View();
        }

        public ActionResult DefinedRecipient(int ProfileId, String ReceiversNumber, String ReceiversName)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DefinedRecipient([Bind(Include = "Id,ProfileId,ReceiversName,ReceiversNumber")] DefinedRecipient definedRecipient)
        {
            if (ModelState.IsValid)
            {
                db.DefinedRecipients.Add(definedRecipient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(definedRecipient);
        }

        [HttpPost]
        public ActionResult CancelDefinedRecipient()
        {
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