using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 9/05/2019

    public class MedicalRecordsController : Controller
    {
        // GET: MedicalRecords
        public ActionResult MedicalRecordsIndex()
        {
            return View();
        }
    }
}