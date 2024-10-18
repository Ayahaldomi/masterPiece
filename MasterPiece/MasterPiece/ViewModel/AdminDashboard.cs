using MasterPiece.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterPiece.ViewModel
{
    public class AdminDashboard
    {
        public int Total_Patients { get; set; }
        public int Todays_Patients { get; set; }
        public int Todays_Appointments { get; set; }
        public decimal? Monthly_Earnings { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public IEnumerable<Test_Order> Payments_History { get; set; }
        public IEnumerable<Lab_Tech> Doctor_List { get; set; }
        public IEnumerable<Lab_Tech> LabTech_List { get; set; }
    }
}