using QBPOSXMLRPLib;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Linq;
using System.Web.UI.WebControls;



namespace MyWebApp

{   
    
    public partial class WebForm1 : System.Web.UI.Page
    {
        public static string connstring()
        {  
            string ConnString;
            string machine = Environment.MachineName.ToString();
            if (machine == "Sohailspc")
            {
                 ConnString = "Computer Name=Sohailspc;Company Data=Sohail Store;Version=11";
            }
            else ConnString = "Computer Name=Server;Company Data=khojandi supermarket;Version=11";

            return ConnString;
        }
       public RequestProcessor rp = new RequestProcessor();
       public  string ticket = null;

        string response = null;
        
      
        protected void Page_Load(object sender, EventArgs e)
        {
            
          //  DateTime lstupdate = Convert.ToDateTime(lastupdateDate());
          //  
          //  if ((DateTime.Now - lstupdate).TotalMinutes > 30)
          //  {
          //      btnUpdateData_Click(btnUpdateData, EventArgs.Empty);
          //  }

            if(Session["user"] == null)
            {
                lblCost.Visible = false;
            }
        }




        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
            rp.OpenConnection("WebApp to Query Inventory", "Webapp");
            ticket = rp.BeginSession(connstring());

            lblName.Text = null;
            lblPrice.Text = null;
            lblOnHandQty.Text = null;
            lblCost.Text = null;
            lblLastRcvd.Text = null;
            lblitmNumber.Text = null;
            lblAttribute.Text = null;
            lblSize.Text = null;
            lblError.Text = null;




