using MasterPiece.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterPiece.Controllers
{
    public class NotificationsController : Controller
    {
        private MasterPieceEntities db = new MasterPieceEntities();
        // GET: Notifications
        [HttpGet]
        public JsonResult GetLatest()
        {
            var notifications = db.Notifications
                .Where(n => n.IsRead == false)
                .OrderByDescending(n => n.Notification_Date)
                .Take(10)
                .ToList(); 

            var formattedNotifications = notifications.Select(n => new {
                n.Order_ID,
                Notification_Date = n.Notification_Date.HasValue ? n.Notification_Date.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null
            }).ToList();

            return Json(formattedNotifications, JsonRequestBehavior.AllowGet);

        }
    }
}