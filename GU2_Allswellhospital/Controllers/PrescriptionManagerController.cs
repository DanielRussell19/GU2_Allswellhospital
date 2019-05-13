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
    /// Contorller used to handle CRUD operations for prescription
    /// </summary>
    public class PrescriptionManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PrescriptionManager
        public ActionResult Index()
        {
            var prescriptions = db.Prescriptions.Include(p => p.Drug).Include(p => p.Patient).Include(p => p.Treatment);
            return View(prescriptions.ToList());
        }

        // GET: PrescriptionManager/Details/5
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

        // GET: PrescriptionManager/Create
        public ActionResult Create()
        {
            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugDetails");
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename");
            ViewBag.TreatmentNo = new SelectList(db.Treatments, "TreatmentNo", "TreatmentDetails");
            return View();
        }

        // POST: PrescriptionManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptionNo,Dosage,LengthofTreatment,DateofPrescription,IssuedByID,PatientID,TreatmentNo,DrugNo")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Prescriptions.Add(prescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugDetails", prescription.DrugNo);
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", prescription.PatientID);
            ViewBag.TreatmentNo = new SelectList(db.Treatments, "TreatmentNo", "TreatmentDetails", prescription.TreatmentNo);
            return View(prescription);
        }

        // GET: PrescriptionManager/Edit/5
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
            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugDetails", prescription.DrugNo);
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", prescription.PatientID);
            ViewBag.TreatmentNo = new SelectList(db.Treatments, "TreatmentNo", "TreatmentDetails", prescription.TreatmentNo);
            return View(prescription);
        }

        // POST: PrescriptionManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrescriptionNo,Dosage,LengthofTreatment,DateofPrescription,IssuedByID,PatientID,TreatmentNo,DrugNo")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugDetails", prescription.DrugNo);
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", prescription.PatientID);
            ViewBag.TreatmentNo = new SelectList(db.Treatments, "TreatmentNo", "TreatmentDetails", prescription.TreatmentNo);
            return View(prescription);
        }

        // GET: PrescriptionManager/Delete/5
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

        // POST: PrescriptionManager/Delete/5
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
