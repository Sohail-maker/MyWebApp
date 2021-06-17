using QBPOSXMLRPLib;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using System.Net;
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
                    MySqlConnection conn = new MySqlConnection(database.connString);
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader read = command.ExecuteReader();
                    
                    (MySqlDataReader, string) reader = database.querySqlDatabase(read);
                    

                            try
                            {
                                reader.Item1.Read();

                                if (reader.Item1.HasRows)
                                {

                                    string response2 = rp.ProcessRequest(ticket, database.createxmlreq(reader.Item1.GetString(0)));
                                    XDocument responsexml2 = XDocument.Parse(response2);
                                    XElement element2 = responsexml2.Element("QBPOSXML").Element("QBPOSXMLMsgsRs").Element("ItemInventoryQueryRs").Element("ItemInventoryRet");

                                    lblError.Text = (  reader.Item2 + "<br />" + $"Last Updated {lastupdateDate()}" + "<br />" + $"{responsexml2.Element("QBPOSXML").Element("QBPOSXMLMsgsRs").Element("ItemInventoryQueryRs").Attribute("statusMessage").Value}");
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
                    read.Close();
                    reader.Item1.Close();
                    conn.Close();


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
    }
    }

        
    





     
