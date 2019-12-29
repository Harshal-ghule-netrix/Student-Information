using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace StudentInformation
{
    public partial class RegistrationForm : System.Web.UI.Page
    {
        string ConnectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["StudentRecordConnectionString"].ConnectionString;

        string Firstname;
        string Lastname;
        string Username;
        string Password;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Signupbtn(object sender, EventArgs e)
        {
            int update = 0;
            Firstname = txtFirstName.Text;
            Lastname = txtLastName.Text;
            Username = txtUsername.Text;
            Password = Encrypt.EncryptText(txtPassword.Text, Username);

           
            using (SqlConnection connectionobj = new SqlConnection(ConnectionStr))
            {
                connectionobj.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("stpInsertUser", connectionobj);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", SqlDbType.VarChar).Value = Firstname.Trim();
                    cmd.Parameters.AddWithValue("@LastName", SqlDbType.VarChar).Value = Lastname.Trim();
                    cmd.Parameters.AddWithValue("@UserName", SqlDbType.VarChar).Value = Username.Trim();
                    cmd.Parameters.AddWithValue("@Password", SqlDbType.VarChar).Value = Password.Trim();
                    cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.VarChar).Value = (Firstname + " " + Lastname).Trim();
                    
                    update = cmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException exception)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Username already exists !')", true);
                    Response.Write(exception);
                }
                catch (Exception exception)
                {
                    Response.Write(exception);
                    //txtFirstName.Text = exception.Message;
                }
            }

            if (update > 0)
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        
    }
}