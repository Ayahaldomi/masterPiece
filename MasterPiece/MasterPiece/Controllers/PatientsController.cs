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

        public ActionResult DownloadPdf(int OrderID)
        {
            var testOrder = db.Test_Order.FirstOrDefault(t => t.Order_ID == OrderID);
            // Path to existing PDF template
            string templatePath = Server.MapPath("~/myContent/assets/pdf/templete.pdf");

            // Load the template PDF
            PdfReader reader = new PdfReader(templatePath);

            // Create a memory stream to hold the modified PDF
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfStamper stamper = new PdfStamper(reader, memoryStream);
                int pageCount = reader.NumberOfPages; // Get the total number of pages

                // Define a base font
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                var name = testOrder.Patient.Full_Name;
                var labNum = testOrder.Order_ID;
                var Date = testOrder.Date;

                for (int i = 1; i <= pageCount; i++)
                {
                    // Get the content layer of each page
                    PdfContentByte content = stamper.GetOverContent(i);

                    // Add dynamic content (header) on top of the background
                    content.BeginText();
                    content.SetFontAndSize(bf, 12);

                    // Example: Adding patient information at specific coordinates for each page
                    content.SetTextMatrix(50, 610); // Set the position (x, y)
                    content.ShowText($"Patient Name: {name}");

                    content.SetTextMatrix(50, 590); // Adjust position for Lab Number
                    content.ShowText($"Lab No.: {labNum}");

                    content.SetTextMatrix(50, 570); // Adjust position for Sampling Date
                    content.ShowText($"Sampling Date: {Date}");

                    // End the text object
                    content.EndText();

                    // Adding the Laboratory Report title on every page
                    content.BeginText();
                    content.SetFontAndSize(bf, 16);
                    content.SetTextMatrix(230, 530); // Position for title
                    content.ShowText("Laboratory Report");
                    content.EndText();
                }

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

                // Set the header row count (in this case, it's 1 because the first row is the header)
                chemistryTable.HeaderRows = 1;

                // Add the test data rows
                foreach (var test in testOrder.Test_Order_Tests)
                {
                    chemistryTable.AddCell(CreateStyledCell(test.Test.Test_Name, Element.ALIGN_LEFT));
                    chemistryTable.AddCell(CreateStyledCell(test.Result, Element.ALIGN_CENTER));
                    chemistryTable.AddCell(CreateStyledCell(test.Test.Unit, Element.ALIGN_CENTER));
                    chemistryTable.AddCell(CreateStyledCell(test.Test.Normal_Range, Element.ALIGN_CENTER));
                }

                // Write table to PDF at specific coordinates on the first page (adjust if needed)
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

        
    }
}
