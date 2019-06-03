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

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 9/05/2019

    /// <summary>
    /// Controller used to handle CRUD operations for Ward
    /// </summary>
    [Authorize(Roles = "StaffAdmin")]
    public class WardManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WardManagement
        public ActionResult Index()
        {
            //initalises all wards with spaces taken, if a patient is admitted to that ward
            List<Patient> patients = db.Patients.Include(w => w.Ward).ToList();
            List<Ward> wards = db.Wards.ToList();

            //scans through each ward using a nested for each patient to find patient who are indeed admitted to that ward
            foreach (Ward w in wards)
            {

                foreach(Patient p in patients)
                {

                    if(p.WardNo == w.WardNo)
                    {
                        w.WardSpacesTaken = w.WardSpacesTaken + 1;
                    }

                }

            }

            return View(wards);
        }

        // GET: WardManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ward ward = db.Wards.Find(id);
            if (ward == null)
            {
                return HttpNotFound();
            }
            return View(ward);
        }

        // GET: WardManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WardManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WardNo,WardName,WardCapacity")] Ward ward)
        {
            //checks if model submitted is valid
            if (ModelState.IsValid)
            {
                if (!(ward.WardCapacity > 0))
                {
                    ViewBag.ErrorMessage = "Ward must have at least one space";
                    return View(ward);
                }

                //temp listing of wards
                List<Ward> wards = db.Wards.ToList();

                //checks if any occurance of this was already exists
                foreach (Ward w in wards)
                {
                    if (w.WardName == ward.WardName)
                    {
                        ViewBag.ErrorMessage = "Ward Already exsists, please choose different name";
                        return View(ward);
                    }
                }

                //if none already exists then added
                db.Wards.Add(ward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //invalid submittion message
            ViewBag.ErrorMessage = "Submit is invalid, please fill all fields";
            return View(ward);
        }

        // GET: WardManagement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ward ward = db.Wards.Find(id);
            if (ward == null)
            {
                return HttpNotFound();
            }
            return View(ward);
        }

        // POST: WardManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WardNo,WardName,WardCapacity")] Ward ward)
        {
            if (ModelState.IsValid)
            {

                if (!(ward.WardCapacity > 0))
                {
                    ViewBag.ErrorMessage = "Ward must have at least one space";
                    return View(ward);
                }

                //temp listing of wards
                List<Ward> wards = db.Wards.ToList();

                //checks if any occurance of this was already exists
                foreach (Ward w in wards)
                {
                    if(w.WardNo == ward.WardNo)
                    {
                        continue;
                    }

                    if (w.WardName == ward.WardName)
                    {
                        ViewBag.ErrorMessage = "Ward Already exsists, please choose different name";
                        return View(ward);
                    }
                }

                db.Set<Ward>().AddOrUpdate(ward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = "Submittion failed, model invalid";
            return View(ward);
        }

        // GET: WardManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ward ward = db.Wards.Find(id);
            if (ward == null)
            {
                return HttpNotFound();
            }
            return View(ward);
        }

        // POST: WardManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Ward ward = db.Wards.Find(id);

            List<Admission> admissions = db.Admissions.Include(a => a.Ward).Where(a => a.isAdmitted == true).ToList();

            //checks is any admission still exists to this ward
            if(admissions.Count > 0)
            {
                ViewBag.ErrorMessage = "Patients are still assigned to this ward";
                return View(ward);
            }

            db.Wards.Remove(ward);
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
