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
    //Daniel Russell 14/05/2019

    /// <summary>
    /// Controller used to handle CRUD operations for Treatments
    /// </summary>
    [Authorize(Roles = "Doctor,Consultant,MedicalRecordsStaff,Nurse,StaffNurse")]
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

            ViewBag.DoctorID = new SelectList(db.ApplicationUsers, "Id", "Forename");

            return View(new Treatment { PatientID = patientid });
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
                                Invoice.Treatments.Add(treatment);

                                db.Entry(invoice).State = EntityState.Modified;
                                db.Entry(treatment).State = EntityState.Modified;
                                db.SaveChanges();

                                return RedirectToAction("Index", "TreatmentManagement", new { treatment.PatientID });
                            }
                        }

                        //if no invoice found, create new invoice with temp invoice and newly created treatment added within
                        treatment.InvoiceNo = Invoice.InvoiceNo;
                        Invoice.Treatments.Add(treatment);
                        db.BillingInvoices.Add(Invoice);
                        db.Entry(treatment).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Index", "TreatmentManagement", new { treatment.PatientID });
                }
                catch
                {
                    ViewBag.DoctorID = new SelectList(db.ApplicationUsers, "Id", "Forename", treatment.DoctorID);
                    ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", treatment.PatientID);
                    return View(treatment);
                }

                
            }

            ViewBag.DoctorID = new SelectList(db.ApplicationUsers, "Id", "Forename", treatment.DoctorID);
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", treatment.PatientID);
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
            ViewBag.DoctorID = new SelectList(db.ApplicationUsers, "Id", "Forename", treatment.DoctorID);
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", treatment.PatientID);
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
                Treatment oldtreatment = db.Treatments.Find(treatment.TreatmentNo);

                BillingInvoice Invoice = new BillingInvoice { PatientID = treatment.PatientID, PaymentRecived = false, TotalDue = treatment.TreatmentCost };

                var billinginvoices = db.BillingInvoices.Include(i => i.Patient).Include(i => i.Prescriptions).Include(i => i.Treatments).Include(i => i.Payment).ToList();

                try
                {

                foreach (BillingInvoice invoice in billinginvoices)
                {
                    if (invoice.PatientID == treatment.PatientID && invoice.PaymentRecived == false && invoice.PaymentNo == null)
                    {
                        Invoice = invoice;
                        Invoice.TotalDue = Invoice.TotalDue - oldtreatment.TreatmentCost;
                        Invoice.Treatments.Remove(oldtreatment);
                        Invoice.Treatments.Add(treatment);
                        Invoice.TotalDue = Invoice.TotalDue + treatment.TreatmentCost;

                        db.Entry(treatment).State = EntityState.Modified;

                        db.SaveChanges();

                        db.Entry(invoice).State = EntityState.Modified;

                        db.SaveChanges();

                        return RedirectToAction("Index", "TreatmentManagement", new { treatment.PatientID });
                    }
                }

                Invoice.Treatments.Add(treatment);
                db.BillingInvoices.Add(Invoice);
                db.SaveChanges();

                return RedirectToAction("Index", "TreatmentManagement", new { treatment.PatientID });

                }
                catch
                {
                    ViewBag.DoctorID = new SelectList(db.ApplicationUsers, "Id", "Forename", treatment.DoctorID);
                    ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", treatment.PatientID);
                    return View(treatment);
                }
            }

            ViewBag.DoctorID = new SelectList(db.ApplicationUsers, "Id", "Forename", treatment.DoctorID);
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", treatment.PatientID);
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
