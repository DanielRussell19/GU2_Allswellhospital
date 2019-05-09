using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 9/05/2019

    public class AdminServicesController : Controller
    {
        // GET: AdminServices
        public ActionResult AdminServicesIndex()
        {
            return View();
        }

        public ActionResult StaffManagement()
        {
            return View();
        }

        public ActionResult WardManagement()
        {
            return View();
        }

        public ActionResult DrugManagement()
        {
            return View();
        }
    }
}