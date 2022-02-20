namespace GU2_Allswellhospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admissions",
                c => new
                    {
                        AdmissionNo = c.String(nullable: false, maxLength: 128),
                        DateAdmitted = c.DateTime(nullable: false),
                        DateDischarged = c.DateTime(),
                        isAdmitted = c.Boolean(nullable: false),
                        PatientID = c.String(maxLength: 128),
                        WardNo = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AdmissionNo)
                .ForeignKey("dbo.Patients", t => t.PatientID)
                .ForeignKey("dbo.Wards", t => t.WardNo)
                .Index(t => t.PatientID)
                .Index(t => t.WardNo);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Forename = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        Town = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        TelNum = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        Occupation = c.String(nullable: false),
                        NextofKinForename = c.String(),
                        NextofKinSurname = c.String(),
                        NextofKinStreet = c.String(),
                        NextofKinTown = c.String(),
                        NextofKinCity = c.String(),
                        NextofkinTelNum = c.String(),
                        WardNo = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Wards", t => t.WardNo)
                .Index(t => t.WardNo);
            
            CreateTable(
                "dbo.BillingInvoices",
                c => new
                    {
                        InvoiceNo = c.String(nullable: false, maxLength: 128),
                        PaymentRecived = c.Boolean(nullable: false),
                        TotalDue = c.Double(nullable: false),
                        PatientID = c.String(maxLength: 128),
                        PaymentNo = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InvoiceNo)
                .ForeignKey("dbo.Patients", t => t.PatientID)
                .ForeignKey("dbo.Payments", t => t.PaymentNo)
                .Index(t => t.PatientID)
                .Index(t => t.PaymentNo);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentNo = c.String(nullable: false, maxLength: 128),
                        PaymentMethod = c.String(nullable: false),
                        PaymentAmount = c.Double(nullable: false),
                        BillingAddress = c.String(nullable: false),
                        Forename = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentNo);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        PrescriptionNo = c.String(nullable: false, maxLength: 128),
                        Dosage = c.String(nullable: false),
                        LengthofTreatment = c.String(nullable: false),
                        PrescriptionCost = c.Double(nullable: false),
                        DateofPrescription = c.DateTime(nullable: false),
                        DoctorID = c.String(maxLength: 128),
                        InvoiceNo = c.String(maxLength: 128),
                        PatientID = c.String(maxLength: 128),
                        DrugNo = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PrescriptionNo)
                .ForeignKey("dbo.BillingInvoices", t => t.InvoiceNo)
                .ForeignKey("dbo.AspNetUsers", t => t.DoctorID)
                .ForeignKey("dbo.Drugs", t => t.DrugNo)
                .ForeignKey("dbo.Patients", t => t.PatientID)
                .Index(t => t.DoctorID)
                .Index(t => t.InvoiceNo)
                .Index(t => t.PatientID)
                .Index(t => t.DrugNo);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Forename = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        Town = c.String(nullable: false),
                        City = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        TeamNo = c.String(maxLength: 128),
                        Specialism = c.String(),
                        WardNo = c.String(maxLength: 128),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamNo)
                .ForeignKey("dbo.Wards", t => t.WardNo)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.TeamNo)
                .Index(t => t.WardNo);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamNo = c.String(nullable: false, maxLength: 128),
                        TeamName = c.String(nullable: false),
                        WardNo = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TeamNo)
                .ForeignKey("dbo.Wards", t => t.WardNo)
                .Index(t => t.WardNo);
            
            CreateTable(
                "dbo.Wards",
                c => new
                    {
                        WardNo = c.String(nullable: false, maxLength: 128),
                        WardName = c.String(nullable: false),
                        WardCapacity = c.Int(nullable: false),
                        WardSpacesTaken = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WardNo);
            
            CreateTable(
                "dbo.Treatments",
                c => new
                    {
                        TreatmentNo = c.String(nullable: false, maxLength: 128),
                        DateofTreatment = c.DateTime(nullable: false),
                        TreatmentDetails = c.String(nullable: false),
                        TreatmentCost = c.Double(nullable: false),
                        DoctorID = c.String(maxLength: 128),
                        PatientID = c.String(maxLength: 128),
                        InvoiceNo = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TreatmentNo)
                .ForeignKey("dbo.BillingInvoices", t => t.InvoiceNo)
                .ForeignKey("dbo.AspNetUsers", t => t.DoctorID)
                .ForeignKey("dbo.Patients", t => t.PatientID)
                .Index(t => t.DoctorID)
                .Index(t => t.PatientID)
                .Index(t => t.InvoiceNo);
            
            CreateTable(
                "dbo.Drugs",
                c => new
                    {
                        DrugNo = c.String(nullable: false, maxLength: 128),
                        DrugDetails = c.String(nullable: false),
                        DrugName = c.String(nullable: false),
                        DrugCost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.DrugNo);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Admissions", "WardNo", "dbo.Wards");
            DropForeignKey("dbo.Admissions", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.Patients", "WardNo", "dbo.Wards");
            DropForeignKey("dbo.Prescriptions", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.Prescriptions", "DrugNo", "dbo.Drugs");
            DropForeignKey("dbo.Prescriptions", "DoctorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Treatments", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.Treatments", "DoctorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Treatments", "InvoiceNo", "dbo.BillingInvoices");
            DropForeignKey("dbo.Teams", "WardNo", "dbo.Wards");
            DropForeignKey("dbo.AspNetUsers", "WardNo", "dbo.Wards");
            DropForeignKey("dbo.AspNetUsers", "TeamNo", "dbo.Teams");
            DropForeignKey("dbo.Prescriptions", "InvoiceNo", "dbo.BillingInvoices");
            DropForeignKey("dbo.BillingInvoices", "PaymentNo", "dbo.Payments");
            DropForeignKey("dbo.BillingInvoices", "PatientID", "dbo.Patients");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Treatments", new[] { "InvoiceNo" });
            DropIndex("dbo.Treatments", new[] { "PatientID" });
            DropIndex("dbo.Treatments", new[] { "DoctorID" });
            DropIndex("dbo.Teams", new[] { "WardNo" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "WardNo" });
            DropIndex("dbo.AspNetUsers", new[] { "TeamNo" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Prescriptions", new[] { "DrugNo" });
            DropIndex("dbo.Prescriptions", new[] { "PatientID" });
            DropIndex("dbo.Prescriptions", new[] { "InvoiceNo" });
            DropIndex("dbo.Prescriptions", new[] { "DoctorID" });
            DropIndex("dbo.BillingInvoices", new[] { "PaymentNo" });
            DropIndex("dbo.BillingInvoices", new[] { "PatientID" });
            DropIndex("dbo.Patients", new[] { "WardNo" });
            DropIndex("dbo.Admissions", new[] { "WardNo" });
            DropIndex("dbo.Admissions", new[] { "PatientID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Drugs");
            DropTable("dbo.Treatments");
            DropTable("dbo.Wards");
            DropTable("dbo.Teams");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.Payments");
            DropTable("dbo.BillingInvoices");
            DropTable("dbo.Patients");
            DropTable("dbo.Admissions");
        }
    }
}
