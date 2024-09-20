using MasterPiece.Models;
using MasterPiece.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterPiece.Controllers
{
    public class HomeController : Controller
    {
        private MasterPieceEntities db = new MasterPieceEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Service()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ServiceDetails()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        /////////////////////////////////////////////   Appointment   ///////////////////////////////////////////
        
        public ActionResult Appointment()
        {
            ViewBag.Message = "Your contact page.";
            var tests = db.Tests.ToList();
            return View(tests);
        }

        [HttpPost]
        public ActionResult CreateAppointment(AppointmentPOST appointment)
        {
            // Save appointment information
            var app = new Appointment
            {
                Full_Name = appointment.Full_Name,
                Gender = appointment.Gender,
                Date_Of_Birth = appointment.Date_Of_Birth,
                Email_Address = appointment.Email_Address,
                Phone_Number = appointment.Phone_Number,
                Home_Address = appointment.Home_Address,
                Date_Of_Appo = appointment.Date_Of_Appo,
                Total_price = appointment.Total_price,
                Amount_paid = appointment.Amount_paid,
                Billing_ID = 2656,
                Status = "Pending" // Example status
            };
            db.Appointments.Add(app);
            db.SaveChanges();

            // Save selected tests
            foreach (var selectedTest in appointment.SelectedTests)
            {
                var appointmentTest = new Appointments_Tests
                {
                    Appointment_ID = app.ID, // Use the saved appointment ID
                    Test_ID = selectedTest.Test_ID
                };

                db.Appointments_Tests.Add(appointmentTest);
            }

            db.SaveChanges();

            return View(appointment);
        }

        public ActionResult EmployeePortal()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}