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

            if (userManager.FindByName("medicalrecordstaff@test.com") == null)
            {
                var medicalRecordsStaff = new MedicalRecordsStaff
                {
                    UserName = "medicalrecordstaff@test.com",
                    Email = "medicalrecordstaff@test.com",
                    EmailConfirmed = true,
                    Forename = "wright",
                    Surname = "mires",
                    DOB = DateTime.Now,
                    Street = "Street",
                    City = "City",
                    Town = "Town",
                    PhoneNumber = "0005550000"
                };
                userManager.Create(medicalRecordsStaff, "password123");
                userManager.AddToRole(medicalRecordsStaff.Id, "MedicalRecordsStaff");
            }

            context.SaveChanges();

            //object seeds

            //wards

            if (!context.Wards.Any())
            {
                context.Wards.Add(new Ward { WardName="Dentistry", WardCapacity=27});
                context.Wards.Add(new Ward { WardName = "Orthadontics", WardCapacity = 27});
                context.Wards.Add(new Ward { WardName = "Phisio", WardCapacity = 27});
                context.Wards.Add(new Ward { WardName = "dermatology", WardCapacity = 27});

                context.SaveChanges();
            }

            //teams

            if (!context.Teams.Any())
            {
                context.Teams.Add(new Team { TeamName = "Dentistry"});
                context.Teams.Add(new Team { TeamName = "Orthadontics"});

                context.SaveChanges();
            }

            //drugs

            if (!context.Drugs.Any())
            {
                context.Drugs.Add(new Drug { DrugName = "Dent", DrugDetails = "Heart medicine", DrugCost = 10.35 });
                context.Drugs.Add(new Drug { DrugName = "Ortha", DrugDetails = "Nose medicine", DrugCost = 5.49 });

                context.SaveChanges();
            }

            //patients

            if (!context.Patients.Any())
            {
                context.Patients.Add(new Patient { Email="me@me.com", Forename="Dan", Surname="Russ", Street="dolph", Town="towni", City="county", DOB=DateTime.Now, Occupation="Unemployed", Id = Guid.NewGuid().ToString(), UserName = "me@me.com"});

                context.SaveChanges();
            }

            //admissions

            if (!context.Admissions.Any())
            {
                context.Admissions.Add(new Admission { DateAdmitted = DateTime.Now, DateDischarged= DateTime.Now, isConfirmed = true });

                context.SaveChanges();
            }

            //treatment invoices

            if (!context.Invoices.Any())
            {
                context.Invoices.Add(new BillingInvoice { });

                context.SaveChanges();
            }

            //payments

            if (!context.Payments.Any())
            {
                context.Payments.Add(new Payment { });

                context.SaveChanges();
            }

            //prescriptions, not proscriptions :V

            if (!context.Prescriptions.Any())
            {
                context.Prescriptions.Add(new Prescription { });

                context.SaveChanges();
            }

            //Treatments

            if (!context.Treatments.Any())
            {
                context.Treatments.Add(new Treatment { });

                context.SaveChanges();
            }

        }


    }
}