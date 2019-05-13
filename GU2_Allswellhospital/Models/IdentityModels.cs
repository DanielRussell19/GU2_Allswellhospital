using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GU2_Allswellhospital.Models
{
    //Daniel Russell 04/05/2019

    //Used to define all idenity models used to represent the user

    /// <summary>
    /// Base Class for user Staff and Patient, inherits idenityuser from the identityframework
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        //attributes
        [Required]
        public string Forename { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        //constructor
        public ApplicationUser() : base()
        {
            Forename = "N/a";
            Surname = "N/a";
            Street = "N/a";
            Town = "N/a";
            City = "N/a";
            DOB = DateTime.Now;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    /// <summary>
    /// Describes an abstract Staff User
    /// </summary>
    public class Staff : ApplicationUser
    {

        //navigational properties
        [ForeignKey("Team")]
        public string TeamNo { get; set; }
        public Team Team { get; set; }

    }

    /// <summary>
    /// Describes a StaffAdmin
    /// </summary>
    public class StaffAdmin : Staff
    {
        
        //constructors
        public StaffAdmin() : base()
        {
         
        }

        public StaffAdmin(StaffAdmin staffAdmin)
        {

        }
    }

    /// <summary>
    /// Describes a MedicalRecordsStaff
    /// </summary>
    public class MedicalRecordsStaff : Staff
    {
        //constructors
        public MedicalRecordsStaff() : base()
        {

        }

        public MedicalRecordsStaff(MedicalRecordsStaff medicalRecordsStaff)
        {

        }
    }

    /// <summary>
    /// Describes a wardsister
    /// </summary>
    public class WardSister : Staff
    {
        ////navigational properties
        [ForeignKey("Ward")]
        public string WardNo { get; set; }
        public Ward Ward { get; set; }

        //constructors
        public WardSister() : base()
        {

        }

        public WardSister(WardSister wardSister)
        {

        }
    }

    /// <summary>
    /// describes a Doctor
    /// </summary>
    public class Doctor : Staff
    {
        //attributes
        public char Grade { get; set; }

        [Required]
        public string Specialism { get; set; }

        //Navigational Properties
        IList<Treatment>Treatments { get; set; }
        IList<Prescription> Prescriptions { get; set; }

        //contructors
        public Doctor() : base()
        {
            Treatments = new List<Treatment>();
            Prescriptions = new List<Prescription>();
        }

        public Doctor(Doctor doctor)
        {

        }
    }

    /// <summary>
    /// describes a consultant
    /// </summary>
    public class Consultant : Doctor
    {
        //constructors
        public Consultant() : base()
        {

        }

        public Consultant(Consultant consultant)
        {

        }
    }

    /// <summary>
    /// describes a nurse
    /// </summary>
    public class Nurse : Staff
    {

        //constructors
        public Nurse() : base()
        {

        }

        public Nurse(Nurse nurse)
        {

        }
    }

    /// <summary>
    /// describes a staff nurse
    /// </summary>
    public class StaffNurse : Nurse
    {
        //constructors
        public StaffNurse() : base()
        {

        }

        public StaffNurse(StaffNurse staffNurse)
        {

        }
    }

    /// <summary>
    /// Describes a Patient Object, Patient has and SHOULD have no access to the system
    /// </summary>
    public class Patient : ApplicationUser
    {
        //attruibutes
        public IList<string> Conditions { get; set; }

        public IList<string> Allergies { get; set; }

        [Required]
        public string Occupation { get; set; }

        public string NextofKinForename { get; set; }

        public string NextofKinSurname { get; set; }

        public string NextofKinStreet { get; set; }
        
        public string NextofKinTown { get; set; }

        public string NextofKinCity { get; set; }

        public string NextofkinTelNum { get; set; }

        //Navigational properties
        IList<Treatment> Treatments { get; set; }

        //constructors
        public Patient() : base()
        {
            Treatments = new List<Treatment>();
            Conditions = new List<string>();
            Allergies = new List<string>();
            Occupation = "N/a";
            NextofKinForename = "N/a";
            NextofKinSurname = "N/a";
            NextofKinStreet = "N/a";
            NextofKinTown = "N/a";
            NextofKinCity = "N/a";
            NextofkinTelNum = "N/a";
        }

        public Patient(Patient patient)
        {

        }

    }

}