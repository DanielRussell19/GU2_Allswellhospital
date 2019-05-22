using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 9/05/2019

    /// <summary>
    /// Controller used to handle CRUD operations for MedicalRecords
    /// </summary>
    [Authorize(Roles ="Doctor,Consultant,MedicalRecordsStaff,Nurse,StaffNurse")]
    public class MedicalRecordsController : Controller
    {
        // GET: MedicalRecords
        public ActionResult MedicalRecordsIndex()
        {
            return View();
        }

        public ActionResult Patients()
        {
            return RedirectToAction("Index","PatientManagement","PatientManagement");
        }

        public ActionResult Admissions()
        {
            return RedirectToAction("Index", "AdmissionManagement", "AdmissionManagement");
        }

        public ActionResult Payments()
        {
            return RedirectToAction("Index", "PaymentManagement", "PaymentManagement");
        }
    }
}