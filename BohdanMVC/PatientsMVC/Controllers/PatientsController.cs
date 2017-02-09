using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatientsMVC.Models;

namespace PatientsMVC.Controllers
{
    public class PatientsController : Controller
    {
        PatientsContext PatientsCntx = new PatientsContext();
        // GET: Patient
        public ActionResult Index()
        {
            //get all patients from the db
            List<Patients> patients = PatientsCntx.Patients.ToList();
            //pass patients into view
            return View(patients);
        }
        
        // GET: One patient
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                //Get one patient from the database by their id
                Patients patient = PatientsCntx.Patients.Single(pnt => pnt.Id == id);
                //pass this one employee into view
                return View(patient);
            }
            else
            {
                //if no id was provided in the url go to the index action
                return RedirectToAction("Index");
            }
        }
    }
}