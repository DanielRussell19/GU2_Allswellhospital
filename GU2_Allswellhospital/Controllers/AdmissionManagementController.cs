﻿using System;
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
    //Daniel Russell 12/05/2019

    /// <summary>
    /// Controller used to handle CRUD operations for Admissions along with additional methods
    /// </summary>
    [Authorize(Roles = "MedicalRecordsStaff")]
    public class AdmissionManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdmissionManagement
        public ActionResult Index()
        {
            var admissions = db.Admissions.Include(a => a.Patient).Include(a => a.Ward);

            return View(admissions.ToList());
        }

        // GET: AdmissionManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admission admission = db.Admissions.Find(id);
            if (admission == null)
            {
                return HttpNotFound();
            }
            return View(admission);
        }

        // GET: AdmissionManagement/Create
        public ActionResult Create()
        {
            ViewBag.PatientID = new SelectList(db.Patients.Where(p => p.WardNo == null), "Id", "Forename");
            ViewBag.WardNo = new SelectList(db.Wards.Where(w => w.WardSpacesTaken < w.WardCapacity), "WardNo", "WardName");
            return View();
        }

        // POST: AdmissionManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdmissionNo,DateAdmitted,DateDischarged,isConfirmed,PatientID,WardNo")] Admission admission)
        {
            //initaliation
            admission.DateDischarged = null;
            admission.AdmissionNo = Guid.NewGuid().ToString();

            //checks model state valid
            if (ModelState.IsValid)
            {
                //checks if date is more than the present date, else error message returned
                if (admission.DateAdmitted > DateTime.Now)
                {

                    Ward ward = db.Wards.Find(admission.WardNo);

                    Patient patient = db.Patients.Find(admission.PatientID);

                    //checks capacity of ward, else error message stateing it is full
                    if (ward.WardSpacesTaken >= ward.WardCapacity)
                    {
                        ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", admission.PatientID);
                        ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName", admission.WardNo);
                        ViewBag.ErrorMessage = "Ward is full";
                        return View(admission);
                    }
                    else
                    {
                        //ward.WardSpacesTaken = ward.WardSpacesTaken + 1;
                        patient.WardNo = admission.WardNo;

                        db.Admissions.Add(admission);
                        db.Entry(ward).State = EntityState.Modified;
                        db.Entry(patient).State = EntityState.Modified;
                        db.SaveChanges();

                        //try catch for sms and email
                        try
                        {
                            //sms process
                            if (patient.TelNum != null)
                            {
                                SmsService smsService = new SmsService();
                                smsService.SendAsync(new IdentityMessage { Destination = patient.TelNum, Body = "you've got an appointment in " + admission.Ward.WardName + "at " + admission.DateAdmitted.ToString(), Subject = "SMSTest" });
                            }

                            //email process
                            if (patient.Email != null)
                            {
                                EmailService emailService = new EmailService();
                                emailService.SendAsync(new IdentityMessage { Destination = patient.Email, Body = "you've got an appointment in " + admission.Ward.WardName + "at " + admission.DateAdmitted.ToString(), Subject = "EmailTest" });
                            }
                        }
                        catch
                        {
                            //notifications failed
                            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", admission.PatientID);
                            ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName", admission.WardNo);
                            ViewBag.ErrorMessage = "Sms notification and or email notification failed";
                        }

                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    //error message for bad date
                    ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", admission.PatientID);
                    ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName", admission.WardNo);
                    ViewBag.ErrorMessage = "Date invalid, Please choose a later date";
                    return View(admission);
                }

            }

            //error message for invalid model
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", admission.PatientID);
            ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName", admission.WardNo);
            ViewBag.ErrorMessage = "Submit is Invalid, please fill all fields";
            return View(admission);
        }

        //confirms admission
        public ActionResult ConfirmAdmission(string id)
        {
            Admission admission = db.Admissions.Find(id);
            admission.isAdmitted = true;

            db.Entry(admission).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //discharges admission
        public ActionResult DischargePatient(string id)
        {
            Admission admission = db.Admissions.Find(id);

            //temp collection of invoices that matches patient id
            List<BillingInvoice> invoices = db.BillingInvoices.Include(i => i.Patient).Where(i => i.PatientID == admission.PatientID && i.PaymentRecived == false).ToList();

            //if any invoices are found to match patient id that are unpaid, display error message of unpaid treatments
            if ((invoices.Count > 0) == true)
            {
                ViewBag.ErrorMessage = "Patient has unpaid treatments";
                return View("Index", db.Admissions.Include(a => a.Patient).Include(a => a.Ward).ToList());
            }

            admission.DateDischarged = DateTime.Now;
            admission.isAdmitted = false;

            Ward ward = db.Wards.Find(admission.WardNo);
            //ward.WardSpacesTaken = ward.WardSpacesTaken - 1;

            //removes all assosiations to confirm discharge
            Patient patient = db.Patients.Find(admission.PatientID);
            patient.WardNo = null;

            db.Entry(ward).State = EntityState.Modified;
            db.Entry(admission).State = EntityState.Modified;
            db.Entry(patient).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AdmissionManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admission admission = db.Admissions.Find(id);
            if (admission == null)
            {
                return HttpNotFound();
            }
            return View(admission);
        }

        // POST: AdmissionManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //temp listing of invoice used to find unpaid invoices that match patatent id
            Admission admission = db.Admissions.Find(id);
            List<BillingInvoice> invoices = db.BillingInvoices.Include(i => i.Patient).Where(i => i.PatientID == admission.PatientID && i.PaymentRecived == false).ToList();

            //if none exist then admission is removeed else error message
            if ((invoices.Count >0) == false)
            {
                //if admission is already null in id's then remove anyway
                if (admission.WardNo == null || admission.PatientID == null)
                {
                    db.Admissions.Remove(admission);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                //else manually remove assosiations update and remove
                Patient patient = db.Patients.Find(admission.PatientID);

                Ward ward = db.Wards.Find(admission.WardNo);

                if (patient.WardNo != null)
                {
                    patient.WardNo = null;
                    //ward.WardSpacesTaken = ward.WardSpacesTaken - 1;

                    db.Entry(patient).State = EntityState.Modified;
                    db.Entry(ward).State = EntityState.Modified;
                }

                db.Admissions.Remove(admission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                //unpaid error message
                ViewBag.ErrorMessage = "Patient has unpaid treatments";
                return View("Index", db.Admissions.Include(a => a.Patient).Include(a => a.Ward).ToList());
            }
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
