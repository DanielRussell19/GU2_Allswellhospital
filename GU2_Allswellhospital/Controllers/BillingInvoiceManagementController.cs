using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GU2_Allswellhospital.Models;
using Microsoft.AspNet.Identity;
using Stripe;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 13/05/2019

    /// <summary>
    /// Controller for CRUD operations with BillingInvoice
    /// </summary>
    [Authorize(Roles = "MedicalRecordsStaff")]
    public class BillingInvoiceManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BillingInvoiceManagement
        public ActionResult Index(string patientid)
        {
            var billingInvoices = db.BillingInvoices.Include(b => b.Patient).Include(b => b.Payment).Where(p => p.PatientID == patientid);

            ViewBag.patientid = patientid;

            return View(billingInvoices.ToList());
        }

        // GET: BillingInvoiceManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get full include of invoices and 
            var billingInvoices = db.BillingInvoices.Include(i => i.Treatments).Include(i => i.Prescriptions).Include(i => i.Patient).Include(i => i.Payment);
            BillingInvoice billingInvoice = new BillingInvoice();

            //searches for the one the matches id
            foreach (BillingInvoice invoice in billingInvoices)
            {
                if(invoice.InvoiceNo == id)
                {
                    billingInvoice = invoice;
                }
            }

            if (billingInvoice == null)
            {
                return HttpNotFound();
            }
            return View(billingInvoice);
        }

        // GET: BillingInvoiceManagement/makepayment
        public ActionResult MakePayment(string id,string patientid)
        {
            ViewBag.patientid = patientid;
            Patient patient = db.Patients.Find(patientid);
            BillingInvoice billingInvoice = db.BillingInvoices.Find(id);

            return View(new CreatePaymentViewModel { PatientId = patientid, InvoiceNo = id, Forename = patient.Forename, Surname = patient.Surname, BillingAddress = patient.Street + " " + patient.Town, InvoiceTotal = billingInvoice.TotalDue });
        }

        // POST: BillingInvoiceManagement/Makepayment
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakePayment([Bind(Include = "PaymentNo,PaymentMethod,BillingAddress,Forename,Surname,CardNumber,SecurityCode,ExpiryDate,PatientId,InvoiceNo,SelectedMethod,InvoiceTotal")] CreatePaymentViewModel payment, string stripeEmail, string stripeToken)
        {
            //checks if model state is valid
            if (ModelState.IsValid)
            {
                //if stripe is choosen then redirect to stripe payment
                if(payment.SelectedMethod == "Stripe")
                {
                    try
                    {
                         return RedirectToAction("StripePayment", "BillingInvoiceManagement", payment);
                    }
                    catch
                    {
                        ViewBag.ErrorMessage = "Unable to Redirect to stripe payment";
                        return View(payment);
                    }
                }
                else
                {
                    //handles non stripe payments that can be directly recorded into the database

                    //transfers details from paymentviewmodel to payment
                    Payment temppayment = new Payment { Forename = payment.Forename, Surname = payment.Surname, BillingAddress = payment.BillingAddress, PaymentMethod = payment.SelectedMethod };
                    BillingInvoice billingInvoice = db.BillingInvoices.Find(payment.InvoiceNo);

                    Patient patient = db.Patients.Find(billingInvoice.PatientID);

                    //email process
                    if (patient.Email != null)
                    {
                        EmailService emailService = new EmailService();
                        emailService.SendAsync(new IdentityMessage { Destination = patient.Email, Body = "Your Payment of £" + payment.InvoiceTotal + " for your treatments has been recived", Subject = "Confirmation of Payment" });
                    }

                    //transfers invoice amount to payment amount 
                    temppayment.PaymentAmount = billingInvoice.TotalDue;

                    //create payment
                    db.Payments.Add(temppayment);

                    db.SaveChanges();

                    //assosiated payment with invoice then updates
                    billingInvoice.PaymentRecived = true;
                    billingInvoice.PaymentNo = temppayment.PaymentNo;

                    db.Entry(billingInvoice).State = EntityState.Modified;

                    db.SaveChanges();
                }

                return RedirectToAction("Index", "BillingInvoiceManagement", new { payment.PatientId });
            }
            //invalid model message
            ViewBag.ErrorMessage = "Invalid submittion, please enter all values";
            return View(payment);
        }

        //get of stripe payment
        [HttpGet]
        public ActionResult StripePayment(CreatePaymentViewModel payment)
        {
            return View(payment);
        }

        //post of stripe payment
        [HttpPost]
        public ActionResult StripePayment([Bind(Include = "PaymentNo,PaymentMethod,BillingAddress,Forename,Surname,CardNumber,SecurityCode,ExpiryDate,PatientId,InvoiceNo,SelectedMethod,InvoiceTotal")] CreatePaymentViewModel payment, string stripeEmail, string stripeToken)
        {
            try
            {
                //attributes
                StripeConfiguration.SetApiKey("sk_test_fHaiXwbfFo3YUowus0cFNdOR00HHNl42Yw");
                var customers = new CustomerService();
                var charges = new ChargeService();

                //create customer
                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Description = "test purposes Charge",
                    SourceToken = stripeToken,
                });

                //creates charge, unable to correctly record charge as amount requires a long input which my entire project relises on double
                
                //most tiresome double to list of char to string to int64 casting ever, but succeeds in turing a double to a long compatable with stripe API
                //create list of char from invoice total
                List<char> chartotal = payment.InvoiceTotal.ToString("C2").ToList();

                //scans through char list removing all decimal points
                while (chartotal.Contains('.') || chartotal.Contains(',') || chartotal.Contains('£'))
                {
                    try
                    {
                        chartotal.Remove('.');
                        chartotal.Remove(',');
                        chartotal.Remove('£');
                    }
                    catch
                    {
                        continue;
                    }
                }

                //utalizes stringbuilder to build a string out of the list of char values
                string temptotal = null;
                var builder = new StringBuilder();

                foreach (char c in chartotal)
                {
                    builder.Append(c);
                }

                //final string product of the tiresome cast that now must be converted to long below
                temptotal = builder.ToString();
                //

                //create charge
                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = Int64.Parse(temptotal),
                    Description = "test purposes Charge",
                    Currency = "gbp",
                    CustomerId = customer.Id,
                    ReceiptEmail = customer.Email,
                });

                //if charge and customer creation successfull and can be found on stripe then payment is recorded in database
                if (customers.Get(customer.Id) != null && charges.Get(charge.Id) != null)
                {
                    //transfers details from payment view model and finds invoice
                    Payment temppayment = new Payment { Forename = payment.Forename, Surname = payment.Surname, BillingAddress = payment.BillingAddress, PaymentMethod = payment.SelectedMethod };
                    BillingInvoice billingInvoice = db.BillingInvoices.Find(payment.InvoiceNo);

                    Patient patient = db.Patients.Find(billingInvoice.PatientID);

                    //email process
                    if (patient.Email != null)
                    {
                        EmailService emailService = new EmailService();
                        emailService.SendAsync(new IdentityMessage { Destination = patient.Email, Body = "Your Payment of £" + payment.InvoiceTotal + " for your treatments has been recived", Subject = "Confirmation of Payment" });
                    }

                    //transfers total to payment
                    temppayment.PaymentAmount = billingInvoice.TotalDue;

                    //creates payment
                    db.Payments.Add(temppayment);

                    db.SaveChanges();

                    //assosiates payment with invoice then updates
                    billingInvoice.PaymentRecived = true;
                    billingInvoice.PaymentNo = temppayment.PaymentNo;

                    db.Entry(billingInvoice).State = EntityState.Modified;

                    db.SaveChanges();

                    return RedirectToAction("Index", "BillingInvoiceManagement", new { payment.PatientId });
                }
                else
                {
                    //stripe error message
                    ViewBag.ErrorMessage = "Unable to Process Stripe Payment";
                    return View(payment);
                }
            }
            catch
            {
                //stripe fault error message
                ViewBag.ErrorMessage = "Error encountered during stripe payment";
                return View(payment);
            }
        }

        //// GET: BillingInvoiceManagement/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BillingInvoice billingInvoice = db.BillingInvoices.Find(id);
        //    if (billingInvoice == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(billingInvoice);
        //}

        //// POST: BillingInvoiceManagement/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    var billingInvoices = db.BillingInvoices.Include(i => i.Prescriptions).Include(i => i.Treatments).Include(i => i.Payment);
        //    BillingInvoice invoice = null;
        //    string patientid = null;

        //    foreach (BillingInvoice billingInvoice in billingInvoices)
        //    {
        //        if(billingInvoice.InvoiceNo == id)
        //        {
        //            invoice = billingInvoice;
        //            patientid = invoice.PatientID;

        //            Payment payment = db.Payments.Find(invoice.PaymentNo);

        //            db.Entry(invoice).State = EntityState.Deleted;
        //            db.Entry(payment).State = EntityState.Deleted;
        //            break;    
        //        }
        //    }

        //    db.SaveChanges();

        //    return RedirectToAction("Index", "BillingInvoiceManagement", new { patientid });
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
