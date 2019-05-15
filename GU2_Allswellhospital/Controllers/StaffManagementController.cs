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
    public class StaffManagementController : AccountController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public StaffManagementController() : base()
        {

        }

        public StaffManagementController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : base(userManager, signInManager)
        {

        }

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
            ViewBag.RoleNo = new SelectList(db.Roles, "Id", "Name");
            CreateStaffViewModel staffviewmodel = new CreateStaffViewModel();
            staffviewmodel.Roles = db.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name}).ToList();
            return View(staffviewmodel);
        }

        // POST: StaffManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create(string id, CreateStaffViewModel staffviewmodel)
        {
            if (ModelState.IsValid)
            {
                var Staff = new Staff { UserName = staffviewmodel.Email, Forename = staffviewmodel.Forename, Email = staffviewmodel.Email, City = staffviewmodel.City, DOB = staffviewmodel.DOB, PhoneNumber = staffviewmodel.Telnum, Surname = staffviewmodel.Surname, Town = staffviewmodel.Town, Street = staffviewmodel.Street };
                var result = await UserManager.CreateAsync(Staff, staffviewmodel.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(Staff.Id, staffviewmodel.Role.ToString());

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Error");
            }

            return RedirectToAction("Error");

            //ViewBag.TeamNo = new SelectList(db.Teams, "TeamNo", "TeamName", staff.TeamNo);
            //return View(staff);

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
