using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GU2_Allswellhospital.Models
{
    //Daniel Russell 04/05/2019

    /// <summary>
    /// Defines how the database will be initalized and how it will be initally seeded
    /// </summary>
    public class DataBaseInitaliser : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        /// <summary>
        /// seeder method override, to seed inital users, roles and objects
        /// </summary>
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            //creates staff roles

            if (!context.Users.Any())
            {

                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (!roleManager.RoleExists("StaffAdmin"))
                {
                    roleManager.Create(new IdentityRole("StaffAdmin"));
                }

                if (!roleManager.RoleExists("MedicalRecordsStaff"))
                {
                    roleManager.Create(new IdentityRole("MedicalRecordsStaff"));
                }

                if (!roleManager.RoleExists("Nurse"))
                {
                    roleManager.Create(new IdentityRole("Nurse"));
                }

                if (!roleManager.RoleExists("StaffNurse"))
                {
                    roleManager.Create(new IdentityRole("StaffNurse"));
                }

                if (!roleManager.RoleExists("WardSister"))
                {
                    roleManager.Create(new IdentityRole("WardSister"));
                }

                if (!roleManager.RoleExists("Doctor"))
                {
                    roleManager.Create(new IdentityRole("Doctor"));
                }

                if (!roleManager.RoleExists("Consultant"))
                {
                    roleManager.Create(new IdentityRole("Consultant"));
                }

                context.SaveChanges();
            }

            //creates staff users

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            if (userManager.FindByName("staffadmin@test.com") == null)
            {
                var staffAdmin = new StaffAdmin
                {
                    UserName = "staff@test.com",
                    Email = "staff@test.com",
                    EmailConfirmed = true,
                    Forename = "sam",
                    Surname = "samson",
                    DOB = DateTime.Now,
                    Street = "Street",
                    City = "City",
                    Town = "Town",
                    PhoneNumber = "0005550000"
                };
                userManager.Create(staffAdmin, "password123");
                userManager.AddToRole(staffAdmin.Id, "StaffAdmin");
            }

            context.SaveChanges();

        }


    }
}