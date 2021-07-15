using System;
using System.Xml;
using QBPOSXMLRPLib;
using System.Data.SQLite;

namespace MyWebApp
{

    public  partial class WebForm2 : System.Web.UI.Page

    {
        
        public RequestProcessor rp = new RequestProcessor();

        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void btnUpdataData_Click(object sender, EventArgs e)
        {
            
            lblError.Text = database.updatedatabase();
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            QueryData(txtQuery.Text);
        }

        public  void  QueryData(string searchInfo)
        {
            lblError.Text = null;
            string query = $"SELECT `itemNumber`, `Name`, `ALU`, `attribute`, `cost`, `lastReceived`, `onHandQty`, `orderCost`, `price`, `size`, `UPC`, `ALU2`, `UPC2`, `ALU3`, `UPC3`, `ALU4`, `UPC4`, `ALU5`, `UPC5` FROM `inventory` WHERE CONCAT( `itemNumber`, `Name`, `ALU`, `attribute`, `UPC`, `ALU2`, `UPC2`, `ALU3`, `UPC3`, `ALU4`, `UPC4`, `ALU5`, `UPC5` ) LIKE '%{searchInfo}%' LIMIT 1; ";
            SQLiteConnection conn = new SQLiteConnection(database.connString);
            conn.Open();
            SQLiteCommand command = new SQLiteCommand(query, conn);
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                reader.Read();

                if (reader.HasRows)
                {
                    lblitmNumber.Text = reader.GetString(0);
                    lblName.Text = reader.GetString(1);
                    lblPrice.Text = reader.GetDecimal(8).ToString();
                }
                else lblError.Text = "Not Found";


                
            }
            catch (Exception EX)
            {
                lblError.Text = EX.Message;
            }

            reader.Close();
            conn.Close();


        }
    }
}