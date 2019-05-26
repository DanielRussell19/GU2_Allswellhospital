using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GU2_Allswellhospital.Models;
using Microsoft.AspNet.Identity;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 14/05/2019

    /// <summary>
    /// Controller used to handle CRUD operations for Treatments
    /// </summary>
    [Authorize(Roles = "Doctor,Consultant")]
    public class TreatmentManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TreatmentManagement
        public ActionResult Index(string patientid)
        {
            var treatments = db.Treatments.Include(t => t.Doctor).Include(t => t.Patient).Where(t => t.PatientID == patientid);

            ViewBag.patientid = patientid;

            return View(treatments.ToList());
        }

        // GET: TreatmentManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treatment treatment = db.Treatments.Find(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }
            return View(treatment);
        }

        // GET: TreatmentManagement/Create
        public ActionResult Create(string patientid)
        {
            ViewBag.patientid = patientid;

            return View(new Treatment { PatientID = patientid, DoctorID = User.Identity.GetUserId() });
        }

        // POST: TreatmentManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TreatmentNo,DateofTreatment,TreatmentDetails,TreatmentCost,DoctorID,PatientID")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                //strange improvised fix for forign key error, if staff are not loaded from staffmanagement treatment creation becomes impossible
                db.Set<ApplicationUser>().Load();

                db.Treatments.Add(treatment);
                db.SaveChanges();

                //temp invoice to be used as a new invoice if no unpaid invoice matching patient is found
                BillingInvoice Invoice = new BillingInvoice { PatientID = treatment.PatientID, PaymentRecived = false, TotalDue = treatment.TreatmentCost };

                try
                {
                        //list of invoices
                        var billinginvoices = db.BillingInvoices.Include(i => i.Patient).Include(i => i.Prescriptions).Include(i => i.Treatments).Include(i => i.Payment).ToList();

                        //searches list of invoices for an invoice that matches the patient that is unpaid, else outside of foreach creates new invoice with unpaid status
                        foreach (BillingInvoice invoice in billinginvoices)
                        {
                            //if an invoice is found matching patient and also be unpaid the invoice is appended with the new treatment
                            if(invoice.PatientID == treatment.PatientID && invoice.PaymentRecived == false && invoice.PaymentNo == null)
                            {
                                Invoice = invoice;

                                Invoice.TotalDue = Invoice.TotalDue + treatment.TreatmentCost;
                                treatment.InvoiceNo = Invoice.InvoiceNo;

                                db.Entry(invoice).State = EntityState.Modified;
                                db.Entry(treatment).State = EntityState.Modified;
                                db.SaveChanges();

                                return RedirectToAction("Index", "TreatmentManagement", new { treatment.PatientID });
                            }
                        }

                        //if no invoice found, create new invoice with temp invoice and newly created treatment added within
                        treatment.InvoiceNo = Invoice.InvoiceNo;
                        db.BillingInvoices.Add(Invoice);

                        db.Entry(treatment).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Index", "TreatmentManagement", new { treatment.PatientID });
                }
                catch
                {
                    return View(treatment);
                }

                
            }

            return View(treatment);
        }

        // GET: TreatmentManagement/Edit/5
        public ActionResult Edit(string id, string patientid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treatment treatment = db.Treatments.Find(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }
            
            return View(treatment);
        }

        // POST: TreatmentManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TreatmentNo,DateofTreatment,TreatmentDetails,TreatmentCost,DoctorID,PatientID")] Treatment treatment)
        {
            if (ModelState.IsValid)
            {

                BillingInvoice Invoice = new BillingInvoice { PatientID = treatment.PatientID, PaymentRecived = false, TotalDue = treatment.TreatmentCost };
                Treatment oldtreatment = treatment;

                var billinginvoices = db.BillingInvoices.Include(i => i.Patient).Include(i => i.Prescriptions).Include(i => i.Treatments).Include(i => i.Payment).ToList();

                try
                {

                foreach (BillingInvoice invoice in billinginvoices)
                {
                    if (invoice.PatientID == treatment.PatientID && invoice.PaymentRecived == false && invoice.PaymentNo == null)
                    {
                        Invoice = invoice;

                        foreach (Treatment t in Invoice.Treatments)
                        {
                                if(t.TreatmentNo == treatment.TreatmentNo)
                                {
                                    oldtreatment = t;
                                }
                        }

                        treatment.InvoiceNo = Invoice.InvoiceNo;
                        Invoice.TotalDue = Invoice.TotalDue - oldtreatment.TreatmentCost + treatment.TreatmentCost;

                        //forign key error fix when editing, stating primary key conflict instead of using state.modified (the origin of error, apparently)
                        db.Set<BillingInvoice>().AddOrUpdate(invoice);
                        
                        db.Set<Treatment>().AddOrUpdate(treatment);
                        
                        db.SaveChanges();

                        return RedirectToAction("Index", "TreatmentManagement", new { treatment.PatientID });
                    }
                }

                    treatment.InvoiceNo = Invoice.InvoiceNo;
                    db.BillingInvoices.Add(Invoice);

                    db.Entry(treatment).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index", "TreatmentManagement", new { treatment.PatientID });

                }
                catch
                {
                    return View(treatment);
                }
            }

            
            return View(treatment);
        }

        // GET: TreatmentManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treatment treatment = db.Treatments.Find(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }
            return View(treatment);
        }

        // POST: TreatmentManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Treatment treatment = db.Treatments.Find(id);
            string patientid = treatment.PatientID;

            var billinginvoices = db.BillingInvoices.Include(i => i.Patient).Include(i => i.Prescriptions).Include(i => i.Treatments).Include(i => i.Payment).ToList();
            BillingInvoice Invoice = new BillingInvoice();

            try
            {

                foreach (BillingInvoice invoice in billinginvoices)
                {
                    if (invoice.PatientID == treatment.PatientID && invoice.PaymentRecived == false && invoice.PaymentNo == null)
                    {
                        Invoice = invoice;

                        Invoice.TotalDue = Invoice.TotalDue - treatment.TreatmentCost;
                        Invoice.Treatments.Remove(treatment);

                        db.Entry(invoice).State = EntityState.Modified;
                        db.SaveChanges();

                        db.Treatments.Remove(treatment);
                        db.SaveChanges();
                        return RedirectToAction("Index", "TreatmentManagement", new { patientid });

                    }
                }

            }
            catch
            {
                return View("Error");
            }

            return RedirectToAction("Index", "TreatmentManagement", new { patientid });
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
