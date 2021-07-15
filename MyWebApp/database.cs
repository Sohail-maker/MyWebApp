using System;
using System.Xml;
using System.Xml.Linq;
using QBPOSXMLRPLib;
using System.Data.SQLite;
using System.Diagnostics;
using System.Web;

namespace MyWebApp

{

    public static class database 

    {
        public static string connString = $@"Data Source={HttpRuntime.AppDomainAppPath}\source\data.db;version=3;";

        public static string updatedatabase()
        {   
            RequestProcessor rp = new RequestProcessor();
            XmlDocument inputXml = new XmlDocument();

            inputXml.AppendChild(inputXml.CreateXmlDeclaration("1.0", null, null));
            inputXml.AppendChild(inputXml.CreateProcessingInstruction("qbposxml", "version=\"3.0\""));

            XmlElement qbXML = inputXml.CreateElement("QBPOSXML");
            inputXml.AppendChild(qbXML);

            XmlElement QBPOSXMLMsgsRq = inputXml.CreateElement("QBPOSXMLMsgsRq");
            qbXML.AppendChild(QBPOSXMLMsgsRq);
            QBPOSXMLMsgsRq.SetAttribute("onError", "stopOnError");

            XmlElement ItemInventoryQueryRq = inputXml.CreateElement("ItemInventoryQueryRq");
            QBPOSXMLMsgsRq.AppendChild(ItemInventoryQueryRq);
            ItemInventoryQueryRq.SetAttribute("requestID", "0");

            XmlElement ItemNumberFilter = inputXml.CreateElement("ItemNumberRangeFilter");
            ItemInventoryQueryRq.AppendChild(ItemNumberFilter);

            ItemNumberFilter.AppendChild(inputXml.CreateElement("FromItemNumber")).InnerText = "1";
            ItemNumberFilter.AppendChild(inputXml.CreateElement("ToItemNumber")).InnerText = "10000";

            string input = inputXml.OuterXml;
            rp.OpenConnection("WebApp to Query Inventory", "Webapp");
            string ticket = rp.BeginSession(WebForm1.connstring());
            string response = rp.ProcessRequest(ticket, input);
            rp.EndSession(ticket);
            rp.CloseConnection();

            XDocument xml = XDocument.Parse(response);
            XElement element = xml.Root;
            int num = 0;
            string ret = null;

            



            try
            {
                using (SQLiteConnection db = new SQLiteConnection(connString))
                {
                    db.Open();
                    
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    using (SQLiteCommand ySqlCommand = new SQLiteCommand(db))
                    {
                        using (var transaction = db.BeginTransaction())
                        {
                            foreach (XElement e in element.Descendants("ItemInventoryRet"))
                            {



                                num++;
                                string listIDRS = TryGetElementValue(e, "ListID");
                                string itemNumber = TryGetElementValue(e, "ItemNumber");
                                string NameRS = TryGetElementValue(e, "Desc1").Replace("'", "");
                                string ALURS = TryGetElementValue(e, "ALU");
                                string costRS = TryGetElementValue(e, "Cost");
                                string itemTypeRs = TryGetElementValue(e, "ItemType");
                                string lastReceivedRS = TryGetElementValue(e, "LastReceived");
                                string onHandqtyRS = TryGetElementValue(e, "QuantityOnHand");
                                string ordercostRS = TryGetElementValue(e, "OrderCost");
                                string priceRS = TryGetElementValue(e, "Price1");
                                string UPCRS = TryGetElementValue(e, "UPC");
                                string ALU2RS = TryGetElementValue(e.Element("VendorInfo2"), "ALU");
                                string UPC2RS = TryGetElementValue(e.Element("VendorInfo2"), "UPC");
                                string ALU3RS = TryGetElementValue(e.Element("VendorInfo3"), "ALU");
                                string UPC3RS = TryGetElementValue(e.Element("VendorInfo3"), "UPC");
                                string ALU4RS = TryGetElementValue(e.Element("VendorInfo4"), "ALU");
                                string UPC4RS = TryGetElementValue(e.Element("VendorInfo4"), "UPC");
                                string ALU5RS = TryGetElementValue(e.Element("VendorInfo5"), "ALU");
                                string UPC5RS = TryGetElementValue(e.Element("VendorInfo5"), "UPC");
                                string timeModifiedRS = TryGetElementValue(e, "TimeModified");
                                string sizeRS = null;
                                string attibuteRS = null;
                                if (TryGetElementValue(e, "Attribute") != null) { attibuteRS = TryGetElementValue(e, "Attribute").Replace("'", ""); }
                                if (TryGetElementValue(e, "Size") != null) { sizeRS = TryGetElementValue(e, "Size").Replace("'", ""); }

                                string query = "INSERT OR REPLACE INTO inventory(      " +
                                " listID,                                 " +
                                " itemNumber,                             " +
                                " Name,                                   " +
                                " ALU,                                    " +
                                " attribute,                              " +
                                " cost,                                   " +
                                " itemType,                               " +
                                " lastReceived,                           " +
                                " onHandQty,                              " +
                                " orderCost,                              " +
                                " price,                                  " +
                                " size,                                   " +
                                " UPC,                                    " +
                                " ALU2,                                   " +
                                " UPC2,                                   " +
                                " ALU3,                                   " +
                                " UPC3,                                   " +
                                " ALU4,                                   " +
                                " UPC4,                                   " +
                                " ALU5,                                   " +
                                " UPC5,                                   " +
                                " timeModified)                           " +
                                " VAlUES(                                   " +
                                " '" + listIDRS + "',                       " +
                                " '" + itemNumber + "',                     " +
                                " '" + NameRS + "',                           " +
                                " '" + ALURS + "',                          " +
                                " '" + attibuteRS + "',                     " +
                                " " + costRS + ",                           " +
                                " '" + itemTypeRs + "',                     " +
                                " '" + lastReceivedRS + "',                 " +
                                " '" + onHandqtyRS + "',                    " +
                                " " + ordercostRS + ",                      " +
                                " " + priceRS + ",                          " +
                                " '" + sizeRS + "',                         " +
                                " '" + UPCRS + "',                          " +
                                " '" + ALU2RS + "',                         " +
                                " '" + UPC2RS + "',                         " +
                                " '" + ALU3RS + "',                         " +
                                " '" + UPC3RS + "',                         " +
                                " '" + ALU4RS + "',                         " +
                                " '" + UPC4RS + "',                         " +
                                " '" + ALU5RS + "',                         " +
                                " '" + UPC5RS + "',                         " +
                                " '" + timeModifiedRS + "');                 ";


                                ySqlCommand.CommandText = query;
                                ySqlCommand.ExecuteNonQuery();

                                ret = $"Updated {num.ToString()} Items";


                            }
                            transaction.Commit();
                        }
                    }
                    stopwatch.Stop();
                     
                    

                    ret += "</br>in" + stopwatch.ElapsedMilliseconds + "MiliSeconds";

                    string lastexch = (
                  "  UPDATE                                                         " +
                  "     `lastupdate`                                                " +
                  "  SET                                                            " +
                  "     `id` = 1,                                                   " +
                  "     `machine` = '" + System.Environment.MachineName + "',       " +
                  $"     `date` = '{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}',  " +
                  "     `totalitems` = '" + num + "'                                " +
                  "  WHERE                                                          " +
                  "      1                                                          "
                    );

                    db.Close();

                }


            }
            catch (Exception ex)
            {

                ret = ($"{ex.GetType()} :{ex.Message}");
            }




            return ret;

        }




