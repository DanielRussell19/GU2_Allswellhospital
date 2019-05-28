using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 9/05/2019

    /// <summary>
    /// Homepage of Admin Controls
    /// </summary>
    [Authorize(Roles = "StaffAdmin")]
    public class AdminServicesController : Controller
    {
        // GET: AdminServices
        public ActionResult AdminServicesIndex()
        {
            return View();
        }

        // GET: Redirects to StaffManagement
        public ActionResult StaffManagement()
        {
            return RedirectToAction("Index","StaffManagement","StaffManagement");
        }

        // GET: Redirects to WardManagement
        public ActionResult WardManagement()
        {
            return RedirectToAction("Index", "WardManagement", "WardManagement");
        }

        // GET: Redirects to DrugManagement
        public ActionResult DrugManagement()
        {
            return RedirectToAction("Index", "DrugManagement", "DrugManagement");
        }

        // GET: Redirects to Analytics
        public ActionResult Analytics()
        {
            return View();
        }
    }
}