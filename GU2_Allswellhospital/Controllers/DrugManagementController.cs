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
    /// Controller used to handle CRUD operations for Drugs
    /// </summary>
    [Authorize(Roles = "StaffAdmin")]
    public class DrugManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DrugManagement
        public ActionResult Index()
        {
            return View(db.Drugs.ToList());
        }

        // GET: DrugManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drug drug = db.Drugs.Find(id);
            if (drug == null)
            {
                return HttpNotFound();
            }
            return View(drug);
        }

        // GET: DrugManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DrugManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DrugNo,DrugDetails,DrugName,DrugCost")] Drug drug)
        {
            //check if model is valid
            if (ModelState.IsValid)
            {
                //temp list to get any insatnce of drug that matches criteria
                List<Drug> drugs = db.Drugs.Where(d => d.DrugName == drug.DrugName).ToList();

                //if any object exists in drugs then it already exists
                if(drugs.Count > 0)
                {
                    ViewBag.ErrorMessage = "Drug already exists by this name";
                    return View(drug);
                }

                //checks drug cost
                if(!(drug.DrugCost > 0))
                {
                    ViewBag.ErrorMessage = "Please input a valid drug cost";
                    return View(drug);
                }

                //adds drug
                db.Drugs.Add(drug);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //invalid message display
            ViewBag.ErrorMessage =  "Submit invalid, model is invalid please fill all fields";
            return View(drug);
        }

        // GET: DrugManagement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drug drug = db.Drugs.Find(id);
            if (drug == null)
            {
                return HttpNotFound();
            }
            return View(drug);
        }

        // POST: DrugManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DrugNo,DrugDetails,DrugName,DrugCost")] Drug drug)
        {
            //checks if model is valid
            if (ModelState.IsValid)
            {
                //gets list of drugs that match criteria
                List<Drug> drugs = db.Drugs.Where(d => d.DrugName == drug.DrugName).ToList();

                //if any are present 
                if (drugs.Count > 0)
                {
                    //if it equals the drugno then save anyway
                    if(drugs.First().DrugNo == drug.DrugNo)
                    {
                        db.Set<Drug>().AddOrUpdate(drug);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    //else it already exsists
                    ViewBag.ErrorMessage = "Drug already exists by this name";
                    return View(drug);
                }

                //checks drug cost
                if (!(drug.DrugCost > 0))
                {
                    ViewBag.ErrorMessage = "Please input a valid drug cost";
                    return View(drug);
                }

                //add or update instacne of drugg
                db.Set<Drug>().AddOrUpdate(drug);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //invalid model errormessage 
            ViewBag.ErrorMessage = "Submit invalid, model is invalid please fill all fileds";
            return View(drug);
        }

        // GET: DrugManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drug drug = db.Drugs.Find(id);
            if (drug == null)
            {
                return HttpNotFound();
            }
            return View(drug);
        }

        // POST: DrugManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Drug drug = db.Drugs.Find(id);
            db.Drugs.Remove(drug);
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
