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
    /// Controller used to handle CRUD operations for patients along with additional methods
    /// </summary>
    [Authorize(Roles = "Doctor,Consultant,MedicalRecordsStaff,Nurse,StaffNurse")]
    public class PatientManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PatientManagement
        public ActionResult Index()
        {
            return View(db.Patients.Include(p => p.Ward).ToList());
        }

        // GET: PatientManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: PatientManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Forename,Surname,Street,Town,City,Email,TelNum,DOB,Occupation,NextofKinForename,NextofKinSurname,NextofKinStreet,NextofKinTown,NextofKinCity,NextofkinTelNum")] Patient patient)
        {
            patient.Id = Guid.NewGuid().ToString();

            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(patient);
        }

        // GET: PatientManagement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: PatientManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Forename,Surname,Street,Town,City,Email,UserName,TelNum,DOB,Occupation,NextofKinForename,NextofKinSurname,NextofKinStreet,NextofKinTown,NextofKinCity,NextofkinTelNum")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: PatientManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: PatientManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //temp list of invoices used to match with patient if any are unpaid
            List<BillingInvoice> billingInvoices = db.BillingInvoices.Include(i => i.Patient).Where(i => i.PaymentRecived == false).ToList();
            Patient patient = db.Patients.Find(id);

            //if count of invoice > 0 the patient can not be deleted
            if (billingInvoices.Count >0)
            {
                ViewBag.ErrorMessage = "Patient has unpaid invoices";
                return View(patient);
            }

            db.Patients.Remove(patient);
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
