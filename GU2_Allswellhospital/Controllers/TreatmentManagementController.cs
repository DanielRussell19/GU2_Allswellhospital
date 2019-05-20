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
        public ActionResult Details(string id, string patientid)
        {
            ViewBag.patientid = patientid;

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
            return View();
        }

        // POST: TreatmentManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TreatmentDetails,TreatmentCost,DoctorID")] Treatment treatment, string patientid)
        {
            ViewBag.patientid = patientid;

            treatment.TreatmentNo = Guid.NewGuid().ToString();
            treatment.PatientID = patientid;
            treatment.DateofTreatment = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Treatments.Add(treatment);
                db.SaveChanges();
                return RedirectToAction("Index","TreatmentManagement",patientid);
            }

            ViewBag.DoctorID = new SelectList(db.ApplicationUsers, "Id", "Forename", treatment.DoctorID);
            return View(treatment);
        }

        // GET: TreatmentManagement/Edit/5
        public ActionResult Edit(string id)
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
                db.Entry(treatment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            db.Treatments.Remove(treatment);
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
