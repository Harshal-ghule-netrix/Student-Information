using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentInformation
{
    public partial class StudentInfo : System.Web.UI.Page
    {
        string ConnectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["StudentRecordConnectionString"].ConnectionString;
        string Firstname;
        string Lastname;
        string Gender;
        string CreatedBy;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Display();
                AddValuesToCheckBoxLst();
            }

        }

        protected void onSubmit(object sender, EventArgs e)
        {
            InsertStudent();
            AddSubjects();
            Display();
        }

        private string GetCheckedRadioButton()
        {
            return rdolstgender.SelectedItem.Value;
        }

        private string[] GetCheckedData()
        {
            List<string> ckddata = new List<string>();

            for (int i = 0; i < cblSubjects.Items.Count; i++)
            {
                if (cblSubjects.Items[i].Selected)
                {
                    ckddata.Add(cblSubjects.Items[i].Text);
                }

            }
            return ckddata.ToArray();
        }

        private string[] GetUnCheckedData()
        {
            List<string> ckddata = new List<string>();

            for (int i = 0; i < cblSubjects.Items.Count; i++)
            {
                if (!cblSubjects.Items[i].Selected)
                {
                    ckddata.Add(cblSubjects.Items[i].Text);
                }

            }
            return ckddata.ToArray();
        }

        private void AddSubjects()
        {
            string[] subjects = GetCheckedData();

            using (SqlConnection connectionobj = new SqlConnection(ConnectionStr))
            {
                connectionobj.Open();

                foreach (string subjectname in subjects)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("stpInsertSubjectDetails", connectionobj);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SubjectName", SqlDbType.VarChar).Value = subjectname;
                        cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy.Trim();
                        cmd.ExecuteNonQuery();

                    }
                    catch (System.Data.SqlClient.SqlException exception)
                    {
                        Response.Write(exception);
                    }
                    catch (Exception exception)
                    {
                        Response.Write(exception);
                    }
                }
            }
        }

        private void InsertStudent()
        {
            int update = 0;
            Firstname = txtStudentFirstName.Text;
            Lastname = txtStudentLastName.Text;
            Gender = GetCheckedRadioButton();
            CreatedBy = Request.QueryString["ID"];

            using (SqlConnection connectionobj = new SqlConnection(ConnectionStr))
            {
                connectionobj.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("stpInsertStudentDetails", connectionobj);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", SqlDbType.VarChar).Value = Firstname.Trim();
                    cmd.Parameters.AddWithValue("@LastName", SqlDbType.VarChar).Value = Lastname.Trim();
                    cmd.Parameters.AddWithValue("@Gender", SqlDbType.Char).Value = Gender;
                    cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.VarChar).Value = CreatedBy.Trim();


                    update = cmd.ExecuteNonQuery();

                    if (update > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Student Record Inserted Successfully !')", true);
                    }
                }
                catch (System.Data.SqlClient.SqlException exception)
                {
                    Response.Write(exception);
                }
                catch (Exception exception)
                {
                    Response.Write(exception);
                }
            }
        }

        public void Display()
        {
            using (SqlConnection ConObj = new SqlConnection(ConnectionStr))
            {
                ConObj.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("stpShowStudentDetails", ConObj);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = null;
                    reader = cmd.ExecuteReader();

                    gvStudentRecord.DataSource = reader;
                    gvStudentRecord.DataBind();

                }
                catch (Exception exp)
                {
                    Response.Write(exp.Message);
                }

            }
        }

        private void AddValuesToCheckBoxLst()
        {
            using (SqlConnection ConObj = new SqlConnection(ConnectionStr))
            {
                ConObj.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("stpShowSubjects", ConObj);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;


                    SqlDataReader reader = null;
                    reader = cmd.ExecuteReader();

                    cblSubjects.DataSource = reader;
                    cblSubjects.DataTextField = "SubName";
                    cblSubjects.DataBind();

                }
                catch (Exception exp)
                {
                    Response.Write(exp.Message);
                }

            }
        }

        protected void GridViewRowDelete(object sender, GridViewDeleteEventArgs e)
        {


            using (SqlConnection ConObj = new SqlConnection(ConnectionStr))
            {
                ConObj.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("stpDeleteStudentRecord", ConObj);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudID", SqlDbType.Int).Value = gvStudentRecord.DataKeys[e.RowIndex].Value;

                    cmd.ExecuteNonQuery();


                }
                catch (Exception exception)
                {

                }

            }
            Display();

        }

        protected void GridViewRowEdit(object sender, GridViewEditEventArgs e)
        {
            gvStudentRecord.EditIndex = e.NewEditIndex;

            Label id = gvStudentRecord.Rows[e.NewEditIndex].FindControl("RollNo") as Label;
            TextBox firstname = gvStudentRecord.Rows[e.NewEditIndex].FindControl("FirstName") as TextBox;
            TextBox lastname = gvStudentRecord.Rows[e.NewEditIndex].FindControl("LastName") as TextBox;
            Label gender = gvStudentRecord.Rows[e.NewEditIndex].FindControl("Gender") as Label;
            TextBox subjects = gvStudentRecord.Rows[e.NewEditIndex].FindControl("SubName") as TextBox;

            txtStudentFirstName.Enabled = false;
            txtStudentLastName.Enabled = false;
            chkSelectAllSubjects.Checked = false;

            lblRollNo.Visible = true;
            lblRollNo.Text = id.Text;
            btnSubmit.Enabled = false;

            FillTextBox(firstname.Text, lastname.Text);
            TickRadioList(gender.Text);
            TickCheckBoxLst(subjects.Text);
            Display();

        }

        protected void GridViewRowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int rowIndex = e.RowIndex;

            UpdateStudentsDetails(rowIndex);
            UpdateSubjectsDetails(rowIndex);
            UpdateUncheckedSubjectDetails(rowIndex);

            gvStudentRecord.EditIndex = -1;
            Clear();
            Display();

        }

        private void UpdateStudentsDetails(int rowIndex)
        {
            string modifiedby = Request.QueryString["ID"];
            TextBox firstname = gvStudentRecord.Rows[rowIndex].FindControl("FirstName") as TextBox;
            TextBox lastname = gvStudentRecord.Rows[rowIndex].FindControl("LastName") as TextBox;
            string gender = GetCheckedRadioButton();

            using (SqlConnection ConObj = new SqlConnection(ConnectionStr))
            {
                ConObj.Open();

                try
                {
                    SqlCommand cmd = new SqlCommand("stpUpdateStudentDetails", ConObj);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudID", SqlDbType.Int).Value = gvStudentRecord.DataKeys[rowIndex].Value;
                    cmd.Parameters.AddWithValue("@FirstName", SqlDbType.VarChar).Value = firstname.Text;
                    cmd.Parameters.AddWithValue("@LastName", SqlDbType.VarChar).Value = lastname.Text;
                    cmd.Parameters.AddWithValue("@Gender", SqlDbType.Char).Value = gender;
                    cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.VarChar).Value = modifiedby;
                    cmd.ExecuteNonQuery();


                }
                catch (Exception exception)
                {
                    Response.AppendToLog(exception.Message);
                }

            }
        }

        private void UpdateSubjectsDetails(int rowIndex)
        {
            int studentid = Convert.ToInt32(gvStudentRecord.DataKeys[rowIndex].Value);
            string[] subjects = GetCheckedData();
            string modifiedby = Request.QueryString["ID"];
            string createdby = modifiedby;

            using (SqlConnection ConObj = new SqlConnection(ConnectionStr))
            {
                ConObj.Open();

                try
                {
                    foreach (string subjectname in subjects)
                    {
                        SqlCommand cmd = new SqlCommand("stpUpdateStudentSubjectsDetails", ConObj);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StudID", SqlDbType.Int).Value = studentid;
                        cmd.Parameters.AddWithValue("@SubName", SqlDbType.VarChar).Value = subjectname;
                        cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.Char).Value = createdby;
                        cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.VarChar).Value = modifiedby;
                        cmd.ExecuteNonQuery();

                    }
                }
                catch (Exception exception)
                {
                    Response.AppendToLog(exception.Message);
                }

            }
        }

        private void UpdateUncheckedSubjectDetails(int rowIndex)
        {
            int studentid = Convert.ToInt32(gvStudentRecord.DataKeys[rowIndex].Value);
            string[] subjects = GetUnCheckedData();
            string modifiedby = Request.QueryString["ID"];
            using (SqlConnection ConObj = new SqlConnection(ConnectionStr))
            {
                ConObj.Open();

                try
                {
                    foreach (string subjectname in subjects)
                    {
                        SqlCommand cmd = new SqlCommand("stpUpdateUncheckedStudentSubjects", ConObj);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StudID", SqlDbType.Int).Value = studentid;
                        cmd.Parameters.AddWithValue("@SubName", SqlDbType.VarChar).Value = subjectname;
                        cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.VarChar).Value = modifiedby;
                        cmd.ExecuteNonQuery();

                    }
                }
                catch (Exception exception)
                {
                    Response.AppendToLog(exception.Message);
                }

            }


        }

        protected void GridViewRowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvStudentRecord.EditIndex = -1;

            Clear();
            Display();
        }

        private void Clear()
        {
            txtStudentFirstName.Enabled = true;
            txtStudentLastName.Enabled = true;
            chkSelectAllSubjects.Checked = false;

            lblRollNo.Visible = false;
            btnSubmit.Enabled = true;
            txtStudentFirstName.Text = String.Empty;
            txtStudentLastName.Text = String.Empty;
            UnTickCheckBoxList();
            UnTickRadioList();
        }

        private void TickCheckBoxLst(string subjects)
        {
            UnTickCheckBoxList();
            foreach (string sub in subjects.Split(','))
            {
                string subject = sub.Trim();

                for (int i = 0; i < cblSubjects.Items.Count; i++)
                {
                    if ((cblSubjects.Items[i].Text).Trim().Equals(subject))
                    {
                        cblSubjects.Items[i].Selected = true;
                    }
                }
            }

        }

        private void TickAllCheckBoxLst()
        {
            for (int i = 0; i < cblSubjects.Items.Count; i++)
            {
                cblSubjects.Items[i].Selected = true;
            }
            cblSubjects.Enabled = false;
        }

        private void UnTickCheckBoxList()
        {
            for (int i = 0; i < cblSubjects.Items.Count; i++)
            {
                cblSubjects.Items[i].Selected = false;
            }
            cblSubjects.Enabled = true;
        }

        private void TickRadioList(string gender)
        {
            UnTickRadioList();
            for (int i = 0; i < rdolstgender.Items.Count; i++)
            {
                if (rdolstgender.Items[i].Text.Equals(gender.Trim()))
                {
                    rdolstgender.Items[i].Selected = true;
                }
            }
        }

        private void UnTickRadioList()
        {
            for (int i = 0; i < rdolstgender.Items.Count; i++)
            {
                rdolstgender.Items[i].Selected = false;
            }
        }

        private void FillTextBox(string fname, string lname)
        {
            txtStudentFirstName.Text = fname;
            txtStudentLastName.Text = lname;
        }

        protected void CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbk = sender as CheckBox;
           
            if (cbk.Checked)
            {
                TickAllCheckBoxLst();
            }
            else
            {
                UnTickCheckBoxList();
            }
        }
    }
}

