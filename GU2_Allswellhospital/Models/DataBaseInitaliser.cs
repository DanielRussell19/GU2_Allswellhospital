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
            
            if (userManager.FindByName("Simpson@Manager.com") == null)
            {
                var staffAdmin = new StaffAdmin
                {
                    UserName = "Simpson@Manager.com",
                    Email = "Simpson@Manager.com",
                    EmailConfirmed = true,
                    Forename = "Sam",
                    Surname = "Simpson",
                    DOB = DateTime.Now,
                    Street = "Street",
                    City = "City",
                    Town = "Town",
                    PhoneNumber = "0005550000"
                };
                userManager.Create(staffAdmin, "password123");
                userManager.AddToRole(staffAdmin.Id, "StaffAdmin");
            }

            if (userManager.FindByName("Carl@MedRec.com") == null)
            {
                var medicalRecordsStaff = new MedicalRecordsStaff
                {
                    UserName = "Carl@MedRec.com",
                    Email = "Carl@MedRec.com",
                    EmailConfirmed = true,
                    Forename = "Fred",
                    Surname = "Carlson",
                    DOB = DateTime.Now,
                    Street = "Street",
                    City = "City",
                    Town = "Town",
                    PhoneNumber = "0005550000"
                };
                userManager.Create(medicalRecordsStaff, "password123");
                userManager.AddToRole(medicalRecordsStaff.Id, "MedicalRecordsStaff");
            }

            if (userManager.FindByName("Fray@WardSister.com") == null)
            {
                var wardSister = new WardSister
                {
                    UserName = "Fray@WardSister.com",
                    Email = "Fray@WardSister.com",
                    EmailConfirmed = true,
                    Forename = "Lilith",
                    Surname = "Fray",
                    DOB = DateTime.Now,
                    Street = "Street",
                    City = "City",
                    Town = "Town",
                    PhoneNumber = "0005550000"
                };
                userManager.Create(wardSister, "password123");
                userManager.AddToRole(wardSister.Id, "WardSister");
            }

            if (userManager.FindByName("Daniels@Nurse.com") == null)
            {
                var Nurse = new Nurse
                {
                    UserName = "Daniels@Nurse.com",
                    Email = "Daniels@Nurse.com",
                    EmailConfirmed = true,
                    Forename = "Liam",
                    Surname = "Daniels",
                    DOB = DateTime.Now,
                    Street = "Street",
                    City = "City",
                    Town = "Town",
                    PhoneNumber = "0005550000"
                };
                userManager.Create(Nurse, "password123");
                userManager.AddToRole(Nurse.Id, "Nurse");
            }

            if (userManager.FindByName("Ulala@SNurse.com") == null)
            {
                var StaffNurse = new StaffNurse
                {
                    UserName = "Ulala@SNurse.com",
                    Email = "Ulala@SNurse.com",
                    EmailConfirmed = true,
                    Forename = "Tay",
                    Surname = "Lewis",
                    DOB = DateTime.Now,
                    Street = "Street",
                    City = "City",
                    Town = "Town",
                    PhoneNumber = "0005550000"
                };
                userManager.Create(StaffNurse, "password123");
                userManager.AddToRole(StaffNurse.Id, "StaffNurse");
            }

            if (userManager.FindByName("Var@JDoctor.com") == null)
            {
                var JuniorDoctor = new Doctor
                {
                    UserName = "Var@JDoctor.com",
                    Email = "Var@JDoctor.com",
                    EmailConfirmed = true,
                    Forename = "Vargas",
                    Surname = "Taylor",
                    DOB = DateTime.Now,
                    Street = "Street",
                    City = "City",
                    Town = "Town",
                    PhoneNumber = "0005550000",
                    Grade = 'X',
                    Specialism="Dentistry"
                };
                userManager.Create(JuniorDoctor, "password123");
                userManager.AddToRole(JuniorDoctor.Id, "Doctor");
            }

            if (userManager.FindByName("Tracer@Doctor.com") == null)
            {
                var Doctor = new Doctor
                {
                    UserName = "Tracer@Doctor.com",
                    Email = "Tracer@Doctor.com",
                    EmailConfirmed = true,
                    Forename = "Brie",
                    Surname = "Tracer",
                    DOB = DateTime.Now,
                    Street = "Street",
                    City = "City",
                    Town = "Town",
                    PhoneNumber = "0005550000",
                    Grade='A',
                    Specialism="Gastrology"
                };
                userManager.Create(Doctor, "password123");
                userManager.AddToRole(Doctor.Id, "Doctor");
            }

            if (userManager.FindByName("Paul@Consultant.com") == null)
            {
                var consultant = new Consultant
                {
                    UserName = "Paul@Consultant.com",
                    Email = "Paul@Consultant.com",
                    EmailConfirmed = true,
                    Forename = "Fred",
                    Surname = "Carlson",
                    DOB = DateTime.Now,
                    Street = "Street",
                    City = "City",
                    Town = "Town",
                    PhoneNumber = "0005550000",
                    Specialism="Phiso",
                    Grade='A'
                };
                userManager.Create(consultant, "password123");
                userManager.AddToRole(consultant.Id, "Consultant");
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