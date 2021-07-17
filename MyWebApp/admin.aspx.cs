using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;

namespace MyWebApp
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
            {
                divPassedit.Visible = false;
                divAddAccount.Visible = false;
            }
            else lblusername.Text = "You're logged in as: " + Session["user"].ToString();
        }



        protected void btnAddAccount_Click(object sender, EventArgs e)
        {

        }

        protected void btnSavePass_Click1(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                 SQLiteConnection conn = new SQLiteConnection(database.connString);
                 conn.Open();
                 SQLiteCommand cmd = conn.CreateCommand();
                 cmd.CommandText = ($"SELECT pswd , salt , email , name FROM users WHERE email = '{Session["id"].ToString()}'");
                 SQLiteDataReader reader = cmd.ExecuteReader();
                 reader.Read();
                 string key = reader.GetString(1);
                 string pass = reader.GetString(0);
                 reader.Close();
                 conn.Close();
                if (pass == Site1.ComputeHash(oldPass.Value, key))
                  {
                using (SQLiteConnection connection = new SQLiteConnection(database.connString))
                {
                    connection.Open();
                    string hash = Guid.NewGuid().ToString();
                    string cmdstring =  $"UPDATE users set pswd = '{Site1.ComputeHash(newPass.Value,hash)}', salt = '{hash}' WHERE email = '{Session["id"].ToString()}'; ";
                    System.Windows.Forms.MessageBox.Show(cmdstring);
                        try
                        {
                            SQLiteCommand cmd2 = new SQLiteCommand(cmdstring, connection);
                            cmd2.VerifyOnly();
                            cmd2.ExecuteNonQuery();

                        }
                        catch(Exception ex)
                        {
                            Site1.alertMessageBox(Page,ex.Message + ex.GetType());
                        }
                        Site1.alertMessageBox(Page, $"Password Changed ");
                }
                oldPass.Value = "";
                newPass.Value = "";
            }
            else Site1.alertMessageBox(Page, "Wrong Password");
            reader.Close();
            conn.Close();

        }
       }

        protected void btnAddAccount_Click1(object sender, EventArgs e)
        {
            string name = accountName.Value;
            string email = userName.Value;
            string pass = userPassword.Value;
            string salt = Guid.NewGuid().ToString();
            try
            {
                using(SQLiteConnection connection = new SQLiteConnection(database.connString))
                {
                    connection.Open();
                    using(SQLiteCommand cmd = new SQLiteCommand(connection))
                    {
                        cmd.CommandText = $"INSERT INTO users (email, name, pswd, salt) VALUES ('{email}','{name}','{pass}','{salt}')";
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                Site1.alertMessageBox(Page,"Account Added");
                
            }
            
            catch(Exception ex)
            {
                Site1.alertMessageBox(Page, ex.Message);
            }
            
        }
    }
}
