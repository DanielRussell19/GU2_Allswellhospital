using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GU2_Allswellhospital.Models
{
    //Daniel Russell 04/05/2019

    //Used to define all objects used by the system such as drugs for proscriptions and ward for patients
 
    /// <summary>
    /// Definition of Object Drug
    /// </summary>
    public class Drug
    {
        //Attributes
        [Key]
        public string DrugNo { get; set; }

        [Required]
        public string DrugDetails { get; set; }

        [Required]
        public string DrugName { get; set; }

        [Required]
        public double DrugCost { get; set; }

        //Constructors
        public Drug()
        {

        }

        public Drug(Drug drug)
        {

        }
    }

    /// <summary>
    /// Definition of Object ward
    /// </summary>
    public class Ward
    {
        //Attributes
        [Key]
        public string WardNo { get; set; }

        [Required]
        public string WardName { get; set; }

        [Required]
        public int WardCapacity { get; set; }
    
        //Navigation Properties
        [ForeignKey("Team")]
        public string TeamNo { get; set; }
        public Team Team { get; set; }

        //constructors
        public Ward()
        {

        }

        public Ward(Ward ward)
        {

        }
    }

    /// <summary>
    /// Definition of Object team
    /// </summary>
    public class Team
    {
        //Attributes
        [Key]
        public string TeamNo { get; set; }

        [Required]
        public string TeamName { get; set; }

        //Navigational Properties
        public IList<Staff> Staffs {get;set;}

        [ForeignKey("Ward")]
        public string WardNo { get; set; }
        public Ward Ward { get; set; }

        //constructors
        public Team()
        {
            Staffs = new List<Staff>();
        }

        public Team(Team team)
        {

        }
    }

    /// <summary>
    /// Definition of Object Proscription
    /// </summary>
    public class Prescription
    {
        //Attributes
        [Key]
        public string PrescriptionNo { get; set; }

        [Required]
        public string Dosage { get; set; }

        [Required]
        public string LengthofTreatment { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime DateofPrescription { get; set; }

        [Required]
        public string IssuedByID { get; set; }

        //Navigational properties
        [ForeignKey("Patient")]
        public string PatientID { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Drug")]
        public string DrugNo { get; set; }
        public Drug Drug { get; set; }

        //Constructors
        public Prescription()
        {

        }

        public Prescription(Prescription prescription)
        {

        }
    }

    /// <summary>
    /// Defines Treatment
    /// </summary>
    public class Treatment
    {
        //attributes
        [Key]
        public string TreatmentNo { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime DateofTreatment { get; set; }

        [Required]
        public string TreatmentDetails { get; set; }

        [Required]
        public double TreatmentCost { get; set; }

        //navigational properties
        public IList<Prescription> prescriptions { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorID { set; get; }
        public Doctor Doctor { get; set; }

        [ForeignKey("Consultant")]
        public string ConsultantID { set; get; }
        public Consultant Consultant { get; set; }

        [ForeignKey("Patient")]
        public string PatientID { get; set; }
        public Patient Patient { get; set; }

        //constructors
        public Treatment()
        {
            prescriptions = new List<Prescription>();
        }

        public Treatment(Treatment treatment)
        {

        }
    }


    /// <summary>
    /// Definition of Object Admission
    /// </summary>
    public class Admission
    {
        //Attributes
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime DateAdmitted { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime DateDischarged { get; set; }

        public bool isConfirmed { get; set; }

        //navigational properties
        [ForeignKey("Patient")]
        public string PatientID { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Ward")]
        public string WardNo { get; set; }
        public Ward Ward { get; set; }

        //constructors
        public Admission()
        {

        }

        public Admission(Admission admission)
        {

        }
    }

    /// <summary>
    /// Definition of Object Payment
    /// </summary>
    public class Payment
    {
        //Attributes
        [Key]
        public string PaymentNo { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public string BillingAddress { get; set; }

        [Required]
        public string Forename { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string SecurityCode { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime ExpiryDate { get; set; }

        //Constructs
        public Payment()
        {

        }

        public Payment(Payment payment)
        {

        }
    }

    /// <summary>
    /// Definition of Object BillingInvoice
    /// </summary>
    public class BillingInvoice
    {
        //Attributes
        [Key]
        public string InvoiceNo { get; set; }

        [Required]
        public bool PaymentRecived { get; set; }

        [Required]
        public double TotalDue { get; set; }

        //Navigational Properties
        [ForeignKey("Patient")]
        public string PatientID { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Treatment")]
        public string TreatmentNo { get; set; }
        public Treatment Treatment { get; set; }

        [ForeignKey("Payment")]
        public string PaymentNo { get; set; }
        public Payment Payment { get; set; }

        //constructors
        public BillingInvoice()
        {

        }

        public BillingInvoice(BillingInvoice billingInvoice)
        {

        }
    }

}