using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;
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
            SQLiteConnection conn = new SQLiteConnection(database.connString);
            conn.Open();
            string checkcommand = ($"SELECT DECODE(`pswd`, 'MD5') AS `pswd` , `email` , `name` FROM `users` WHERE `email` = '{inputUser.Value}'");
            SQLiteCommand command = new SQLiteCommand(checkcommand, conn);
            SQLiteDataReader SqlDataReader = command.ExecuteReader();
            SqlDataReader.Read();
            if (SqlDataReader.HasRows)
            {
                if (SqlDataReader.GetString(0) == inputPass.Value)
                {
                    Session["user"] = SqlDataReader.GetValue(2);
                    Response.Redirect("Default.aspx");
                }
                else alertMessageBox(this.Page, "Invalid Password");

            }
            else alertMessageBox(this.Page, "User Not Found");



            SqlDataReader.Close();
            conn.Close();
        }
    }
}