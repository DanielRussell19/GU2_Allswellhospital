using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 28/05/2019

    //sendgrid, twillo, leaflet, stripe, github, videoapi

    /// <summary>
    /// Controller used to handle quieries and enable the abillity to convert it to pdf
    /// </summary>
    public class AnalyticsManagementController : Controller
    {
        // GET: AnalyticsManagement
        public ActionResult Index()
        {
            return View();
        }
    }
}