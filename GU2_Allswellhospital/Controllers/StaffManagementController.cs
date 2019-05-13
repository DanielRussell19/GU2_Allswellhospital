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
    //Daniel Russell 9/05/2019

    /// <summary>
    /// controller used to handle CRUD operations for staff
    /// </summary>
    public class StaffManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StaffManagement
        public ActionResult Index()
        {
            var applicationUsers = db.ApplicationUsers.Include(s => s.Team);
            return View(applicationUsers.ToList());
        }

        // GET: StaffManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.ApplicationUsers.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // GET: StaffManagement/Create
        public ActionResult Create()
        {
            ViewBag.TeamNo = new SelectList(db.Teams, "TeamNo", "TeamName");
            return View();
        }

        // POST: StaffManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Forename,Surname,Street,Town,City,DOB,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,TeamNo")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationUsers.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeamNo = new SelectList(db.Teams, "TeamNo", "TeamName", staff.TeamNo);
            return View(staff);
        }

        // GET: StaffManagement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.ApplicationUsers.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeamNo = new SelectList(db.Teams, "TeamNo", "TeamName", staff.TeamNo);
            return View(staff);
        }

        // POST: StaffManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Forename,Surname,Street,Town,City,DOB,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,TeamNo")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeamNo = new SelectList(db.Teams, "TeamNo", "TeamName", staff.TeamNo);
            return View(staff);
        }

        [HttpGet]
        public ActionResult ChangeRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeRole(ChangeRoleViewModel roleChange)
        {
            return View();
        }

        // GET: StaffManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.ApplicationUsers.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: StaffManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Staff staff = db.ApplicationUsers.Find(id);
            db.ApplicationUsers.Remove(staff);
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
