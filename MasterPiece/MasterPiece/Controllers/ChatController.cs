﻿using MasterPiece.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterPiece.Controllers
{
    public class ChatController : Controller
    {
        private MasterPieceEntities _context = new MasterPieceEntities();
        // GET: Chat
        // View chat room between a patient and lab tech
        public ActionResult Chat(int chatRoomId)
        {
            var chatRoom = _context.ChatRooms.Find(chatRoomId);
            if (chatRoom == null)
            {
                return HttpNotFound("Chat room not found.");
            }

            var messages = _context.ChatMessages
                .Where(m => m.ChatRoom_ID == chatRoomId)
                .OrderBy(m => m.SentAt)
                .ToList();

            // Set ViewBag data for use in the view
            ViewBag.ChatRoomId = chatRoomId;
            ViewBag.PatientId = chatRoom.Patient_ID;
            ViewBag.LabTechId = chatRoom.LabTech_ID;

            // Get the patient's payment status
            var patient = _context.Patients.Find(chatRoom.Patient_ID);
            ViewBag.PaymentStatus = patient?.PaymentStatus;

            return View(messages);
        }

        public ActionResult Chat2(int chatRoomId)
        {
            var chatRoom = _context.ChatRooms.Find(chatRoomId);
            if (chatRoom == null)
            {
                return HttpNotFound("Chat room not found.");
            }

            var messages = _context.ChatMessages
                .Where(m => m.ChatRoom_ID == chatRoomId)
                .OrderBy(m => m.SentAt)
                .ToList();

            // Set ViewBag data for use in the view
            ViewBag.ChatRoomId = chatRoomId;
            ViewBag.PatientId = chatRoom.Patient_ID;
            ViewBag.LabTechId = chatRoom.LabTech_ID;

            // Get the patient's payment status
            var patient = _context.Patients.Find(chatRoom.Patient_ID);
            ViewBag.PaymentStatus = patient?.PaymentStatus;

            return View(messages);
        }

        // Send a message
        [HttpPost]
        public ActionResult SendMessage(int chatRoomId, int senderId, string messageText, string senderType)
        {
            var chatRoom = _context.ChatRooms.Find(chatRoomId);

            if (chatRoom == null)
            {
                return HttpNotFound("Chat room not found.");
            }

            // Check if the sender is a patient and if they are allowed to send more messages
            if (senderType == "Patient")
            {
                int patientMessageCount = _context.ChatMessages
                    .Where(m => m.ChatRoom_ID == chatRoomId && m.SenderId == senderId && m.SenderType == "Patient")
                    .Count();

                var patient = _context.Patients.Find(senderId);

                if (patientMessageCount >= 2 && patient.PaymentStatus != "Paid")
                {
                    // Redirect to payment required view if the patient has exceeded the free message limit
                    return RedirectToAction("PaymentRequired", new { chatRoomId });
                }
            }

            // Add the new message to the ChatMessages table
            var message = new ChatMessage
            {
                ChatRoom_ID = chatRoomId,
                SenderId = senderId,
                MessageText = messageText,
                SentAt = DateTime.Now,
                SenderType = senderType
            };

            _context.ChatMessages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Chat", new { chatRoomId });
        }

        // Send a message
        [HttpPost]
        public ActionResult SendMessage2(int chatRoomId, int senderId, string messageText, string senderType)
        {
            var chatRoom = _context.ChatRooms.Find(chatRoomId);

            if (chatRoom == null)
            {
                return HttpNotFound("Chat room not found.");
            }

            // Check if the sender is a patient and if they are allowed to send more messages
            if (senderType == "Patient")
            {
                int patientMessageCount = _context.ChatMessages
                    .Where(m => m.ChatRoom_ID == chatRoomId && m.SenderId == senderId && m.SenderType == "Patient")
                    .Count();

                var patient = _context.Patients.Find(senderId);

                if (patientMessageCount >= 2 && patient.PaymentStatus != "Paid")
                {
                    // Redirect to payment required view if the patient has exceeded the free message limit
                    return RedirectToAction("PaymentRequired", new { chatRoomId });
                }
            }

            // Add the new message to the ChatMessages table
            var message = new ChatMessage
            {
                ChatRoom_ID = chatRoomId,
                SenderId = senderId,
                MessageText = messageText,
                SentAt = DateTime.Now,
                SenderType = senderType
            };

            _context.ChatMessages.Add(message);
            _context.SaveChanges();

            return RedirectToAction("Chat2", new { chatRoomId });
        }

        // View to display when payment is required
        public ActionResult PaymentRequired(int chatRoomId)
        {
            ViewBag.ChatRoomId = chatRoomId;
            var chatRoom = _context.ChatRooms.Find(chatRoomId);
            if (chatRoom == null)
            {
                return HttpNotFound("Chat room not found.");
            }

            ViewBag.PatientId = chatRoom.Patient_ID;

            return View();
        }

        // Process the payment (this could be integrated with PayPal, Stripe, etc.)
        [HttpPost]
        public ActionResult ProcessPayment(int patientId)
        {
            var patient = _context.Patients.Find(patientId);
            if (patient != null)
            {
                // Update the payment status to "Paid"
                patient.PaymentStatus = "Paid";
                _context.SaveChanges();
            }

            // After payment, redirect back to the chat room
            var chatRoomId = _context.ChatRooms.FirstOrDefault(cr => cr.Patient_ID == patientId)?.ChatRoom_ID;
            if (chatRoomId.HasValue)
            {
                return RedirectToAction("Chat", new { chatRoomId = chatRoomId.Value });
            }

            return RedirectToAction("Index", "Home"); // If no chat room, redirect to home
        }

        // Create a chat room between a patient and lab tech (if it doesn't exist already)
        [HttpPost]
        public ActionResult CreateChatRoom(int labTechId, int patientId)
        {
            // Check if a chat room already exists between the patient and the lab tech
            var existingRoom = _context.ChatRooms.FirstOrDefault(cr => cr.LabTech_ID == labTechId && cr.Patient_ID == patientId);

            if (existingRoom == null)
            {
                var chatRoom = new ChatRoom
                {
                    LabTech_ID = labTechId,
                    Patient_ID = patientId,
                    CreatedAt = DateTime.Now
                };

                _context.ChatRooms.Add(chatRoom);
                _context.SaveChanges();

                return RedirectToAction("Chat", new { chatRoomId = chatRoom.ChatRoom_ID });
            }
            else
            {
                // If a room already exists, redirect to that room
                return RedirectToAction("Chat", new { chatRoomId = existingRoom.ChatRoom_ID });
            }
        }

        // List all chat rooms for a particular patient (for patient view)
        public ActionResult PatientChatRooms(int patientId)
        {
            var chatRooms = _context.ChatRooms
                .Where(cr => cr.Patient_ID == patientId)
                .Include(cr => cr.Lab_Tech)
                .ToList();

            return View(chatRooms);
        }

        // List all chat rooms for a particular lab tech (for lab tech view)
        public ActionResult LabTechChatRooms(int labTechId)
        {
            labTechId = 1;

            var chatRooms = _context.ChatRooms
                .Where(cr => cr.LabTech_ID == labTechId)
                .Include(cr => cr.Patient)
                .ToList();

            return View(chatRooms);
        }

        public ActionResult test() { return View(); }


        public ActionResult GetChatMessages(int chatRoomId)
        {
            var messages = _context.ChatMessages
                .Where(m => m.ChatRoom_ID == chatRoomId)
                .OrderBy(m => m.SentAt)
                .ToList();

            return PartialView("_ChatMessagesPartial", messages); // Return the partial view with updated messages
        }
    }
}