using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.Utils
{
    public class PdfUtils
    {
        // Example: 
        // PdfUtils.GenerateCerfificate(env, "test.pdf", "cerBOficat2.pdf", new PdfSymbols { FirstName = "BO" });

        public static byte[] GenerateCerfificate(IHostingEnvironment env, string templateName, PdfSymbols pdfSymbols)
        {
            var root = new Uri(env.WebRootPath);
            var rootParent = root.AbsoluteUri.Remove(root.AbsoluteUri.Length - root.Segments.Last().Length);
            var templatePath = rootParent + $@"PDF/Templates/{templateName}";
            //var outputPath = rootParent + $@"wwwRoot/PDF/OutPut/{outputName}";

            var fileLength = "file:///".Length;
            //outputPath = outputPath.Substring(fileLength, outputPath.Count() - fileLength);

            return PdfUtils.GeneratePDF(templatePath.Substring(fileLength, templatePath.Count() - fileLength)
                , pdfSymbols);
        }

        private static byte[] GeneratePDF(string pathToPDFTemplate, PdfSymbols pdf)
        {
            MemoryStream newFileStream;
            using (var existingFileStream = new FileStream(pathToPDFTemplate, FileMode.Open))
            using (newFileStream = new MemoryStream())
            using (var pdfReader = new PdfReader(existingFileStream))
            using (var stamper = new PdfStamper(pdfReader, newFileStream))
            {
                var form = stamper.AcroFields;
                form.GenerateAppearances = true;

                if (form.Fields.Keys.Contains(nameof(pdf.CertificateName)) && pdf.CertificateName != null)
                    form.SetField(nameof(pdf.CertificateName), pdf.CertificateName);

                if (form.Fields.Keys.Contains(nameof(pdf.Date)) && pdf.Date != null)
                    form.SetField(nameof(pdf.Date), pdf.Date);

                if (form.Fields.Keys.Contains(nameof(pdf.Details)) && pdf.Details != null)
                    form.SetField(nameof(pdf.Details), pdf.Details);

                if (form.Fields.Keys.Contains(nameof(pdf.StudentName)) && pdf.StudentName != null)
                    form.SetField(nameof(pdf.StudentName), pdf.StudentName);

                if (form.Fields.Keys.Contains(nameof(pdf.Author)) && pdf.Author != null)
                    form.SetField(nameof(pdf.Author), pdf.Author);

                if (form.Fields.Keys.Contains(nameof(pdf.Company)) && pdf.Company != null)
                    form.SetField(nameof(pdf.Company), pdf.Company);

                stamper.FormFlattening = true;
            }
                return newFileStream.ToArray();
        }
    }

    public class PdfSymbols
    {
        public string CertificateName { get; set; }
        public string Date { get; set; }
        public string Details { get; set; }
        public string StudentName { get; set; }
        public string Author { get; set; }
        public string Company { get; set; }
    }
}
