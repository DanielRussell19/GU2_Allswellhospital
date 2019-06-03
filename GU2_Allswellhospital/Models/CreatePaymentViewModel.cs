using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Models
{
    /// <summary>
    /// Definition of Object Payment
    /// </summary>
    public class CreatePaymentViewModel
    {
        //Attributes

        //temp value to patient id
        public string PatientId { get; set; }

        //temp value for invoice no
        public string InvoiceNo { get; set; }

        //predefined payments methods
        [Required]
        public List<SelectListItem> PaymentMethods = new List<SelectListItem>()
        {
            new SelectListItem(){Text = "Stripe", Value = "Stripe"},
            new SelectListItem(){Text = "Cheque", Value = "Cheque"},
            new SelectListItem(){Text = "Cash", Value = "Cash"},
            new SelectListItem(){Text = "Pre-paid", Value = "Pre-paid"}
        };

        //method user has selected
        public string SelectedMethod { get; set; }

        //total invoice has calculated
        public double InvoiceTotal { get; set; }

        //billing address of invoice
        [Required]
        public string BillingAddress { get; set; }

        //firstname of patient
        [Required]
        public string Forename { get; set; }

        //surname of patient
        [Required]
        public string Surname { get; set; }

    }
}