using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterPiece.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminDashboard()
        {
            return View();
        }

        public ActionResult AddPatient()
        {
            return View();
        }

        public ActionResult AddPatientTests()
        {
            return View();
        }

        public ActionResult AddPatientPayment()
        {
            return View();
        }

        public ActionResult ManagePatient()
        {
            return View();
        }

        public ActionResult ManagePatientDetails()
        {
            return View();
        }

        public ActionResult TestResults()
        {
            return View();
        }

        public ActionResult TestResultsAdd()
        {
            return View();
        }

        public ActionResult InventoryManagement()
        {
            return View();
        }

        public ActionResult TestsDocumentation()
        {
            return View();
        }
    }
}