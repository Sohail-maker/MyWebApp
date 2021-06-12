using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace MyWebApp
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        public static void alertMessageBox(System.Web.UI.Page page, string strMsg)
        {
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alertMessage", "alert('" + strMsg + "')", true);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
          if (Session["user"] != null)
            {
                divLogin.Visible = false;
                lblusername.Text = "Logged in as: " + Session["user"].ToString();
            }
          else
            {

            }

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(database.connString);
            conn.Open();
            string checkcommand = ($"SELECT DECODE(`pswd`, 'MD5') AS `pswd` , `email` , `name` FROM `users` WHERE `email` = '{inputUser.Value}'");
            MySqlCommand command = new MySqlCommand(checkcommand, conn);
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            mySqlDataReader.Read();
            if (mySqlDataReader.HasRows)
            {
                if (mySqlDataReader.GetString(0) == inputPass.Value)
                {
                    Session["user"] = mySqlDataReader.GetValue(2);
                    Response.Redirect("Default.aspx");
                }
                else alertMessageBox(this.Page, "Invalid Password");

            }
            else alertMessageBox(this.Page, "User Not Found");



            mySqlDataReader.Close();
            conn.Close();
        }
    }
}