            if (string.IsNullOrEmpty(txtUPC.Text))
            {
                lblError.Text = ("Please insert Input");
            }
            else
            {
                response = rp.ProcessRequest(ticket, database.createxmlreq(txtUPC.Text));
                XDocument xmlResponse = XDocument.Parse(response);
                XElement element = xmlResponse.Element("QBPOSXML").Element("QBPOSXMLMsgsRs").Element("ItemInventoryQueryRs").Element("ItemInventoryRet");
                
                
                
                    try
                    {


                        lblName.Text = element.Element("Desc1").Value;
                        lblPrice.Text = element.Element("Price1").Value;
                        lblOnHandQty.Text = element.Element("QuantityOnHand").Value;
                        lblCost.Text = element.Element("OrderCost").Value;
                        lblLastRcvd.Text = element.Element("LastReceived").Value;
                        lblitmNumber.Text = element.Element("ItemNumber").Value;
                        lblSize.Text = database.TryGetElementValue(element, "Size");
                        lblAttribute.Text = database.TryGetElementValue(element, "Attribute");

                   

                    }
                    catch (NullReferenceException)
                    {
                    string query = $"SELECT `itemNumber`, `Name`, `ALU`, `attribute`, `cost`, `lastReceived`, `onHandQty`, `orderCost`, `price`, `size`, `UPC`, `ALU2`, `UPC2`, `ALU3`, `UPC3`, `ALU4`, `UPC4`, `ALU5`, `UPC5` FROM `inventory` WHERE CONCAT( `itemNumber`, `Name`, `ALU`,  `UPC`, `ALU2`, `UPC2`, `ALU3`, `UPC3`, `ALU4`, `UPC4`, `ALU5`, `UPC5` ) LIKE '%{txtUPC.Text.Trim()}%' LIMIT 1; ";
                    using (MySqlConnection conn = new MySqlConnection(database.connString))
                    {
                        conn.Open();
                        MySqlCommand command = new MySqlCommand(query, conn);
                        using (MySqlDataReader read = command.ExecuteReader())
                        {

                            (MySqlDataReader, string) reader = database.querySqlDatabase(read);


                            try
                            {
                                reader.Item1.Read();

                                if (reader.Item1.HasRows)
                                {

                                    string response2 = rp.ProcessRequest(ticket, database.createxmlreq(reader.Item1.GetString(0)));
                                    XDocument responsexml2 = XDocument.Parse(response2);
                                    XElement element2 = responsexml2.Element("QBPOSXML").Element("QBPOSXMLMsgsRs").Element("ItemInventoryQueryRs").Element("ItemInventoryRet");

                                    lblError.Text = (reader.Item2 + "<br />" + $"Last Updated {lastupdateDate()}" + "<br />" + $"{responsexml2.Element("QBPOSXML").Element("QBPOSXMLMsgsRs").Element("ItemInventoryQueryRs").Attribute("statusMessage").Value}");
                                    lblError.Style.Add("color", "green");

                                    lblName.Text = database.TryGetElementValue(element2, "Desc1");
                                    lblPrice.Text = database.TryGetElementValue(element2, "Price1");
                                    lblOnHandQty.Text = database.TryGetElementValue(element2, "QuantityOnHand");
                                    lblCost.Text = database.TryGetElementValue(element2, "OrderCost");
                                    lblLastRcvd.Text = database.TryGetElementValue(element2, "LastReceived");
                                    lblitmNumber.Text = database.TryGetElementValue(element2, "ItemNumber");
                                    lblSize.Text = database.TryGetElementValue(element2, "Size");
                                    lblAttribute.Text = database.TryGetElementValue(element2, "Attribute");

                                }
                                else lblError.Text = reader.Item2 + "<br />" + xmlResponse.Element("QBPOSXML").Element("QBPOSXMLMsgsRs").Element("ItemInventoryQueryRs").Attribute("statusMessage").Value + " And Nothing Found in UPC Database";

                                reader.Item1.Close();

                            }
                            catch (Exception EX)
                            {
                                lblError.Text = EX.Message;
                            }

                            if (read.IsClosed) { }
                            else read.Close();
                            if (reader.Item1.IsClosed) { }
                            else reader.Item1.Close();
                            if (conn.State == System.Data.ConnectionState.Open) { }
                            else conn.Close();
                        }
                    }

                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ($"{ex.GetType()} says {ex.Message}");
                    }

                    rp.EndSession(ticket);
                    rp.CloseConnection();

                    if (lblName.Text != null)
                    {
                        printDiv.Visible = true;
                    }
                    else
                    {
                        printDiv.Visible = false;
                    }

                
                

            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

            string command = (

                "S4"                                                             + "\n" +
                                                                                   "\n" +
                "ZT"                                                             + "\n" +
                                                                                   "\n" +
                "OD"                                                             + "\n" +
                                                                                   "\n" +
                "D12"                                                            + "\n" +
                                                                                   "\n" +
                "q456"                                                           + "\n" +
                                                                                   "\n" +
                "Q280,24"                                                        + "\n" +
                                                                                   "\n" +
                "N"                                                              + "\n" +
                                                                                   "\n" +
                $"A43,10,0,2,1,1,N,\"{lblName.Text}\""                           + "\n" +
                "LS410,32,1,43,32"                                               + "\n" +
                $"A43,42,0,2,1,1,N,\"{lblSize.Text}\""                           + "\n" +
                $"B200,39,0,1,3,8,131,N,\"{BarcodeConvert(lblitmNumber.Text)}\"" + "\n" +
                $"A43,77,0,2,1,1,N,\"{lblAttribute.Text}\""                      + "\n" +
                $"A39,126,0,4,1,1,N,\"  {lblPrice.Text}\""                       + "\n" +
                $"A256,179,0,2,1,1,N,\" # {lblitmNumber.Text}\""                 + "\n" +
                $"P1,{txtCopies.Value}"                                          + "\n" );

            //using file copy
            // string printAddress = @"\\192.168.168.1\zlp2844";
            // File.WriteAllText(@"C:\website\source\queue.prn", command);
            // File.Copy(@"C:\website\source\queue.prn", printAddress);


            //using tcp socket
            IPAddress ip = IPAddress.Parse("192.168.168.15");
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(ip, 9100);
            StreamWriter writer = new StreamWriter(tcpClient.GetStream());
            writer.Write(command);
            writer.Flush();
            writer.Close();
            tcpClient.Close();


        }

