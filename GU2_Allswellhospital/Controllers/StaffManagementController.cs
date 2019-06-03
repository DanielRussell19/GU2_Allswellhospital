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
            //create new instance of modifystaffviewmodel to use for creation
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
            //checks model state
            if (ModelState.IsValid)
            {
                //checks if user is of valid working age, 16
                if (!(staffviewmodel.DOB.Year < DateTime.Now.Year - 16))
                {
                    ViewBag.RoleNo = new SelectList(db.Roles, "Id", "Name");
                    ViewBag.ErrorMessage = "Not legal age to work";
                    staffviewmodel.Roles = db.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();

                    return View(staffviewmodel);
                }

                //scans all instances of staff to check for pre existing staff by these details
                foreach(Staff staff in db.ApplicationUsers.ToList())
                {
                    if(staff.Email == staffviewmodel.Email || (staff.Forename == staffviewmodel.Forename && staff.Surname == staffviewmodel.Surname) || staff.PhoneNumber == staffviewmodel.Telnum)
                    {
                        ViewBag.RoleNo = new SelectList(db.Roles, "Id", "Name");
                        ViewBag.ErrorMessage = "Staff Member Already Exists By These Details";
                        staffviewmodel.Roles = db.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();

                        return View(staffviewmodel);
                    }
                }

                //create instance of staff
                var Staff = new Staff { UserName = staffviewmodel.Email, Forename = staffviewmodel.Forename, Email = staffviewmodel.Email, City = staffviewmodel.City, DOB = staffviewmodel.DOB, PhoneNumber = staffviewmodel.Telnum, Surname = staffviewmodel.Surname, Town = staffviewmodel.Town, Street = staffviewmodel.Street };

                //passes instance of staff to usermanager to create user, if success passed to index, if fail redirected to error
                var result = await UserManager.CreateAsync(Staff, staffviewmodel.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(Staff.Id, staffviewmodel.Role.ToString());

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Error");
            }

            //if model state invalid redirects to current view confirming model not valid error
            ViewBag.RoleNo = new SelectList(db.Roles, "Id", "Name");
            ViewBag.ErrorMessage = "Submission not valid, please fill all fields";
            staffviewmodel = new ModifyStaffViewModel();
            staffviewmodel.Roles = db.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();
            return View(staffviewmodel);
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

            //create new instance of modify staff view model for edit
            ViewBag.RoleNo = new SelectList(db.Roles, "Id", "Name");
            ModifyStaffViewModel staffviewmodel = new ModifyStaffViewModel();

            string oldRole = (await UserManager.GetRolesAsync(id)).Single();

            //intakes current values available about user to modify, minus password due to hashed display and confidential
            staffviewmodel.Roles = db.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name, Selected = r.Name == oldRole }).ToList();
            staffviewmodel.Forename = staff.Forename;
            staffviewmodel.Surname = staff.Surname;
            staffviewmodel.Street = staff.Street;
            staffviewmodel.Town = staff.Town;
            staffviewmodel.City = staff.City;
            staffviewmodel.DOB = staff.DOB;
            staffviewmodel.Email = staff.Email;
            staffviewmodel.Telnum = staff.PhoneNumber;
            staffviewmodel.Role = staff.Role;
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
            //checks if model is valid
            if (ModelState.IsValid)
            {
                //checks legal working age, 16 and above
                if (!(staffviewmodel.DOB.Year < DateTime.Now.Year - 16))
                {
                    ViewBag.RoleNo = new SelectList(db.Roles, "Id", "Name");
                    ViewBag.ErrorMessage = "Not legal age to work";
                    staffviewmodel.Roles = db.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();

                    return View(staffviewmodel);
                }

                //scans all instances of staff to check for pre existing staff by these details
                foreach (Staff s in db.ApplicationUsers.ToList())
                {
                    if(s.Id == staffviewmodel.tempid)
                    {
                        continue;
                    }

                    if (s.Email == staffviewmodel.Email || (s.Forename == staffviewmodel.Forename && s.Surname == staffviewmodel.Surname) || s.PhoneNumber == staffviewmodel.Telnum)
                    {
                        ViewBag.RoleNo = new SelectList(db.Roles, "Id", "Name");
                        ViewBag.ErrorMessage = "Staff Member Already Exists By These Details";
                        staffviewmodel.Roles = db.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();

                        return View(staffviewmodel);
                    }
                }

                //finds user and previous role
                var staff = UserManager.FindById(staffviewmodel.tempid);
                string oldRole = (await UserManager.GetRolesAsync(staffviewmodel.tempid)).Single();

                //updates details of staff
                staff.Forename = staffviewmodel.Forename;
                staff.Surname = staffviewmodel.Surname;
                staff.DOB = staffviewmodel.DOB;
                staff.Email = staffviewmodel.Email;
                staff.City = staffviewmodel.City;
                staff.PhoneNumber = staffviewmodel.Telnum;
                staff.UserName = staff.Email;
                staff.Town = staffviewmodel.Town;
                staff.Street = staffviewmodel.Street;

                //saves changes made
                await UserManager.UpdateAsync(staff);

                //removes from oldrole defined above, replaces with new role defined in staffviewmodel
                await UserManager.RemoveFromRoleAsync(staffviewmodel.tempid, oldRole);
                await UserManager.AddToRoleAsync(staffviewmodel.tempid, staffviewmodel.Role);

                //if a password is enter into the modifyviewmodel the password is then hashed, removes previous password and add the new one to user
                if(staffviewmodel.Password != null)
                {
                    IPasswordHasher passwordHasher = new PasswordHasher();

                    string hashedpassword = passwordHasher.HashPassword(staffviewmodel.Password);

                    UserManager.RemovePassword(staffviewmodel.tempid);
                    await UserManager.AddPasswordAsync(staffviewmodel.tempid, staffviewmodel.Password);

                    db.SaveChanges();
                }

                //overall savechanges if password is empty
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //if model is not valid returns viewmodel confirming error
            ViewBag.RoleNo = new SelectList(db.Roles, "Id", "Name");
            ViewBag.ErrorMessage = "Submission not valid, please fill all fields";
            staffviewmodel.Roles = db.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();

            return View(staffviewmodel);
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
