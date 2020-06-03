using BankProjekt.DAL;
using BankProjekt.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BankProjekt.Controllers
{
    public class DefinedRecipientsController : Controller
    {
        private BankContext db = new BankContext();

        // GET: DefinedRecipients
        public ActionResult Index()
        {
            return View(db.DefinedRecipients.ToList());
        }


        // GET: DefinedRecipients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefinedRecipient definedRecipient = db.DefinedRecipients.Find(id);
            if (definedRecipient == null)
            {
                return HttpNotFound();
            }
            return View(definedRecipient);
        }

        // POST: DefinedRecipients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProfileId,ReciversName,ReciversNumber")] DefinedRecipient definedRecipient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(definedRecipient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(definedRecipient);
        }

        // GET: DefinedRecipients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefinedRecipient definedRecipient = db.DefinedRecipients.Find(id);
            if (definedRecipient == null)
            {
                return HttpNotFound();
            }
            return View(definedRecipient);
        }

        // POST: DefinedRecipients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DefinedRecipient definedRecipient = db.DefinedRecipients.Find(id);
            db.DefinedRecipients.Remove(definedRecipient);
            db.SaveChanges();
            return RedirectToAction("Index", "Profiles");
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