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
    public class PrescriptionManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PrescriptionManagement
        public ActionResult Index()
        {
            var prescriptions = db.Prescriptions.Include(p => p.Drug).Include(p => p.Patient);
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
        public ActionResult Create()
        {
            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugDetails");
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename");
            return View();
        }

        // POST: PrescriptionManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptionNo,Dosage,LengthofTreatment,DateofPrescription,IssuedByID,PatientID,DrugNo")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Prescriptions.Add(prescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugDetails", prescription.DrugNo);
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", prescription.PatientID);
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
            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugDetails", prescription.DrugNo);
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", prescription.PatientID);
            return View(prescription);
        }

        // POST: PrescriptionManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrescriptionNo,Dosage,LengthofTreatment,DateofPrescription,IssuedByID,PatientID,DrugNo")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DrugNo = new SelectList(db.Drugs, "DrugNo", "DrugDetails", prescription.DrugNo);
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", prescription.PatientID);
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
