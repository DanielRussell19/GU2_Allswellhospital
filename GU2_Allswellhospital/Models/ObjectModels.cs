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

        //navigational properties
        public IList<Prescription> Prescriptions { get; set; }

        //Constructors
        public Drug()
        {
            Prescriptions = new List<Prescription>();
            DrugNo = Guid.NewGuid().ToString();
            DrugDetails = "N/a";
            DrugName = "N/a";
            DrugCost = 0.00;
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

        public int WardSpacesTaken { get; set; }

        //constructors
        public Ward()
        {
            WardNo = Guid.NewGuid().ToString();
            WardName = "N/a";
            WardCapacity = 0;
            WardSpacesTaken = WardCapacity;
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
        [ForeignKey("Ward")]
        public string WardNo { get; set; }
        public Ward Ward { get; set; }

        public IList<Staff> Staffs { get; set; }

        //constructors
        public Team()
        {
            Staffs = new List<Staff>();
            TeamNo = Guid.NewGuid().ToString();
            TeamName = "N/a";
        }

        public Team(Team team)
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

        [ForeignKey("Doctor")]
        public string DoctorID { set; get; }
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("Patient")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("BillingInvoice")]
        public string InvoiceNo { get; set; }
        public virtual BillingInvoice BillingInvoice { get; set; }

        //constructors
        public Treatment()
        {
            TreatmentNo = Guid.NewGuid().ToString();
            TreatmentCost = 0.00;
            TreatmentDetails = "N/a";
            DateofTreatment = DateTime.Now;
        }

        public Treatment(Treatment treatment)
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

        public double PrescriptionCost { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime DateofPrescription { get; set; }

        //Navigational properties
        [ForeignKey("Doctor")]
        public string DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("BillingInvoice")]
        public string InvoiceNo { get; set; }
        public virtual BillingInvoice BillingInvoice { get; set; }

        [ForeignKey("Patient")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("Drug")]
        public string DrugNo { get; set; }
        public virtual Drug Drug { get; set; }

        //Constructors
        public Prescription()
        {
            PrescriptionNo = Guid.NewGuid().ToString();
            Dosage = "N/a";
            LengthofTreatment = "N/a";
            DateofPrescription = DateTime.Now;
        }

        public Prescription(Prescription prescription)
        {

        }
    }

    /// <summary>
    /// Definition of Object Admission
    /// </summary>
    public class Admission
    {
        //Attributes
        [Key]
        public string AdmissionNo { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime DateAdmitted { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: yyyy-MM-dd}")]
        public DateTime? DateDischarged { get; set; }

        public bool isAdmitted { get; set; }

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
            AdmissionNo = Guid.NewGuid().ToString();
            DateAdmitted = DateTime.Now;
            //DateDischarged = DateTime.Now;
            isAdmitted = false;
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
            PaymentNo = Guid.NewGuid().ToString();
            PaymentMethod = "N/a";
            BillingAddress = "N/a";
            Forename = "N/a";
            Surname = "N/a";
            CardNumber = "N/a";
            SecurityCode = "N/a";
            ExpiryDate = DateTime.Now;
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

        public IList<Treatment> Treatments { get; set; }
        public IList<Prescription> Prescriptions { get; set; }

        [ForeignKey("Payment")]
        public string PaymentNo { get; set; }
        public Payment Payment { get; set; }

        //constructors
        public BillingInvoice()
        {
            Treatments = new List<Treatment>();
            Prescriptions = new List<Prescription>();
            InvoiceNo = Guid.NewGuid().ToString();
            PaymentRecived = false;
            TotalDue = 0.00;
        }

        public BillingInvoice(BillingInvoice billingInvoice)
        {

        }
    }

}