using GU2_Allswellhospital.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 05/05/2019

    /// <summary>
    /// controller used to handle staff home
    /// </summary>
    public class StaffHomeController : Controller
    {

        // GET: StaffHome
        public ActionResult StaffHome()
        {
            return View();
        }

        //redirects to admin services
        public ActionResult AdminServicesRedirect()
        {
            return RedirectToAction("AdminServicesIndex","AdminServices","AdminServices");
        }

        //redirects to medical records
        public ActionResult MedicalRecordsRedirect()
        {
            return RedirectToAction("MedicalRecordsIndex","MedicalRecords","MedicalRecords");
        }

        //redirects to team management
        public ActionResult TeamManagementRedirect()
        {
            return RedirectToAction("Index", "TeamManagment", "TeamManagment");
        }
    }
}