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
        
        [ValidateInput(false)]
        public ActionResult GeneratePdf(string htmlContent, string htmlUrl) {

            string[] htmlFiles = Directory.GetFiles(htmlUrl);
            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            foreach (var htmlDoc in htmlFiles)
            {
                
                doc.Load(htmlDoc);

                doc.DocumentNode.SelectSingleNode("//div[@class='pageHeader']").Remove();
                doc.DocumentNode.SelectSingleNode("//div[@class='leftNav']").Remove();
                doc.DocumentNode.SelectSingleNode("//div[@class='topicContent']").SetAttributeValue("Style", "margin-left: 0px");

                MemoryStream output = new System.IO.MemoryStream();
                doc.Save(output);

                System.IO.File.WriteAllBytes(htmlDoc, output.ToArray());
            }

            var htmlToPdf = new HtmlToPdfConverter();
            htmlToPdf.Orientation = PageOrientation.Landscape;
            var pdfContentType = "application/pdf";
            
            string pdfName = Path.GetTempPath()+"/Document.pdf";

            if (!String.IsNullOrEmpty(htmlUrl)) {
                htmlToPdf.GeneratePdfFromFiles(htmlFiles, null, pdfName);
                return File(pdfName, pdfContentType);
                
            } else {
                return File( htmlToPdf.GeneratePdf(htmlContent, null), pdfContentType);
            }
        }
        
    }
}
