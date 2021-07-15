using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
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
            string checkcommand = ($"SELECT pswd , salt , email , name FROM users WHERE email = '{inputUser.Value}'");
            SQLiteCommand command = new SQLiteCommand(checkcommand, conn);
            SQLiteDataReader SqlDataReader = command.ExecuteReader();
            SqlDataReader.Read();
            if (SqlDataReader.HasRows)
            {
                if (Site1.ComputeHash(SqlDataReader.GetString(0), SqlDataReader.GetString(1)) == Site1.ComputeHash(inputPass.Value, SqlDataReader.GetString(1)))
                {
                    Session["user"] = SqlDataReader.GetValue(3);
                    Session["id"] = SqlDataReader.GetString(2);
                    Response.Redirect("Default.aspx");
                }
                else alertMessageBox(this.Page, "Invalid Password");

            }
            else alertMessageBox(this.Page, "User Not Found");



            SqlDataReader.Close();
            conn.Close();
        }
        public static string ComputeHash(string passwordPlainText, string saltString)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(saltString);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(passwordPlainText);

            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            
            HashAlgorithm hash;

            hash = new SHA256Managed();

            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            return hashValue;
        }
    }
}