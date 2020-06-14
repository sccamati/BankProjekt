using BankProjekt.DAL;
using BankProjekt.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BankProjekt.Controllers
{
    public class HomeController : Controller
    {
        private BankContext db = new BankContext();

        public ActionResult UserIndex()
        {
            var transfers = db.Transfers.Select(t => t);
            var bankAccounts = db.Profiles.Single(u => u.Email == User.Identity.Name).BankAccounts;

            ViewBag.BankAccounts = bankAccounts.ToList();
            List<Transfer> list = new List<Transfer>();

            foreach (var item in bankAccounts)
            {
                list.AddRange(transfers.Where(t => t.AddressesNumber.Equals(item.Number) || t.ReceiversNumber.Equals(item.Number)));
            }
            ViewBag.BankNumbers = bankAccounts.Select(b => b.Number);
            return View(list.Take(5).OrderBy(t => t.Date));
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Start()
        {
            return View();
        }
    }
}