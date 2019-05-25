using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GU2_Allswellhospital.Models;

namespace GU2_Allswellhospital.Controllers
{
    public class BillingInvoiceManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BillingInvoiceManagement
        public ActionResult Index()
        {
            var billingInvoices = db.BillingInvoices.Include(b => b.Patient).Include(b => b.Payment);
            return View(billingInvoices.ToList());
        }

        // GET: BillingInvoiceManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var billingInvoices = db.BillingInvoices.Include(i => i.Treatments).Include(i => i.Prescriptions);
            BillingInvoice billingInvoice = new BillingInvoice();

            foreach(BillingInvoice invoice in billingInvoices)
            {
                if(invoice.InvoiceNo == id)
                {
                    billingInvoice = invoice;
                }
            }

            if (billingInvoice == null)
            {
                return HttpNotFound();
            }
            return View(billingInvoice);
        }

        // GET: BillingInvoiceManagement/Create
        public ActionResult Create()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename");
            ViewBag.PaymentNo = new SelectList(db.Payments, "PaymentNo", "PaymentMethod");
            return View();
        }

        // POST: BillingInvoiceManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceNo,PaymentRecived,TotalDue,PatientID,PaymentNo")] BillingInvoice billingInvoice)
        {
            if (ModelState.IsValid)
            {
                db.BillingInvoices.Add(billingInvoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", billingInvoice.PatientID);
            ViewBag.PaymentNo = new SelectList(db.Payments, "PaymentNo", "PaymentMethod", billingInvoice.PaymentNo);
            return View(billingInvoice);
        }

        // GET: BillingInvoiceManagement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillingInvoice billingInvoice = db.BillingInvoices.Find(id);
            if (billingInvoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", billingInvoice.PatientID);
            ViewBag.PaymentNo = new SelectList(db.Payments, "PaymentNo", "PaymentMethod", billingInvoice.PaymentNo);
            return View(billingInvoice);
        }

        // POST: BillingInvoiceManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceNo,PaymentRecived,TotalDue,PatientID,PaymentNo")] BillingInvoice billingInvoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billingInvoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", billingInvoice.PatientID);
            ViewBag.PaymentNo = new SelectList(db.Payments, "PaymentNo", "PaymentMethod", billingInvoice.PaymentNo);
            return View(billingInvoice);
        }

        // GET: BillingInvoiceManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillingInvoice billingInvoice = db.BillingInvoices.Find(id);
            if (billingInvoice == null)
            {
                return HttpNotFound();
            }
            return View(billingInvoice);
        }

        // POST: BillingInvoiceManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BillingInvoice billingInvoice = db.BillingInvoices.Find(id);
            db.BillingInvoices.Remove(billingInvoice);
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
