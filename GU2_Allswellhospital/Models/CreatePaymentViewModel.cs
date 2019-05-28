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
        public string PatientId { get; set; }
        public string InvoiceNo { get; set; }

        [Required]
        public List<SelectListItem> PaymentMethods = new List<SelectListItem>()
        {
            new SelectListItem(){Text = "Stripe", Value = "Stripe"},
            new SelectListItem(){Text = "Cheque", Value = "Cheque"},
            new SelectListItem(){Text = "Cash", Value = "Cash"},
            new SelectListItem(){Text = "Pre-paid", Value = "Pre-paid"}
        };

        public string SelectedMethod { get; set; }

        public double InvoiceTotal { get; set; }

        [Required]
        public string BillingAddress { get; set; }

        [Required]
        public string Forename { get; set; }

        [Required]
        public string Surname { get; set; }

    }
}