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

        //get listing of patients and returns view
        public ActionResult GetPatients()
        {
            return View(db.Patients.ToList());
        }

        //gets all patients under the care of a doctor and returns view as a pdf
        public ActionResult GetPatientsDoctors(string id)
        {
            //temp lists to find all instances of treatments with that patient
            List<Treatment> treatments = db.Treatments.Include(t => t.Patient).Include(t => t.Doctor).Where(t => t.PatientID == id).ToList();
            List<ApplicationUser> doctors = new List<ApplicationUser>();

            //if none are found return error message
            if(treatments.Count == 0)
            {
                Patient patient = db.Patients.Find(id);
                ViewBag.ErrorMessage = "Patient " + patient.Forename + " " + patient.Surname + " has no treatment";
                return View("GetPatients", db.Patients.ToList());
            }

            //looks through treatments and finds instances of doctors according to the doctorid found in treatment
            foreach(Treatment treatment in treatments)
            {
                doctors.Add(db.ApplicationUsers.Find(treatment.DoctorID));
            }

            //returns view as pdf
            return new ViewAsPdf(doctors)
            {
                FileName = "PatientDoctors.pdf"
            };
        }

        //gets listing of teams and returns view
        public ActionResult GetTeams()
        {
            return View(db.Teams.Include(t => t.Staffs).Include(t => t.Ward));
        }

        //looks for pateitns under the care of a team and returns a pdf of the view
        public ActionResult GetPatientsUnderTeam(string id)
        {
            Team team = db.Teams.Find(id);

            //temp listing used to find patients according to ward which the team is assigned to
            List<Admission> admissions = db.Admissions.Include(a => a.Patient).Where(a => a.WardNo == team.WardNo && a.isAdmitted == true).ToList();
            List<Patient> patients = new List<Patient>();

            //if none are found returns error message
            if (admissions.Count == 0)
            {
                ViewBag.ErrorMessage = "Team " + team.TeamName + " has not cared for any patients";
                return View("GetTeams", db.Teams.ToList());
            }

            //scans through admissions and adds found patients according to their patientid
            foreach (Admission admission in admissions)
            {
                patients.Add(db.Patients.Find(admission.PatientID));
            }

            //returns pdf of the view
            return new ViewAsPdf(patients)
            {
                FileName = "PatientsUnderSelectTeam"+team.TeamName+".pdf"
            };
        }
    }
}