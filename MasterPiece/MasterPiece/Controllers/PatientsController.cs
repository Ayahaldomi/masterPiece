using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterPiece.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf.Grid;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MasterPiece.Controllers
{
    public class PatientsController : Controller
    {
        private MasterPieceEntities db = new MasterPieceEntities();

        public ActionResult DownloadPdf()
        {
            // Path to your existing PDF template
            string templatePath = Server.MapPath("~/myContent/assets/pdf/templete.pdf");

            // Load the template PDF
            PdfReader reader = new PdfReader(templatePath);

            // Create a memory stream to hold the modified PDF
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfStamper stamper = new PdfStamper(reader, memoryStream);

                // Get the content layer of the existing PDF (background)
                PdfContentByte content = stamper.GetOverContent(1); // Page 1 of the PDF

                // Define a base font
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                var name = "Ayah Hasan";

                // Add dynamic content on top of the background
                content.BeginText();
                content.SetFontAndSize(bf, 12);

                // Example: Adding patient information at specific coordinates
                content.SetTextMatrix(50, 610); // Set the position (x, y)
                content.ShowText($"Patient Name: {name}");

                content.SetTextMatrix(50, 590); // Adjust position for Lab Number
                content.ShowText("Lab No.: 240812524");

                content.SetTextMatrix(50, 570); // Adjust position for Sampling Date
                content.ShowText("Sampling Date: 2024-09-04 14:34:41");

                // End the text object
                content.EndText();

                // Adding the Laboratory Report title
                content.BeginText();
                content.SetFontAndSize(bf, 16);
                content.SetTextMatrix(230, 530); // Position for title
                content.ShowText("Laboratory Report");
                content.EndText();

                // Create the chemistry table
                PdfPTable chemistryTable = new PdfPTable(4);
                chemistryTable.TotalWidth = 500f;
                chemistryTable.SetWidths(new float[] { 40f, 20f, 20f, 20f }); // Adjust column widths
                chemistryTable.LockedWidth = true;
                chemistryTable.HorizontalAlignment = Element.ALIGN_CENTER;

                // Add the headers with underline only
                PdfPCell testNameHeader = new PdfPCell(new Phrase("Test Name", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD)));
                testNameHeader.Border = PdfPCell.BOTTOM_BORDER;
                testNameHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                chemistryTable.AddCell(testNameHeader);

                PdfPCell resultHeader = new PdfPCell(new Phrase("Result", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD)));
                resultHeader.Border = PdfPCell.BOTTOM_BORDER;
                resultHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                chemistryTable.AddCell(resultHeader);

                PdfPCell unitHeader = new PdfPCell(new Phrase("Unit", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD)));
                unitHeader.Border = PdfPCell.BOTTOM_BORDER;
                unitHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                chemistryTable.AddCell(unitHeader);

                PdfPCell normalRangeHeader = new PdfPCell(new Phrase("Normal Range", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD)));
                normalRangeHeader.Border = PdfPCell.BOTTOM_BORDER;
                normalRangeHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                chemistryTable.AddCell(normalRangeHeader);

                // Add test data (no borders)
                chemistryTable.AddCell(CreateStyledCell("Cholesterol", Element.ALIGN_LEFT));
                chemistryTable.AddCell(CreateStyledCell("178", Element.ALIGN_CENTER));
                chemistryTable.AddCell(CreateStyledCell("mg/dl", Element.ALIGN_CENTER));
                chemistryTable.AddCell(CreateStyledCell("( < 200 )", Element.ALIGN_CENTER));

                chemistryTable.AddCell(CreateStyledCell("Triglycerides", Element.ALIGN_LEFT));
                chemistryTable.AddCell(CreateStyledCell("89", Element.ALIGN_CENTER));
                chemistryTable.AddCell(CreateStyledCell("mg/dl", Element.ALIGN_CENTER));
                chemistryTable.AddCell(CreateStyledCell("( 35.0 - 160.0 )", Element.ALIGN_CENTER));

                chemistryTable.AddCell(CreateStyledCell("SGOT (AST)", Element.ALIGN_LEFT));
                chemistryTable.AddCell(CreateStyledCell("28", Element.ALIGN_CENTER));
                chemistryTable.AddCell(CreateStyledCell("U/L", Element.ALIGN_CENTER));
                chemistryTable.AddCell(CreateStyledCell("( < 35 )", Element.ALIGN_CENTER));

                chemistryTable.AddCell(CreateStyledCell("SGPT (ALT)", Element.ALIGN_LEFT));
                chemistryTable.AddCell(CreateStyledCell("21", Element.ALIGN_CENTER));
                chemistryTable.AddCell(CreateStyledCell("U/L", Element.ALIGN_CENTER));
                chemistryTable.AddCell(CreateStyledCell("( < 35 )", Element.ALIGN_CENTER));

                // Write table to PDF at specific coordinates
                PdfContentByte tableContent = stamper.GetOverContent(1);
                chemistryTable.WriteSelectedRows(0, -1, 50, 500, tableContent); // Position for the table

                // Close the stamper and the reader
                stamper.Close();
                reader.Close();

                // Return the PDF in a new tab
                byte[] bytes = memoryStream.ToArray();
                Response.AppendHeader("Content-Disposition", "inline; filename=LaboratoryReport.pdf");
                return File(bytes, "application/pdf");
            }
        }

        // Helper method to create styled cells without borders
        private PdfPCell CreateStyledCell(string content, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(content));
            cell.Border = PdfPCell.NO_BORDER; // No border for the data rows
            cell.HorizontalAlignment = alignment;
            cell.PaddingBottom = 6f; // Optional: Add some padding to make the table more readable
            return cell;
        }





        //public ActionResult CreatePDFDocument()
        //{
        //    //Create an instance of PdfDocument.
        //    //Create a new PDF document.
        //    PdfDocument doc = new PdfDocument();
        //    //Add a page.
        //    PdfPage page = doc.Pages.Add();
        //    //Create a PdfGrid.
        //    PdfGrid pdfGrid = new PdfGrid();
        //    //Create a DataTable.
        //    DataTable dataTable = new DataTable();
        //    //Add columns to the DataTable.
        //    dataTable.Columns.Add("ID");
        //    dataTable.Columns.Add("Name");
        //    //Add rows to the DataTable.
        //    dataTable.Rows.Add(new object[] { "E01", "Clay" });
        //    dataTable.Rows.Add(new object[] { "E02", "Thomas" });
        //    dataTable.Rows.Add(new object[] { "E03", "Andrew" });
        //    dataTable.Rows.Add(new object[] { "E04", "Paul" });
        //    dataTable.Rows.Add(new object[] { "E05", "Gary" });
        //    //Assign data source.
        //    pdfGrid.DataSource = dataTable;
        //    //Apply built-in table style
        //    pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);
        //    //Draw grid to the page of PDF document.
        //    pdfGrid.Draw(page, new PointF(10, 10));
        //    //Open the document in browser after saving it.
        //    doc.Save("Output.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
        //    //Close the document.
        //    doc.Close(true);
        //    return View();
        //}
        // GET: Patients
        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Patient_ID,Full_Name,Date_Of_Birth,Gender,Marital_Status,Nationality,Phone_Number,Home_Address,Note")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Patient_ID,Full_Name,Date_Of_Birth,Gender,Marital_Status,Nationality,Phone_Number,Home_Address,Note")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