        public static string TryGetElementValue(XElement e, string elementName, string defaultValue = null)
        {
            string ret = defaultValue;
            try
            {
                var foundEl = e.Element(elementName);

                if (foundEl != null)
                {
                    ret = foundEl.Value;
                }
                else
                {
                    ret = defaultValue;
                }
            }
            catch (Exception ex)
            {

            }

            return ret;
        }


        public static string createxmlreq(string scan)
        {
            string inputUPC = scan.Trim();

            //xml creation starts

            XmlDocument inputXml = new XmlDocument();

            inputXml.AppendChild(inputXml.CreateXmlDeclaration("1.0", null, null));
            inputXml.AppendChild(inputXml.CreateProcessingInstruction("qbposxml", "version=\"3.0\""));

            XmlElement qbXML = inputXml.CreateElement("QBPOSXML");
            inputXml.AppendChild(qbXML);

            XmlElement QBPOSXMLMsgsRq = inputXml.CreateElement("QBPOSXMLMsgsRq");
            qbXML.AppendChild(QBPOSXMLMsgsRq);
            QBPOSXMLMsgsRq.SetAttribute("onError", "stopOnError");

            XmlElement ItemInventoryQueryRq = inputXml.CreateElement("ItemInventoryQueryRq");
            QBPOSXMLMsgsRq.AppendChild(ItemInventoryQueryRq);
            ItemInventoryQueryRq.SetAttribute("requestID", "0");



            if (int.TryParse(inputUPC, out int intInputUPC))

            {

                if (inputUPC.Length > 6)
                {
                    XmlElement UPCFilter = inputXml.CreateElement("UPCFilter");
                    ItemInventoryQueryRq.AppendChild(UPCFilter);

                    UPCFilter.AppendChild(inputXml.CreateElement("MatchStringCriterion")).InnerText = "equal";
                    UPCFilter.AppendChild(inputXml.CreateElement("UPC")).InnerText = inputUPC;
                }
                else
                {

                    XmlElement ItemNumberFilter = inputXml.CreateElement("ItemNumberFilter");
                    ItemInventoryQueryRq.AppendChild(ItemNumberFilter);

                    ItemNumberFilter.AppendChild(inputXml.CreateElement("MatchNumericCriterion")).InnerText = "Equal";
                    ItemNumberFilter.AppendChild(inputXml.CreateElement("ItemNumber")).InnerText = inputUPC.TrimStart('0');

                }
            }
            else
            {
                XmlElement Desc1Filter = inputXml.CreateElement("Desc1Filter");
                ItemInventoryQueryRq.AppendChild(Desc1Filter);

                Desc1Filter.AppendChild(inputXml.CreateElement("MatchStringCriterion")).InnerText = "Contains";
                Desc1Filter.AppendChild(inputXml.CreateElement("Desc1")).InnerText = inputUPC;


            }




            return inputXml.OuterXml;
        }


        public static (SQLiteDataReader, string) querySqlDatabase(SQLiteDataReader reader)
        {

            if (reader.HasRows)
            {
                return (reader, "Available in UPC DataBase");

            }

            else
            {
                string update = updatedatabase();
                return (reader, update);

            }

        }

    }
      
}
