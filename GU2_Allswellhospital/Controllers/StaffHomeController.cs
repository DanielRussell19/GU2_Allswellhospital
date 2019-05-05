using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 05/05/2019

    public class StaffHomeController : Controller
    {
        // GET: StaffHome
        public ActionResult StaffHome()
        {
            return View();
        }

        public ActionResult AdminServices()
        {
            return View();
        }

        public ActionResult MedicalRecords()
        {
            return View();
        }

        public ActionResult TeamManagement()
        {
            return View();
        }
    }
}