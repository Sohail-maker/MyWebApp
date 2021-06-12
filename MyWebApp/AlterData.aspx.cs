using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QBPOSXMLRPLib;
using System.Xml.Linq;
using System.IO;
using static MyWebApp.database;

namespace MyWebApp
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ( Session["user"] ==null)
            {
                Response.Redirect("Default.aspx");
            }

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            RequestProcessor rp = new RequestProcessor();
            rp.OpenConnection("WebApp to Query Inventory", "Webapp");
            string ticket = rp.BeginSession(WebForm1.connstring());
            string reponse = rp.ProcessRequest(ticket,database.createxmlreq(txtQuery.Text));
            rp.EndSession(ticket);
            rp.CloseConnection();
            XDocument responsexml = XDocument.Parse(reponse);
            XElement ResElement = responsexml.Element("QBPOSXML").Element("QBPOSXMLMsgsRs").Element("ItemInventoryQueryRs").Element("ItemInventoryRet");
            nametxt.Value = TryGetElementValue(ResElement,"Desc1");
            pricetxt.Value = TryGetElementValue(ResElement,"Price1");
            sizetxt.Value = TryGetElementValue(ResElement,"Size");
            attributetxt.Value = TryGetElementValue(ResElement,"Attribute");
            lblIDtxt.Text = TryGetElementValue(ResElement, "ListID");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            XDocument requestXml = new XDocument(
                new XDeclaration("1.0","utf-8",null),
                new XProcessingInstruction("qbposxml","version=\"3.0\""),
                new XElement("QBPOSXML",
                new XElement("QBPOSXMLMsgsRq", new XAttribute("onError", "stopOnError"),
                new XElement("ItemInventoryModRq",
                new XElement("ItemInventoryMod",
                new XElement("ListID", lblIDtxt.Text),
                new XElement("Desc1", nametxt.Value),
                new XElement("Price1", pricetxt.Value),
                new XElement("Attribute", attributetxt.Value),
                new XElement("Size", sizetxt.Value)

                )))));
            StringWriter writer = new StringWriter();
            requestXml.Save(writer);
            string input = writer.ToString();


            RequestProcessor rp = new RequestProcessor();
            rp.OpenConnection("WebApp to Query Inventory", "Webapp");
            string ticket = rp.BeginSession(WebForm1.connstring());
            string response = rp.ProcessRequest(ticket, input);
            rp.EndSession(ticket);
            rp.CloseConnection();

            XDocument responsexml = XDocument.Parse(response);
            string status = responsexml.Element("QBPOSXML").Element("QBPOSXMLMsgsRs").Element("ItemInventoryModRs").Attribute("statusMessage").Value;
            if (status == "Status OK")
            {
                lblError.Text = "Changes Saved";
                lblError.Style.Add("color", "Green");
            }
            else lblError.Text = "Error";
            
                

        }
    }
}