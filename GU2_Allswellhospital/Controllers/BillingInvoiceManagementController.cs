using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GU2_Allswellhospital.Models;
using Stripe;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 13/05/2019

    /// <summary>
    /// Controller for CRUD operations with BillingInvoice
    /// </summary>
    [Authorize(Roles = "Doctor,Consultant,MedicalRecordsStaff")]
    public class BillingInvoiceManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BillingInvoiceManagement
        public ActionResult Index(string patientid)
        {
            var billingInvoices = db.BillingInvoices.Include(b => b.Patient).Include(b => b.Payment).Where(p => p.PatientID == patientid);

            ViewBag.patientid = patientid;

            return View(billingInvoices.ToList());
        }

        // GET: BillingInvoiceManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var billingInvoices = db.BillingInvoices.Include(i => i.Treatments).Include(i => i.Prescriptions).Include(i => i.Patient).Include(i => i.Payment);
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

        // GET: BillingInvoiceManagement/Edit/5
        public ActionResult MakePayment(string id,string patientid)
        {
            ViewBag.patientid = patientid;

            return View(new CreatePaymentViewModel { PatientId = patientid, InvoiceNo = id });
        }

        // POST: BillingInvoiceManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakePayment([Bind(Include = "PaymentNo,PaymentMethod,BillingAddress,Forename,Surname,CardNumber,SecurityCode,ExpiryDate,PatientId,InvoiceNo")] CreatePaymentViewModel payment)
        {

            if (ModelState.IsValid)
            {

                StripeConfiguration.SetApiKey("sk_test_fHaiXwbfFo3YUowus0cFNdOR00HHNl42Yw");
                var paymentIntentService = new PaymentIntentService();
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = 999,
                    Currency = "gbp",
                    PaymentMethodTypes = new List<string> { "card" },
                    ReceiptEmail = "danielrussell19@gmail.com",
                };
                paymentIntentService.Create(createOptions);

                return RedirectToAction("Index", "BillingInvoiceManagement", new { payment.PatientId });
            }

            return View(payment);
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
            var billingInvoices = db.BillingInvoices.Include(i => i.Prescriptions).Include(i => i.Treatments);
            BillingInvoice invoice = null;
            string patientid = null;

            foreach (BillingInvoice billingInvoice in billingInvoices)
            {
                if(billingInvoice.InvoiceNo == id)
                {
                    invoice = billingInvoice;
                    patientid = invoice.PatientID;

                    db.Entry(invoice).State = EntityState.Deleted;
                    break;
                    
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index", "BillingInvoiceManagement", new { patientid });
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
