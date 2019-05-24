using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace GU2_Allswellhospital.Models
{
    //Daniel Russell 04/05/2019

    /// <summary>
    /// Defines the context of the database
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

            public IDbSet<Drug> Drugs { get; set; }

            public IDbSet<Patient> Patients { get; set; }

            public IDbSet<Ward> Wards { get; set; }

            public IDbSet<Team> Teams { get; set; }

            public IDbSet<Prescription> Prescriptions { get; set; }

            public IDbSet<Treatment> Treatments { get; set; }

            public IDbSet<Admission> Admissions { get; set; }

            public IDbSet<Payment> Payments { get; set; }

            public IDbSet<BillingInvoice> BillingInvoices { get; set; }

            public IDbSet<Staff> ApplicationUsers { get; set; }

            public ApplicationDbContext() : base("DBConnection", throwIfV1Schema: false)
            {
            Database.SetInitializer(new DataBaseInitaliser());
            }

            public static ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }

    }
}