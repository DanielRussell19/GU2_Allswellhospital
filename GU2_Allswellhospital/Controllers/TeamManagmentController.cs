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
    /// controller used to handle CRUD operations for Team
    /// </summary>
    public class TeamManagmentController : AccountController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public TeamManagmentController() : base()
        {

        }

        public TeamManagmentController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : base(userManager, signInManager)
        {

        }

        // GET: TeamManagment
        public ActionResult Index()
        {
            var teams = db.Teams.Include(t => t.Ward);
            return View(teams.ToList());
        }

        // GET: TeamManagment/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // GET: TeamManagment/Create
        public ActionResult Create()
        {
            ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName");
            return View();
        }

        // POST: TeamManagment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeamNo,TeamName,WardNo")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName", team.WardNo);
            return View(team);
        }

        // GET: TeamManagment/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName", team.WardNo);
            return View(team);
        }

        // POST: TeamManagment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeamNo,TeamName,WardNo")] Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WardNo = new SelectList(db.Wards, "WardNo", "WardName", team.WardNo);
            return View(team);
        }

        // GET: TeamManagment/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: TeamManagment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Team team = db.Teams.Find(id);

            var applicationUsers = db.ApplicationUsers.Include(s => s.Team).Include(s => s.Roles).Where(s => s.TeamNo == id).ToList();

            foreach(Staff staff in applicationUsers)
            {
                staff.TeamNo = null;

                db.Entry(staff).State = EntityState.Modified;
            }

            db.SaveChanges();

            db.Teams.Remove(team);
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

        
        public ActionResult AssignStaffListing(string id)
        {
            var applicationUsers = db.ApplicationUsers.Include(s => s.Team).Include(s => s.Roles).Where(s => s.TeamNo == null);

            ViewBag.TeamNo = id;

            return View(applicationUsers.ToList());
        }

        
        public ActionResult AssignStaff(string StaffId, string teamNo)
        {

            if (StaffId == null || teamNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.ApplicationUsers.Find(StaffId);
            if (staff == null || teamNo == null)
            {
                return HttpNotFound();
            }

            staff.TeamNo = teamNo;
            //await UserManager.UpdateAsync(staff);

            db.Entry(staff).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        
        public ActionResult UnAssignStaffListing(string id)
        {
            var applicationUsers = db.ApplicationUsers.Include(s => s.Team).Include(s => s.Roles).Where(s => s.TeamNo == id);

            ViewBag.TeamNo = id;

            return View(applicationUsers.ToList());
        }

        
        public ActionResult UnAssignStaff(string StaffId, string teamNo)
        {
            if (StaffId == null || teamNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.ApplicationUsers.Find(StaffId);
            if (staff == null || teamNo == null)
            {
                return HttpNotFound();
            }

            staff.TeamNo = null;
            //await UserManager.UpdateAsync(staff);

            db.Entry(staff).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
