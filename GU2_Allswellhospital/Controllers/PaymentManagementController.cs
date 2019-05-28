using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GU2_Allswellhospital.Models;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 12/05/2019

    /// <summary>
    /// Controller used to handle CRUD operations for payments
    /// </summary>
    [Authorize(Roles = "Doctor,Consultant,MedicalRecordsStaff,Nurse,StaffNurse")]
    public class PaymentManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PaymentManagement
        public ActionResult Index()
        {
            return View(db.Payments.ToList());
        }

    }
}
