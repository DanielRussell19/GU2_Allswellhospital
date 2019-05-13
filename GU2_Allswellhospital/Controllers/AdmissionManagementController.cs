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
    /// Controller used to handle CRUD operations for Admissions along with additional methods
    /// </summary>
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
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename");
            ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName");
            return View();
        }

        // POST: AdmissionManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdmissionNo,DateAdmitted,DateDischarged,isConfirmed,PatientID,WardNo")] Admission admission)
        {
            if (ModelState.IsValid)
            {
                db.Admissions.Add(admission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", admission.PatientID);
            ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName", admission.WardNo);
            return View(admission);
        }

        // GET: AdmissionManagement/Edit/5
        public ActionResult Edit(string id)
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
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", admission.PatientID);
            ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName", admission.WardNo);
            return View(admission);
        }

        // POST: AdmissionManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdmissionNo,DateAdmitted,DateDischarged,isConfirmed,PatientID,WardNo")] Admission admission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "Id", "Forename", admission.PatientID);
            ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName", admission.WardNo);
            return View(admission);
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
            Admission admission = db.Admissions.Find(id);
            db.Admissions.Remove(admission);
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
