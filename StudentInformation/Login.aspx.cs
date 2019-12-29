using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentInformation
{
    public partial class Login : System.Web.UI.Page
    {
        string ConnectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["StudentRecordConnectionString"].ConnectionString;
        string Username;
        string Password;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void onLogin(object sender, EventArgs e)
        {
            Username = txtUsername.Text;
            Password = Encrypt.EncryptText(txtPassword.Text,Username);

            try
            {
                using (SqlConnection connectionobj = new SqlConnection(ConnectionStr))
                {
                    connectionobj.Open();

                    SqlCommand cmd = new SqlCommand("stpGetLoginInfo", connectionobj);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", SqlDbType.VarChar).Value = Username.Trim();
                    SqlDataReader reader = cmd.ExecuteReader();

                    reader.Read();

                    string actualPassword = reader["Passwd"].ToString();

                    if (Password.Equals(actualPassword))
                    {
                        Response.Redirect("~/StudentInfo.aspx?ID="+ Username);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Password is Invalid')", true);
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Username is Invalid')", true);
            }
            catch (Exception exception)
            {
                Response.Write(exception);
            }
        }

        protected void onSignup(object sender, EventArgs e)
        {
            Response.Redirect("~/RegistrationForm.aspx");
        }
    }
}