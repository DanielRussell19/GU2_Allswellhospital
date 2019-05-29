using GU2_Allswellhospital.Models;
using RotativaHQ.MVC5;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GU2_Allswellhospital.Controllers
{
    //Daniel Russell 28/05/2019

    /// <summary>
    /// Controller used to handle quieries and enable the abillity to convert it to pdf
    /// </summary>
    [Authorize(Roles = "StaffAdmin")]
    public class AnalyticsManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AnalyticsManagement
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPatients()
        {
            return View(db.Patients.ToList());
        }

        public ActionResult GetPatientsDoctors(string id)
        {
            List<Treatment> treatments = db.Treatments.Include(t => t.Patient).Include(t => t.Doctor).Where(t => t.PatientID == id).ToList();
            List<ApplicationUser> doctors = new List<ApplicationUser>();

            if(doctors.Count == 0)
            {
                Patient patient = db.Patients.Find(id);
                ViewBag.ErrorMessage = "Patient " + patient.Forename + " " + patient.Surname + " has no treatment";
                return View("GetPatients", db.Patients.ToList());
            }

            foreach(Treatment treatment in treatments)
            {
                doctors.Add(db.ApplicationUsers.Find(treatment.DoctorID));
            }

            return new ViewAsPdf(doctors)
            {
                FileName = "PatientDoctors.pdf"
            };
        }

        public ActionResult GetTeams()
        {
            return View(db.Teams.Include(t => t.Staffs).Include(t => t.Ward));
        }

        public ActionResult GetPatientsUnderTeam(string id)
        {
            Team team = db.Teams.Find(id);

            List<Admission> admissions = db.Admissions.Include(a => a.Patient).Where(a => a.WardNo == team.WardNo && a.isAdmitted == true).ToList();
            List<Patient> patients = new List<Patient>();

            if (patients.Count == 0)
            {
                ViewBag.ErrorMessage = "Team " + team.TeamName + " has not cared for any patients";
                return View("GetTeams", db.Teams.ToList());
            }

            foreach (Admission admission in admissions)
            {
                patients.Add(db.Patients.Find(admission.PatientID));
            }

            return new ViewAsPdf(patients)
            {
                FileName = "PatientsUnderSelectTeam"+team.TeamName+".pdf"
            };
        }
    }
}