using System;
using System.Data;
using System.Xml.Linq;
using System.Xml.XPath;
using QBXMLRP2Lib;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Web;

namespace MyWebApp
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var today = DateTime.Now;
            string datefrm = new DateTime(today.Year, today.Month,1).ToString("yyyy-MM-dd");
            string dateto = new DateTime(today.Year, today.Month, today.Day).ToString("yyyy-MM-dd");

        }
        

        XDocument createXmlReq(string reporttype)
        {
            XDocument inputxml = new XDocument(
                    new XDeclaration("1.0", null, null),
                    new XProcessingInstruction("qbxml", "version=\"2.0\""),
                    new XElement("QBXML"));
            XElement QBXML = inputxml.Element("QBXML");

            if (reporttype == "MiscellaneousAR")
            {
                QBXML.Add(
                    new XElement("QBXMLMsgsRq", new XAttribute("onError", "continueOnError"),
                    new XElement("CustomDetailReportQueryRq", new XAttribute("requestID", "1255"),
                    new XElement("CustomDetailReportType", "CustomTxnDetail"),
                    new XElement("ReportDateMacro", "All"),
                    new XElement("ReportAccountFilter", new XElement("AccountTypeFilter", "AccountsReceivable")),
                    new XElement("ReportEntityFilter", new XElement("FullName", selectCustomer.Value)),
                    new XElement("SummarizeRowsBy", "TotalOnly"),
                    new XElement("IncludeColumn", "Date"),
                    new XElement("IncludeColumn", "RefNumber"),
                    new XElement("IncludeColumn", "Memo"),
                    new XElement("IncludeColumn", "OpenBalance"),
                    new XElement("IncludeColumn", "Amount"),
                    new XElement("ReportOpenBalanceAsOf", "Today")

                    )));
            }
            else
            {
                QBXML.Add(
                   new XElement("QBXMLMsgsRq", new XAttribute("onError", "continueOnError"),
                   new XElement("GeneralSummaryReportQueryRq",
                   new XElement("GeneralSummaryReportType", reporttype),
                   selectDateMacro.Value == "Select Date Period" ?
                    new XElement("ReportPeriod",
                    new XElement("FromReportDate", dateFrom.Value),
                    new XElement("ToReportDate", dateTo.Value))
                    : new XElement("ReportDateMacro", selectDateMacro.Value)
                
                    )));
            }

            return inputxml;
        }

        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            
            XDocument inputxml = createXmlReq(selectReport.Value);
            StringWriter writer = new StringWriter();
            inputxml.Save(writer);
            string input = writer.ToString();


            RequestProcessor2 rp = new RequestProcessor2();
            rp.OpenConnection2("WebApp to Query Inventory", "Webapp", QBXMLRPConnectionType.localQBD);

            string ticket = rp.BeginSession(@"D:\Financial Company File\khojandi supermarket.qbw", QBFileMode.qbFileOpenDoNotCare);
            string response= rp.ProcessRequest(ticket, input);
            rp.EndSession(ticket);
            rp.CloseConnection();

            updateTable(response);





        }

        void updateTable(string xmlStr)
        {
            

            XDocument xml = XDocument.Parse(xmlStr);
            XElement root = xml.Root;

            DataTable table = new DataTable();
            DataColumn column;
            DataRow row;
            
            if (selectReport.Value == "MiscellaneousAR")
            {
                foreach(XElement e in root.Descendants("ColDesc").Where(n => n.Attribute("colID").Value != "1" ))
                {
                    column = new DataColumn();
                    column.ColumnName = e.Element("ColType").Value;
                    table.Columns.Add(column);
                }
                // using .where
              //  foreach(XElement e in root.Descendants("DataRow").Where(n => n.Element("ColData").Attribute("colID").Value == "2"))
              //  {
              //      MessageBox.Show(e.ToString());
              //  }


                //using xpath (both works)
                foreach(XElement e in root.XPathSelectElements("//DataRow[ColData/@colID='5']"))
                {
                    row = table.NewRow();
                    row[0] = e.XPathSelectElement("ColData[@colID='2']") != null ?  e.XPathSelectElement("ColData[@colID='2']").Attribute("value").Value : "";
                    row[1] = e.XPathSelectElement("ColData[@colID='3']") != null ? e.XPathSelectElement("ColData[@colID='3']").Attribute("value").Value : "";
                    row[2] = e.XPathSelectElement("ColData[@colID='4']") != null ?  e.XPathSelectElement("ColData[@colID='4']").Attribute("value").Value : "" ;
                    row[3] = e.XPathSelectElement("ColData[@colID='5']") != null ?  e.XPathSelectElement("ColData[@colID='5']").Attribute("value").Value : "" ;
                    row[4] = e.XPathSelectElement("ColData[@colID='6']") != null ?  e.XPathSelectElement("ColData[@colID='6']").Attribute("value").Value : "" ;

                    table.Rows.Add(row);
                }

                divTotal.Visible = true;
                lblTotal.Text = root.XPathSelectElement("//TotalRow/ColData[@colID='5']").Attribute("value").Value;


            }
            else
            {


                foreach (XElement e in root.Descendants("ColDesc"))
                {
                    column = new DataColumn();
                    column.ColumnName = e.Element("ColType").Value;
                    table.Columns.Add(column);
                }

                foreach (XElement e in root.XPathSelectElements("//DataRow | //SubtotalRow | //TotalRow"))
                {

                    row = table.NewRow();

                    foreach (XElement coldata in e.Descendants("ColData").Where(n => n.Attribute("colID").Value == "1"))
                    {
                        row[0] = coldata.Attribute("value").Value;
                    }
                    foreach (XElement coldata in e.Descendants("ColData").Where(n => n.Attribute("colID").Value == "2"))
                    {
                        row[1] = decimal.Parse(coldata.Attribute("value").Value).ToString("C");


                    }


                    table.Rows.Add(row);
                    
                }
            }
            
            reportView.Visible = true;
            reportView.DataSource = table;
            reportView.DataBind();
            reportView.Sort("Date",System.Web.UI.WebControls.SortDirection.Ascending);
        }
    }
}