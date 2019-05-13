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
    //Daniel Russell 12/05/2019

    /// <summary>
    /// Controller used to handle CRUD operations of Billing invoices for treatments
    /// </summary>
    public class BillingInvoiceManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BillingInvoiceManagement
        public ActionResult Index()
        {
            var billingInvoices = db.Invoices.Include(b => b.Patient).Include(b => b.Payment).Include(b => b.Treatment);
            return View(billingInvoices.ToList());
        }

        // GET: BillingInvoiceManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillingInvoice billingInvoice = db.Invoices.Find(id);
            if (billingInvoice == null)
            {
                return HttpNotFound();
            }
            return View(billingInvoice);
        }

        // GET: BillingInvoiceManagement/Create
        public ActionResult Create()
        {
            ViewBag.PatientID = new SelectList(db.ApplicationUsers, "Id", "Forename");
            ViewBag.PaymentNo = new SelectList(db.Payments, "PaymentNo", "PaymentMethod");
            ViewBag.TreatmentNo = new SelectList(db.Treatments, "TreatmentNo", "TreatmentDetails");
            return View();
        }

        // POST: BillingInvoiceManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceNo,PaymentRecived,TotalDue,PatientID,TreatmentNo,PaymentNo")] BillingInvoice billingInvoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(billingInvoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.ApplicationUsers, "Id", "Forename", billingInvoice.PatientID);
            ViewBag.PaymentNo = new SelectList(db.Payments, "PaymentNo", "PaymentMethod", billingInvoice.PaymentNo);
            ViewBag.TreatmentNo = new SelectList(db.Treatments, "TreatmentNo", "TreatmentDetails", billingInvoice.TreatmentNo);
            return View(billingInvoice);
        }

        // GET: BillingInvoiceManagement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillingInvoice billingInvoice = db.Invoices.Find(id);
            if (billingInvoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientID = new SelectList(db.ApplicationUsers, "Id", "Forename", billingInvoice.PatientID);
            ViewBag.PaymentNo = new SelectList(db.Payments, "PaymentNo", "PaymentMethod", billingInvoice.PaymentNo);
            ViewBag.TreatmentNo = new SelectList(db.Treatments, "TreatmentNo", "TreatmentDetails", billingInvoice.TreatmentNo);
            return View(billingInvoice);
        }

        // POST: BillingInvoiceManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceNo,PaymentRecived,TotalDue,PatientID,TreatmentNo,PaymentNo")] BillingInvoice billingInvoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billingInvoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.ApplicationUsers, "Id", "Forename", billingInvoice.PatientID);
            ViewBag.PaymentNo = new SelectList(db.Payments, "PaymentNo", "PaymentMethod", billingInvoice.PaymentNo);
            ViewBag.TreatmentNo = new SelectList(db.Treatments, "TreatmentNo", "TreatmentDetails", billingInvoice.TreatmentNo);
            return View(billingInvoice);
        }

        // GET: BillingInvoiceManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillingInvoice billingInvoice = db.Invoices.Find(id);
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
            BillingInvoice billingInvoice = db.Invoices.Find(id);
            db.Invoices.Remove(billingInvoice);
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
