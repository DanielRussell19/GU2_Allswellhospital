using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GU2_Allswellhospital.Models;
using Microsoft.AspNet.Identity;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 13/05/2019

    /// <summary>
    /// Controller for CRUD operations with Prescription
    /// </summary>
    [Authorize(Roles = "Doctor,Consultant")]
    public class PrescriptionManagementController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PrescriptionManagement
        public ActionResult Index(string patientid)
        {
            var prescriptions = db.Prescriptions.Include(p => p.Doctor).Include(p => p.Patient).Where(p => p.PatientID == patientid);

            ViewBag.patientid = patientid;

            return View(prescriptions.ToList());
        }

        // GET: PrescriptionManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // GET: PrescriptionManagement/Create
        public ActionResult Create(string patientid)
        {
            ViewBag.patientid = patientid;

            ViewBag.Drugs = (db.Drugs.ToList());
            return View(new Prescription { PatientID = patientid, DoctorID = User.Identity.GetUserId() });
        }

        // POST: PrescriptionManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptionNo,Dosage,Lengthofprescription,DateofPrescription,DoctorID,PatientID")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {

                db.Prescriptions.Add(prescription);
                db.SaveChanges();

                //temp invoice to be used as a new invoice if no unpaid invoice matching patient is found
                BillingInvoice Invoice = new BillingInvoice { PatientID = prescription.PatientID, PaymentRecived = false, TotalDue = prescription.PrescriptionCost };

                try
                {
                    //list of invoices
                    var billinginvoices = db.BillingInvoices.Include(i => i.Patient).Include(i => i.Prescriptions).Include(i => i.Treatments).Include(i => i.Payment).ToList();

                    //searches list of invoices for an invoice that matches the patient that is unpaid, else outside of foreach creates new invoice with unpaid status
                    foreach (BillingInvoice invoice in billinginvoices)
                    {
                        //if an invoice is found matching patient and also be unpaid the invoice is appended with the new prescription
                        if (invoice.PatientID == prescription.PatientID && invoice.PaymentRecived == false && invoice.PaymentNo == null)
                        {
                            Invoice = invoice;

                            Invoice.TotalDue = Invoice.TotalDue + prescription.PrescriptionCost;
                            prescription.InvoiceNo = Invoice.InvoiceNo;

                            db.Entry(invoice).State = EntityState.Modified;
                            db.Entry(prescription).State = EntityState.Modified;
                            db.SaveChanges();

                            return RedirectToAction("Index", "prescriptionManagement", new { prescription.PatientID });
                        }
                    }

                    //if no invoice found, create new invoice with temp invoice and newly created prescription added within
                    prescription.InvoiceNo = Invoice.InvoiceNo;
                    db.BillingInvoices.Add(Invoice);

                    db.Entry(prescription).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index", "prescriptionManagement", new { prescription.PatientID });
                }
                catch
                {
                    ViewBag.Drugs = (db.Drugs.ToList());
                    return View(prescription);
                }


            }

            ViewBag.Drugs = (db.Drugs.ToList());
            return View(prescription);
        }

        // GET: PrescriptionManagement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // POST: PrescriptionManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrescriptionNo,Dosage,Lengthofprescription,DateofPrescription,DoctorID,PatientID")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prescription);
        }

        // GET: PrescriptionManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // POST: PrescriptionManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Prescription prescription = db.Prescriptions.Find(id);
            db.Prescriptions.Remove(prescription);
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
