using MasterPiece.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var tests = db.Tests.ToList();
            return View(tests);
        }

        public ActionResult TestsDocumentationssssssssssss()
        {
            return View();
        }

        public ActionResult TestDocumentationADD()
        {
            return View();
        }



        ////////////////////////////////////////////Emloyee Page ///////////////////////////////////
      
        public ActionResult Employees()
        {
            var employees = db.Lab_Tech.ToList();
            return View(employees);
        }
        [HttpPost]
        public ActionResult CreateEmployee(Lab_Tech employee)
        {
            if (ModelState.IsValid) 
            {
                db.Lab_Tech.Add(employee);
                db.SaveChanges();

                // Redirect to the list of employees (or wherever you want)
                return RedirectToAction("Employees");
            }

            return View(employee);
        }

        [HttpPost]
        public ActionResult EditEmployee(Lab_Tech employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Employees");
            }
            return View(employee);
        }

        [HttpPost]
        public ActionResult DeleteEmployee(int Tech_ID)
        {
            var employee = db.Lab_Tech.Find(Tech_ID);
            if (employee != null)
            {
                db.Lab_Tech.Remove(employee);
                db.SaveChanges();
            }
            return RedirectToAction("Employees");
        }


        //////////////////////////////////////////////Profile Page/////////////////////////////////////////


        public ActionResult Profile(int id)
        {
            var employee = db.Lab_Tech.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(Lab_Tech updatedEmployee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(updatedEmployee).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile", new { id = updatedEmployee.Tech_ID });
            }
            return View("EmployeeProfile", updatedEmployee);
        }


        ////////////////////////////////////////////////////Packages ////////////////////////////////////////////////

        public ActionResult Packages()
        {
            var packages = db.Packages.Include(p => p.Package_Tests.Select(pt => pt.Test)).ToList();
            ViewBag.TestsList= db.Tests.ToList();
            return View(packages);
        }

        // This action gets the selected tests for a specific package via Ajax
        public JsonResult GetPackageDetails(int id)
        {
            var package = db.Packages.Include(p => p.Package_Tests.Select(pt => pt.Test))
                                      .FirstOrDefault(p => p.Package_ID == id);
            var selectedTests = package.Package_Tests.Select(pt => new {
                pt.Test.Test_Name,
                pt.Test.Price
            }).ToList();

            return Json(new { selectedTests = selectedTests }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPackage(int id)
        {
            // Fetch the package details
            var package = db.Packages.Include(p => p.Package_Tests.Select(t => t.Test)).FirstOrDefault(p => p.Package_ID == id);

            // Fetch the list of all tests
            var tests = db.Tests.Select(t => new { t.Test_ID, t.Test_Name, t.Price }).ToList();

            // Prepare the selected tests to pass to the view
            var selectedTests = package.Package_Tests.Select(pt => new {
                pt.Test.Test_Name,
                pt.Test.Price
            }).ToList();

            ViewBag.TestsList = tests;
            ViewBag.SelectedTests = selectedTests;

            return View(package);
        }


        ///////////////////////////////////////////////////////          FeedBack          ///////////////////////////////////////////////////////////

        public ActionResult FeedBacks()
        {
            var feed = db.Feedbacks.ToList();
            return View(feed);
        }
        public ActionResult ApproveFeedback(int id)
        {
            var feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }

            feedback.Status = "Approved";
            db.SaveChanges();

            return RedirectToAction("FeedBacks");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.Status = "Pending"; // Default status when feedback is created
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Profile", "User");
            }

            // If there's an issue with the model state, return the same view with validation errors
            return View(feedback);
        }

        [HttpPost]
        public ActionResult DeleteFeedback(int Feedback_ID)
        {
            // Find the feedback by ID
            var feedback = db.Feedbacks.Find(Feedback_ID);

            // Check if feedback exists
            if (feedback == null)
            {
                return HttpNotFound();
            }

            // Remove the feedback from the database
            db.Feedbacks.Remove(feedback);
            db.SaveChanges();

            // Redirect to the feedback list page
            return RedirectToAction("FeedBacks");
        }





    }
}