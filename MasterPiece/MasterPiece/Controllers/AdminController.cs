using MasterPiece.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterPiece.Controllers
{
    public class AdminController : Controller
    {
        private MasterPieceEntities db = new MasterPieceEntities();

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

        public ActionResult Appointment()
        {
            var appointments = from a in db.Appointments
                               select new AppointmentViewModel
                               {
                                   ID = a.ID,
                                   Full_Name = a.Full_Name,
                                   Gender = a.Gender,
                                   Date_Of_Birth = a.Date_Of_Birth,
                                   Email_Address = a.Email_Address,
                                   Phone_Number = a.Phone_Number,
                                   Home_Address = a.Home_Address,
                                   Date_Of_Appo = a.Date_Of_Appo,
                                   Total_price = a.Total_price,
                                   Amount_paid = a.Amount_paid,
                                   Billing_ID = a.Billing_ID,
                                   Status = a.Status,

                                   // Query to get the test names for each appointment
                                   TestNames = (from at in db.Appointments_Tests
                                                join t in db.Tests on at.Test_ID equals t.Test_ID
                                                where at.Appointment_ID == a.ID
                                                select t.Test_Name).ToList()
                               };

            return View(appointments.ToList());
        }

        public ActionResult InventoryManagement()
        {
            return View();
        }

        public ActionResult TestsDocumentation()
        {
            return View();
        }

        public ActionResult TestDocumentationADD()
        {
            return View();
        }








    }
}