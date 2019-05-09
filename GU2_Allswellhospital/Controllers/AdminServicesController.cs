using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Controllers
{
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