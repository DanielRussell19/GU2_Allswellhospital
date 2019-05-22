using System;
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
    //Daniel Russell 9/05/2019

    /// <summary>
    /// controller used to handle CRUD operations for staff
    /// </summary>
    [Authorize(Roles = "StaffAdmin")]
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
            var applicationUsers = db.ApplicationUsers.Include(s => s.Team).Include(s => s.Roles);
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
            ModifyStaffViewModel staffviewmodel = new ModifyStaffViewModel();
            staffviewmodel.Roles = db.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name}).ToList();
            return View(staffviewmodel);
        }

        // POST: StaffManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create(string id, ModifyStaffViewModel staffviewmodel)
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
        }

        // GET: StaffManagement/Edit/5
        public async System.Threading.Tasks.Task<ActionResult> Edit(string id)
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

            ViewBag.RoleNo = new SelectList(db.Roles, "Id", "Name");
            ModifyStaffViewModel staffviewmodel = new ModifyStaffViewModel();

            string oldRole = (await UserManager.GetRolesAsync(id)).Single();

            staffviewmodel.Roles = db.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name, Selected = r.Name == oldRole }).ToList();
            staffviewmodel.Forename = staff.Forename;
            staffviewmodel.Surname = staff.Surname;
            staffviewmodel.Street = staff.Street;
            staffviewmodel.Town = staff.Town;
            staffviewmodel.City = staff.City;
            staffviewmodel.DOB = staff.DOB;
            staffviewmodel.Email = staff.Email;
            staffviewmodel.Telnum = staff.PhoneNumber;
            staffviewmodel.Role = staff.Roles.First().ToString();
            staffviewmodel.tempid = staff.Id;

            return View(staffviewmodel);
        }

        // POST: StaffManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Edit(ModifyStaffViewModel staffviewmodel)
        {
            if (ModelState.IsValid)
            {
                var staff = UserManager.FindById(staffviewmodel.tempid);
                string oldRole = (await UserManager.GetRolesAsync(staffviewmodel.tempid)).Single();

                await UserManager.UpdateAsync(staff);

                await UserManager.RemoveFromRoleAsync(staffviewmodel.tempid, oldRole);
                await UserManager.AddToRoleAsync(staffviewmodel.tempid, staffviewmodel.Role);

                if(staffviewmodel.Password != null)
                {
                    IPasswordHasher passwordHasher = new PasswordHasher();

                    string hashedpassword = passwordHasher.HashPassword(staffviewmodel.Password);

                    UserManager.RemovePassword(staffviewmodel.tempid);
                    await UserManager.AddPasswordAsync(staffviewmodel.tempid, staffviewmodel.Password);

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View("Error");

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
