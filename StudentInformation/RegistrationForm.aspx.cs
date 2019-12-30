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

            if (IsUserExists(Username)) 
            {
                lblError.Visible = true;
                return;
            }

            lblError.Visible = false;
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
                catch (Exception exception)
                {
                    
                }
            }

            if (update > 0)
            {
                MessageBox("User successfully registered");
            }
        }

        private bool IsUserExists(string name)
        {
            bool exists = false;

            using (SqlConnection ConObj = new SqlConnection(ConnectionStr))
            {
                ConObj.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("stpUserExists", ConObj);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserName", SqlDbType.VarChar).Value = name.Trim();
                    exists = (int)cmd.ExecuteScalar() > 0;
                }
                catch (Exception exp)
                {
                    
                }
            }

            return exists;
        }

        private void MessageBox(string message)
        {
            Response.Write("<script language='javascript'>window.alert('" + message + "');window.location='Login.aspx';</script>");
        }

    }
}