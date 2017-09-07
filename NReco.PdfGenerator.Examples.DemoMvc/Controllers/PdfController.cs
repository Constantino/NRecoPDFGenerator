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

            string[] htmlFiles;

            try
            {
                htmlFiles = Directory.GetFiles(htmlUrl);
            }
            catch (Exception e) {
                throw e;
            }
            
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            foreach (var htmlDoc in htmlFiles)
            {
                
                doc.Load(htmlDoc);

                var x = doc.DocumentNode.SelectSingleNode("//div[@class='pageHeader']");
                if (!object.Equals(x, null)) x.Remove();

                x = doc.DocumentNode.SelectSingleNode("//div[@class='leftNav']");
                if (!object.Equals(x, null)) x.Remove();

                x = doc.DocumentNode.SelectSingleNode("//div[@class='topicContent']");
                if (!object.Equals(x, null)) x.SetAttributeValue("Style", "margin-left: 0px; margin-top: 100px;");

                MemoryStream output = new System.IO.MemoryStream();
                doc.Save(output);

                System.IO.File.WriteAllBytes(htmlDoc, output.ToArray());
            }

            var htmlToPdf = new HtmlToPdfConverter();
            htmlToPdf.Orientation = PageOrientation.Landscape;
            htmlToPdf.PageHeaderHtml = "<img width='182' height='75' src='https://www.avanade.com/~/media/logo/avanade-logo.svg' align='right' hspace='12' v:shapes='Picture_x0020_2'>";
            htmlToPdf.PageFooterHtml = "<span style='font-size:8.0pt;font-family:&quot;Segoe UI&quot;,sans-serif;color:#70AD47;mso-themecolor:accent6;mso-font-kerning:12.0pt'> </span><span style='font-size:"+
            "8.0pt; font - family:&quot; Segoe UI&quot;,sans - serif; color: black; mso - themecolor:text1; mso - font - kerning:12.0pt'>&lt;Confidential&gt; See Avanade’s </span><a href='https://at.avanade.com/organizations/Policies/Policies2/Forms/Document%20Set/docsethomepage.aspx?ID=670&amp;FolderCTID=0x0120D52000634FE8B87F4B4141A21BFCB3CDC3E3D6&amp;List=caf52708-714a-4e5f-ba50-5017dacf9744&amp;RootFolder=/organizations/Policies/Policies2/Data%20Management'><span style='font-size:8.0pt;font-family:&quot;Segoe UI&quot;,sans-serif;color:#FF5800;mso-font-kerning:12.0pt'>Data Classification and Protection Standard</span></a><span class='MsoHyperlink'><span style='font-size:8.0pt;font-family:&quot;Segoe UI&quot;,sans-serif;color:#70AD47;mso-themecolor:accent6;mso-font-kerning:12.0pt'><o:p></o:p></span></span></p><p class='MsoNormal'><span style='font-size:8.0pt;font-family:&quot;Segoe UI&quot;,sans-serif'>©2017 Avanade Inc. All Rights Reserved<o:p></o:p></span>";

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