        static string BarcodeConvert(string Barcode)
        { 
            string rslt = null;
            int lnt = Barcode.Length;
            switch (lnt)
            {
                case 1:
                    rslt = $"00000{Barcode}";
                    break;
                case 2:
                    rslt = $"0000{Barcode}";
                    break;
                case 3:
                    rslt = $"000{Barcode}";
                    break;
                case 4:
                    rslt = $"00{Barcode}";
                    break;
                case 5:
                    rslt = $"0{Barcode}";
                    break;

            }
            return rslt;
        }

        protected void btnUpdateData_Click(object sender, EventArgs e)
        {
            lblError.Text = database.updatedatabase();
            lblError.Style.Add("color", "green");
        }
        public string lastupdateDate()
        {
            string query = "SELECT * FROM `lastupdate` WHERE 1";
            MySqlConnection conn = new MySqlConnection(database.connString);
            conn.Open();
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            return reader.GetDateTime(2).ToString();

        }

        protected void btnLabelPrint_Click(object sender, EventArgs e)
        {
            printlabel();
        }

        void printlabel()
        {
           string btmp = ("BITMAP 160, 54, 12, 192, 1,ÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿðÿÿÿÿüÿÿÿÿð ÿÿÿÿàÿÿÿÿðÿÿÿÿÀÿÿÿÿð ÿÿÿÿ€ÿÿÿÿÿÿüÿ ÿÿÿÿþÿøÿþ ÿÿÿÿüÿðÿü ÿÿÿÿðaÿñÿü ÿÿÿÿññÿñÿüÿÿÿÿÿùýÿñÿüÿÿÿÿÿûÿÿñùøÿÿÿÿÿÿÿÿñùøÿÿÿÿÿôÿñýøÿÿÿÿÿðÿñùøøÿÿÿÿðÿñýøÀÿÿÿþ?ÿøÿø ÿÿÿþÿøÿø ÿÿÿþ?ÿüÿø ÿÿÿðÿüø ÿÿÿðÿþ?ü  ÿÿÿõWÿÿü  ÿÿÿÿÿÿþü ÿÿÿùÿüü ÿÿÿøÿüþ ÿÿÿðÿÜÿ ÿÿÿñÿÿŒÿ€ÿÿÿñÿÿœÿÀÿÿÿùÿÿÞÿðÿÿÿðÿœÿýWUÿÿÿðÿœÿÿÿÿÿÿÿðÿÜÿÿÿÿÿÿÿÿÿÿüxÿüÿÿÿõÿÿüpüÿÿÿñÿÿü`üÿÿÿñÿÿÿüÿÿÿñ‡ÿÿþ?üÿÿÿðÿÿÿ?üÿÿÿøÿÿÿ?üÿÿÿÿÿÿÿÿüÿÿÿÿÿÿÿÿŸüÿÿÿñÿÿÿÿŸüÿÿÿðÿÿÿÿüÿÿÿðÿÿÿßüÿÿÿüÿÿÿÿüÿÿÿüGÿßÿÿü ÿÿÿüãÿÏÿÿü ÿÿÿüÿÏÿÿü ÿÿÿðÿÏÿÿü ÿÿÿðÿÇÿÿü ÿÿÿ÷ÿÿÃÇÿü ÿÿÿÿÿÿÁ‡ÿü ÿÿÿÿÿÿðÿü ÿÿÿðÿýÿÿWÿÿÿÿðÿÿÿÿÿÿÿÿÿÿñÿÿÿÿüÿÿÿÿÿÿÿ ?üÿÿÿÿÿÿü ü ßÿÿþ?ÿü?ÿü ÿÿüÿüÿü ÿÿðÿÿüÿþ ÿÿðÿüÿþ ÿÿðÿþÿþ ÿÿõßÿüÿü ÿÿÿÿÿü÷ÿü ÿÿõWÿü÷ÿüÿÿÿÿðÿøÿüÿÿÿÿðÿüÿÿÿÿÿÿÿñçÿÿÿÿÿÿÿÿÿÿñÇÿÿÿüÿÿÿñÇþÿÿüÿÿÿð‡ÿÿÿüÿÿÿøþÿÿüÿÿÿþ?þ?ÿüÿÿÿÿÿþ?ÿâÿÿÿÿßÿÿÁÿÿÿðÿŒ?ÿ€‡ÿÿÿðÿôÿ€‡ÿÿÿò'ÿüÿ€‡ÿÿÿÿÿÿÜÿÁÿÿÿÿÿÿŒgÿÁÿÿÿÿÿþ\"ÿöÿÿÿÿÿþOÿü ÿÿÿÿÿÿÿÿü ÿÿÿÿÿÿßÿÿü ÿÿÿÿÿÿÿÿÿü ÿÿÿÿÿþÿÿÿü ÿÿÿûçÿÿÿü ÿÿÿùþ|ÿü ÿÿÿðþ|Gÿü ÿÿÿðþ<gÿüÿÿÿÿøþGÿüÝÿÿÿþÿÿŒÿüÿÿÿÿÿÿÄÿüÿÿÿÿÿÿð?ÿüÿÿÿùÿüÿüÿÿÿøÿüÿüÿÿÿðÿüÿüÿÿÿñÿÿüÿüÿÿÿñÿÿüÿüÿÿùÿÿüÿüÿÿðÿüÿüÿÿðÿüÿüÿÿõWÿüÿüÿÿÿÿÿüÿüÿÿõWÿüÿü?ÿÿðÿüoÿü ÿÿÿðÿÿÿü ÿÿÿøçÿÿÿÿþ ÿÿÿügÿÿÿÿþ ÿÿÿþÿÿÿÿþ ÿÿÿþÿÿÿÿÿ€ÿÿÿÿŸÿÿÿÿÿÀÿÿÿÿÿÿÿÿÿÿðÿÿÿú§ÿÿÿÿÿÿÿÿÿÿðÿÿÿÿÿÿý ÿÿðÿÿÿÿÿÿ€ ÿógÿãÿÿÿ@  ÿògÿÁÿÿð   ÿðgÿÁÿÿ  øÿñÿÿÁÿþ  çøÿÿÿñÁÿÿ _÷øÿðÀAÿþÏçøÿðÎÿþß÷øÿð'Ÿÿþßçøÿþgÿþß÷üÿø±ÿþÏçøÿñÿü    ÿóŸŸÿþ   ÿ÷ßÆÿüß÷üÿÿÿÀAÿþÏçøÿõÿñÁÿüß÷üÿðÿÿÁÿüß÷øÿðÿÁÿüß÷üÿþÿÁÿüÏçøÿÿÿÁÿüß÷üÿþÿÁÿüßçøÿðÿÁÿüß÷üÿðÿÿÁÿü    ÿøÿÁÿü   ÿþÿÁÿüßçüÿÿÿÁÿüß÷üÿðÿÁÿüÏçüÿð_ÿÁÿüß÷üÿñÿÿÁÿüßçüÿ÷ÿÿÁÿüß÷üÿóÿÿÁÿü?ÏçüÿðÿÁÿüß÷üÿðÿÁÿø?ßçü?ÿüÿÁÿü    ÿüÃÿÁÿø    ?ÿüGÿÁÿø=ß÷üÿüƒÿÁÿø?ßçü?ÿðÿÁÿø?ß÷üÿðÿÁÿø?Ïçü?ÿñÿÿÁÿøß÷ü?ÿÿÿÿÁÿø?ßçü?ÿÿÿÿÁÿðß÷üÿðÿÁÿøÏçü?ÿðÿÁÿðß÷üÿþgÿÁÿðˆ€ˆ?ÿügÿÁÿð    ?ÿøÿÁÿðÏçü?ÿñÿÁÿðß÷ü?ÿóŸÿÁÿðßçü?ÿÿÿÿÁÿðß÷ü?ÿÿÿÿÁÿðÏçþ?ÿðÿÁÿðß÷ü?ÿðÿÁÿðßçþ?ÿðÿÁÿðß÷ü?ÿþÿÁÿàÏçþ?ÿüÿÁÿÀß÷ü?ÿð‡ÿÁÿ€    ?ÿñçÿÁÿ  ET?ÿû÷àÁÿ  çþ?ÿÿÿÄAü ÷ü?ÿÿÿü€ ÷þ?ÿðŸüð þ?ÿð¹øü þ?ÿñG€ðÿ€üÿòg™€ ?ÿà >?ÿ" + "\n"
                        + "BITMAP 160, 246, 16, 32, 1,ògŸ ÿü ÿÿÿÿÿògÆ0 ÿÿþ ?ÿÿÿÿÿýÿÀxÿÿÿÀ ÿÿÿÿÿÿÿûüÿÿÿø ÿÿÿÿÿÿçÿÿÿÿÿü ÿÿÿÿÿÿçÿÿÿÿÿÿÿ€ÿÿÿÿÿðÿÿÿÿÿÿÿð ÿÿÿÿðÿÿÿÿÿÿÿü ÿÿÿÿðÿÿÿÿÿÿÿÿ ÿÿÿÿúçÿÿÿÿÿÿÿÿà ÿÿÿÿÿçÿÿÿÿÿÿÿÿü ÿÿÿÿûÿÿÿÿÿÿÿÿÿ ÿÿÿÿÿÿÿÿÿÿÿÿÿÿÀÿÿÿÿÿÿÿÿÿÿÿÿÿÿøÿÿÿÿÿÿÿÿÿÿÿÿÿÿþÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿƒÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿƒÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿƒÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿƒÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿƒÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿƒÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿƒÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÁÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÁÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÁÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿãÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿ÷ÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿÿ");

            string command;
           command   = (
                     "SIZE 54 mm,80 mm"                                                                        + "\n"
                    + "GAP 0.00,0.00"                                                                          + "\n"
                    + "REFERENCE 0,0"                                                                          + "\n"
                    + "SPEED 4.0"                                                                              + "\n"
                    + "DENSITY 8"                                                                              + "\n"
                    + "SET PEEL OFF"                                                                           + "\n"
                    + "SET CUTTER OFF"                                                                         + "\n"
                    + "SET PARTIAL_CUTTER OFF"                                                                 + "\n"
                    + "SET TEAR ON"                                                                            + "\n"
                    + "DIRECTION 0"                                                                            + "\n"
                    + "SHIFT 0 "                                                                               + "\n"
                    + "OFFSET 0 mm"                                                                            + "\n"
                    + "CLS"                                                                                    + "\n"
                    + $"{btmp}"                                                                                + "\n"
                    + $"BARCODE 130,14,\"128M\",53,0,90,4,12,\"!105{BarcodeConvert(lblitmNumber.Text)}\""      + "\n"
                    + $"TEXT 330,14,\"3\",90,1,1,\"{lblAttribute.Text}\""                                      + "\n"
                    + $"TEXT 300,14,\"3\",90,1,1,\"{lblSize.Text}\""                                           + "\n"
                    + $"TEXT 380,14,\"4\",90,1,1,\"{lblName.Text.Replace("\"","")}\""                          + "\n"
                    + $"TEXT 289,328,\"5\",90,1,3,\"{(Convert.ToDecimal(lblPrice.Text)).ToString("N0")}\""     + "\n"        
                    + $"TEXT 130,450,\"2\",90,1,1,\"AFN\""                                                     + "\n"
                    + $"TEXT 74,138,\"7\",90,1,1,\"{BarcodeConvert(lblitmNumber.Text)}\""                      + "\n"
                    + "BAR 80,600,300,10"                                                                      + "\n"
                    + "PRINT 1,1"                                                                              + "\n" );
            TcpClient client = new TcpClient("192.168.168.16", 9100);
            StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.GetEncoding("ISO-8859-1"));
            writer.Write(command);
            writer.Flush();
            writer.Close();
        }
    }
    }

        
    





     
