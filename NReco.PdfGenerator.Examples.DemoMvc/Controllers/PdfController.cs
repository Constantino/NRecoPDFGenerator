using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

using System.Web.Security;
using NReco.PdfGenerator;

namespace Controllers {
    
    public class PdfController : Controller {

        public ActionResult DemoPage() {
            return View();
        }

        public void PreProcessFile(string filePath){

            const string style = "<style>body { height:203mm; width:267mm; margin-left:auto; margin-right:auto; }</style>";
            string HTMLCode = System.IO.File.ReadAllText(filePath);
            HTMLCode = HTMLCode.Replace("<head>", "<head>" + style);

            // Open the file.
            using (FileStream fs = System.IO.File.OpenWrite(filePath))
            {
                Byte[] info =
                    new UTF8Encoding(true).GetBytes(HTMLCode);

                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        [ValidateInput(false)]
        public ActionResult GeneratePdf(string htmlContent, string htmlUrl) {

            string[] htmlFiles = htmlUrl.Split(';');

            foreach (var e in htmlFiles) {
                PreProcessFile(e);
            }

            var htmlToPdf = new HtmlToPdfConverter();

            var pdfContentType = "application/pdf";
            Stream output = new System.IO.MemoryStream();
            
            string pdfName = Path.GetTempPath()+"/firstPdf.pdf";
            if (!String.IsNullOrEmpty(htmlUrl)) {
                //return File( htmlToPdf.GeneratePdfFromFile(htmlUrl, null), pdfContentType);
                htmlToPdf.GeneratePdfFromFiles(htmlFiles, null, pdfName);
                return File(pdfName, pdfContentType);
                
            } else {
                return File( htmlToPdf.GeneratePdf(htmlContent, null), pdfContentType);
            }
        }





    }
}
