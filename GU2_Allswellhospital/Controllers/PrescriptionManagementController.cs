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
using Microsoft.AspNet.Identity;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 13/05/2019

    /// <summary>
    /// Controller for CRUD operations with Prescription
    /// </summary>
    [Authorize(Roles = "Doctor,Consultant,MedicalRecordsStaff,Nurse,StaffNurse")]
    public class PrescriptionManagementController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PrescriptionManagement
        public ActionResult Index(string patientid)
        {
            var prescriptions = db.Prescriptions.Include(p => p.Doctor).Include(p => p.Patient).Include(p => p.Drug).Where(p => p.PatientID == patientid);

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

            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
            return View(new Prescription { PatientID = patientid, DoctorID = User.Identity.GetUserId() });
        }

        // POST: PrescriptionManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptionNo,Dosage,LengthofTreatment,DateofPrescription,DoctorID,PatientID,DrugNo")] Prescription prescription)
        {
            //checks if model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    //checks lenght of treatment is integer > 0 and dosage is a integer > 0 , else error message in catch
                    if ((!(int.Parse(prescription.LengthofTreatment) > 0)) || (!(int.Parse(prescription.Dosage) > 0)))
                    {
                        ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
                        ViewBag.ErrorMessage = "Please enter valid values > 0";
                        return View(prescription);
                    }
                }
                catch
                {
                    ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
                    ViewBag.ErrorMessage = "Please enter valid values, please enter integer values";
                    return View(prescription);
                }


                //strange improvised fix for forign key error, if staff are not loaded from staffmanagement prescription creation becomes impossible ALSO
                db.ApplicationUsers.Load();

                Drug drug = db.Drugs.Find(prescription.DrugNo);

                prescription.PrescriptionCost = (drug.DrugCost * double.Parse(prescription.Dosage) * double.Parse(prescription.LengthofTreatment));

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

                            return RedirectToAction("Index", "PrescriptionManagement", new { prescription.PatientID });
                        }
                    }

                    //if no invoice found, create new invoice with temp invoice and newly created prescription added within
                    prescription.InvoiceNo = Invoice.InvoiceNo;
                    db.BillingInvoices.Add(Invoice);

                    db.Entry(prescription).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index", "PrescriptionManagement", new { prescription.PatientID });
                }
                catch
                {
                    //failed to create invocie message
                    ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
                    ViewBag.ErrorMessage = "Failded to create invoice";
                    return View(prescription);
                }


            }

            //invalid submit message
            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
            ViewBag.ErrorMessage = "submittion invalid, please enter all values";
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
            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
            return View(prescription);
        }

        // POST: PrescriptionManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrescriptionNo,Dosage,LengthofTreatment,DateofPrescription,DoctorID,PatientID,DrugNo")] Prescription prescription)
        {
            // check if model is valid
            if (ModelState.IsValid)
            {
                Drug drug = db.Drugs.Find(prescription.DrugNo);

                //chcks if values is integer > 0 else error message in cathc
                try
                {
                    if ((!(int.Parse(prescription.LengthofTreatment) > 0)) || (!(int.Parse(prescription.Dosage) > 0)))
                    {
                        ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
                        ViewBag.ErrorMessage = "Please enter valid values > 0";
                        return View(prescription);
                    }
                }
                catch
                {
                    ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
                    ViewBag.ErrorMessage = "Please enter valid values";
                    return View(prescription);
                }

                //calculates cost
                prescription.PrescriptionCost = (drug.DrugCost * double.Parse(prescription.Dosage) * double.Parse(prescription.LengthofTreatment));

                //temp invoice for createion and  temp old prescription to be initalised if previous prescription exists
                BillingInvoice Invoice = new BillingInvoice { PatientID = prescription.PatientID, PaymentRecived = false, TotalDue = prescription.PrescriptionCost };
                Prescription oldprescription = prescription;

                var billinginvoices = db.BillingInvoices.Include(i => i.Patient).Include(i => i.Prescriptions).Include(i => i.Treatments).Include(i => i.Payment).ToList();

                try
                {
                    //searches for unpaid invoice and look for old prescription matching prescription no
                    foreach (BillingInvoice invoice in billinginvoices)
                    {
                        if (invoice.PatientID == prescription.PatientID && invoice.PaymentRecived == false && invoice.PaymentNo == null)
                        {
                            Invoice = invoice;

                            foreach (Prescription p in Invoice.Prescriptions)
                            {
                                if (p.PrescriptionNo == prescription.PrescriptionNo)
                                {
                                    oldprescription = p;
                                }
                            }

                            prescription.InvoiceNo = Invoice.InvoiceNo;
                            Invoice.TotalDue = Invoice.TotalDue - oldprescription.PrescriptionCost + prescription.PrescriptionCost;

                            //forign key error fix when editing, stating primary key conflict instead of using state.modified (the origin of error, apparently)
                            db.Set<BillingInvoice>().AddOrUpdate(invoice);

                            db.Set<Prescription>().AddOrUpdate(prescription);

                            db.SaveChanges();

                            return RedirectToAction("Index", "PrescriptionManagement", new {prescription.PatientID });
                        }
                    }

                    //create new invoice
                    prescription.InvoiceNo = Invoice.InvoiceNo;
                    db.BillingInvoices.Add(Invoice);

                    db.Entry(prescription).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index", "PrescriptionManagement", new { prescription.PatientID });

                }
                catch
                {
                    //invoice failed error
                    ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
                    ViewBag.ErrorMessage = "Invoice failed to update";
                    return View(prescription);
                }
            }

            //ivnalid submit error
            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
            ViewBag.ErrorMessage = "submittion invalid, please enter all values";
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
            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugName");
            return View(prescription);
        }

        // POST: PrescriptionManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //temp variables used to find previous invoice 
            Prescription prescription = db.Prescriptions.Find(id);
            string patientid = prescription.PatientID;

            var billinginvoices = db.BillingInvoices.Include(i => i.Patient).Include(i => i.Prescriptions).Include(i => i.Treatments).Include(i => i.Payment).ToList();
            BillingInvoice Invoice = new BillingInvoice();

            try
            {
                //once found invoice makes changes to invoice then updates
                foreach (BillingInvoice invoice in billinginvoices)
                {
                    if (invoice.PatientID == prescription.PatientID && invoice.PaymentRecived == false && invoice.PaymentNo == null)
                    {
                        Invoice = invoice;

                        Invoice.TotalDue = Invoice.TotalDue - prescription.PrescriptionCost;
                        Invoice.Prescriptions.Remove(prescription);

                        db.Entry(invoice).State = EntityState.Modified;
                        db.SaveChanges();

                        db.Prescriptions.Remove(prescription);
                        db.SaveChanges();
                        return RedirectToAction("Index", "PrescriptionManagement", new { patientid });

                    }
                }

            }
            catch
            {
                return View("Error");
            }

            return RedirectToAction("Index", "PrescriptionManagement", new { patientid });
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
