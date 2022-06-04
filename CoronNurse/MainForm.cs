using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace CoronNurse
{
    public partial class MainForm : Form
    {     
        static private DataSet ds = new DataSet();
        static private DataSet ds1 = new DataSet();
        static private DataSet ds2 = new DataSet();
        static private DataSet ds3 = new DataSet();
        static private DataSet ds4 = new DataSet();
        static private DataSet ds5 = new DataSet();
        static private DataSet ds6 = new DataSet();
        static private DataSet ds7 = new DataSet();
        static private string doc_id;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (LoginForm.accType == "admin")
            {
                tabControl1.TabPages.Remove(ProfilePage);
                tabControl1.TabPages.Remove(MedRecPage);
                tabControl1.TabPages.Remove(DocAppoManagePage);
                tabControl1.TabPages.Remove(myAppoPage);
                btn_BookAnAppo.Visible = false;
            }
            else if (LoginForm.accType == "doctor")
            {
                tabControl1.TabPages.Remove(ManageNewsPage);
                tabControl1.TabPages.Remove(CentersManagePage);
                tabControl1.TabPages.Remove(UserManagePage);
                tabControl1.TabPages.Remove(MedRecPage);
                tabControl1.TabPages.Remove(myAppoPage);
                btn_BookAnAppo.Visible = false;
                datePicker_AppoDate.Value = DateTime.Now;
            }
            else if (LoginForm.accType == "patient")
            {
                tabControl1.TabPages.Remove(ManageNewsPage);
                tabControl1.TabPages.Remove(CentersManagePage);
                tabControl1.TabPages.Remove(UserManagePage);
                tabControl1.TabPages.Remove(DocAppoManagePage);
            }
        }

        private static DataSet Trans(DataSet dataset, string queryString)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, LoginForm.conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(adapter);
                adapter.Fill(dataset);
                return dataset;
            }
            catch (SqlException)
            {
                DialogResult confirmClose = MessageBox.Show("Cannot connect to the server\n The application will be shoutdown", "Warning", MessageBoxButtons.OK);
                if (confirmClose == DialogResult.OK)
                {
                    Application.Exit();
                }
                return null;
            }
        }
        private static void cmd(string queryString)
        {
            try
            {
                LoginForm.conn.Open();
                SqlCommand sqlcmd = new SqlCommand(queryString, LoginForm.conn);
                sqlcmd.ExecuteNonQuery();
                LoginForm.conn.Close();
            }
            catch (SqlException)
            {
                DialogResult confirmClose = MessageBox.Show("Cannot connect to the server\n The application will be shoutdown", "Warning", MessageBoxButtons.OK);
                if (confirmClose == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }
        private static bool CheckUsernameExist(string username)
        {
            string select = "select * from gue.CheckUname('" + username + "')";
            ds = new DataSet();
            Trans(ds, select);
            if (ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                return false;
            }
            else
                return true;
        }
        private static bool CheckAppoExist(string id, DateTime date, DateTime time)
        {
            string select = "select * from Doctors.CheckAppo(" + id + ",'" + date + "','" + time + "')";
            ds = new DataSet();
            Trans(ds, select);
            if (ds.Tables[0].Rows[0][0].ToString() == "0")
            {
                return false;
            }
            else
                return true;
        }
        private string useMD5Hash(string pw)
        {
            byte[] tmpSource;
            byte[] tmpHash;
            tmpSource = ASCIIEncoding.ASCII.GetBytes(pw);
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            //
            int i;
            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (i = 0; i < tmpHash.Length; i++)
            {
                sOutput.Append(tmpHash[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        ///////////////////////////////// ADMIN PAGES SECTION BEGIN
        ///////////////////////////////// ADMIN PAGES SECTION BEGIN
        ///////////////////////////////// ADMIN PAGES SECTION BEGIN
        ///////////////////////////////// ADMIN PAGES SECTION BEGIN

        private void EditDocPage_Enter(object sender, EventArgs e)
        {
            comBox_EditDoc_spec.Items.Clear();
            dgv_EditDoc.Rows.Clear();
            string select = "select * from admins.getDoctors()";
            ds4 = new DataSet();
            Trans(ds4, select);
            foreach (DataRow dr in ds4.Tables[0].Rows)
            {

                dgv_EditDoc.Rows.Add(dr["id"], dr["login_id"], dr["f_name"], dr["l_name"], dr["spec"]);
            }
            select = "select * from pub.getDocSpec()";
            ds = new DataSet();
            Trans(ds, select);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                comBox_EditDoc_spec.Items.Add(dr[0]);
            }
        }
        private void UserManagePage_Enter(object sender, EventArgs e)
        {
            EditDocPage_Enter(null, null);
        }

        private void dgv_EditDoc_SelectionChanged(object sender, EventArgs e)
        {
            txt_EditDoc_ID.Text = null;
            txt_EditDoc_LoginID.Text = null;
            txt_EditDoc_fname.Text = null;
            txt_EditDoc_lname.Text = null;
            txt_EditDoc_addr.Text = null;
            txt_EditDoc_mob.Text = null;
            txt_EditDoc_mail.Text = null;
            richTxt_EditDoc_details.Text = null;
            comBox_EditDoc_sex.Text = null;
            comBox_EditDoc_spec.Text = null;
            if (dgv_EditDoc.Rows.Count != 0)
            {
                txt_EditDoc_ID.Text = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][0].ToString();
                txt_EditDoc_LoginID.Text = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][1].ToString();
                txt_EditDoc_fname.Text = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][2].ToString();
                txt_EditDoc_lname.Text = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][3].ToString();
                comBox_EditDoc_spec.Text = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][4].ToString();
                txt_EditDoc_addr.Text = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][5].ToString();
                txt_EditDoc_mob.Text = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][6].ToString();
                txt_EditDoc_mail.Text = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][7].ToString();
                richTxt_EditDoc_details.Text = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][8].ToString();
                comBox_EditDoc_sex.Text = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][9].ToString();
                string dob = ds4.Tables[0].Rows[dgv_EditDoc.CurrentCell.RowIndex][10].ToString();
                datePicker_EditDoc_dob.Value = DateTime.ParseExact(dob, "yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        private void btn_EditDoc_Click(object sender, EventArgs e)
        {
            if (dgv_EditDoc.Rows.Count != 0)
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to edit the doctor information?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string query = ("EXEC admins.editDoc " +
                    "@id= " + txt_EditDoc_ID.Text.Trim() +
                    ",@login_id= " + txt_EditDoc_LoginID.Text.Trim() +
                    ",@first_name= N'" + txt_EditDoc_fname.Text.Trim() +
                    "',@last_name= N'" + txt_EditDoc_lname.Text.Trim() +
                    "',@spec= N'" + comBox_EditDoc_spec.Text.Trim() +
                    "',@addr= N'" + txt_EditDoc_addr.Text.Trim() +
                    "',@mobile= N'" + txt_EditDoc_mob.Text.Trim() +
                    "',@mail= N'" + txt_EditDoc_mail.Text.Trim() +
                    "',@dob= N'" + datePicker_EditDoc_dob.Text.Trim() +
                    "',@sex= N'" + comBox_EditDoc_sex.Text.Trim() +
                    "',@info= N'" + richTxt_EditDoc_details.Text + "'");

                    cmd(query);

                    EditDocPage_Enter(null, null);
                    MessageBox.Show("The doctor information has been edited successfully");
                }
            }
            else
                MessageBox.Show("There is no doctor to be edited");
        }

        private void btn_DelDoc_Click(object sender, EventArgs e)
        {
            if (dgv_EditDoc.Rows.Count != 0)
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete the selected doctor?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string query = ("EXEC admins.delDoc @id= " + txt_EditDoc_ID.Text);
                    cmd(query);

                    EditDocPage_Enter(null, null);
                    MessageBox.Show("The doctor has been deleted successfully");

                }
            }
            else
                MessageBox.Show("There is no doctor to be deleted");

        }
        private void btn_NewDoc_Click(object sender, EventArgs e)
        {
            comBox_NewDoc_spec.Items.Clear();
            groupBox4.Enabled = true;
            groupBox5.Enabled = true;
            btn_AddDoc.Enabled = true;
            btn_AddDoc.BackColor = Color.MediumTurquoise;
            btn_NewDoc.Enabled = false;
            btn_NewDoc.BackColor = Color.LightGray;
            txt_NewDoc_Password.Text = null;
            txt_NewDoc_Username.Text = null;
            richTxt_NewDoc_details.Text = null;
            txt_NewDoc_Mail.Text = null;
            txt_NewDoc_mob.Text = null;
            txt_NewDoc_Addr.Text = null;
            txt_NewDoc_lname.Text = null;
            txt_NewDoc_fname.Text = null;
            txt_NewDoc_LoginID.Text = null;
            txt_NewDoc_ID.Text = null;
            comBox_NewDoc_sex.SelectedIndex = 0;

            string select = "select * from pub.getDocSpec()";
            ds = new DataSet();
            Trans(ds, select);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                comBox_NewDoc_spec.Items.Add(dr[0]);
            }
            comBox_NewDoc_spec.Text = null;
        }
        private void btn_AddDoc_Click(object sender, EventArgs e)
        {
            if (CheckUsernameExist(txt_NewDoc_Username.Text.Trim()))
            {
                MessageBox.Show("Username is taken, please try another one", "Warning");
            }
            else
            {
                if (!string.IsNullOrEmpty(txt_NewDoc_Password.Text) &&
                    !string.IsNullOrEmpty(txt_NewDoc_Username.Text) &&
                    !string.IsNullOrEmpty(txt_NewDoc_mob.Text) &&
                    !string.IsNullOrEmpty(txt_NewDoc_Addr.Text) &&
                    !string.IsNullOrEmpty(comBox_NewDoc_spec.Text) &&
                    !string.IsNullOrEmpty(txt_NewDoc_lname.Text) &&
                    !string.IsNullOrEmpty(txt_NewDoc_fname.Text))
                {

                    DialogResult confirm = MessageBox.Show("Are you sure of all the information?", "Confirm", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        string query = ("EXEC admins.addDoc " +
              "@username= '" + txt_NewDoc_Username.Text.Trim() +
              "',@password= N'" + useMD5Hash(txt_NewDoc_Password.Text.Trim()) +
              "',@first_name= N'" + txt_NewDoc_fname.Text.Trim() +
              "',@last_name= N'" + txt_NewDoc_lname.Text.Trim() +
              "',@spec= N'" + comBox_NewDoc_spec.Text.Trim() +
              "',@addr= N'" + txt_NewDoc_Addr.Text.Trim() +
              "',@mobile= N'" + txt_NewDoc_mob.Text.Trim() +
              "',@mail= N'" + txt_NewDoc_Mail.Text.Trim() +
              "',@dob= N'" + datePicker_NewDoc_dob.Text.Trim() +
              "',@sex= N'" + comBox_NewDoc_sex.Text.Trim() +
              "',@info= N'" + richTxt_NewDoc_details.Text + "'");
                        cmd(query);

                        query = "SELECT * from admins.getNextID()";
                        ds = new DataSet();
                        Trans(ds, query);
                        //dataAdapter.Fill(ds);
                        txt_NewDoc_ID.Text = (int.Parse(ds.Tables[0].Rows[0][0].ToString())).ToString();
                        txt_NewDoc_LoginID.Text = (int.Parse(ds.Tables[0].Rows[0][1].ToString())).ToString();
                        groupBox4.Enabled = false;
                        groupBox5.Enabled = false;
                        btn_AddDoc.Enabled = false;
                        btn_AddDoc.BackColor = Color.LightGray;
                        btn_NewDoc.Enabled = true;
                        btn_NewDoc.BackColor = Color.MediumTurquoise;
                    }
                }
                else
                {
                    MessageBox.Show("A required field is empty, please fill all the required fields", "Warning");
                }
            }
        }
        private void NursesManagePage_Enter(object sender, EventArgs e)
        {
            string select = "select * from pub.getNurse()";

            ds = new DataSet();
            Trans(ds, select);
            dgv_EditNurses.DataSource = ds.Tables[0];
            foreach (DataGridViewColumn dc in dgv_EditNurses.Columns)
            {
                dc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void dgv_EditNurses_SelectionChanged(object sender, EventArgs e)
        {
            txt_Nurse_ID.Text = dgv_EditNurses[0, dgv_EditNurses.CurrentCell.RowIndex].Value.ToString();
            txt_Nurse_Name.Text = dgv_EditNurses[1, dgv_EditNurses.CurrentCell.RowIndex].Value.ToString();
            txt_Nurse_Mob.Text = dgv_EditNurses[2, dgv_EditNurses.CurrentCell.RowIndex].Value.ToString();
            richTxt_Nurse_Details.Text = dgv_EditNurses[3, dgv_EditNurses.CurrentCell.RowIndex].Value.ToString();
        }
        private void btn_NewNurse_Click(object sender, EventArgs e)
        {
            if (btn_NewNurse.Text == "Add new nurse")
            {
                txt_Nurse_ID.Visible = false;
                label50.Visible = false;
                txt_Nurse_ID.Text = null;
                txt_Nurse_Name.Text = null;
                txt_Nurse_Mob.Text = null;
                richTxt_Nurse_Details.Text = null;
                btn_AddNurse.Visible = true;
                btn_DelNurse.Visible = false;
                btn_EditNurse.Visible = false;
                btn_NewNurse.Text = "Back";
            }
            else if (btn_NewNurse.Text == "Back")
            {
                txt_Nurse_ID.Visible = true;
                label50.Visible = true;
                btn_AddNurse.Visible = false;
                btn_DelNurse.Visible = true;
                btn_EditNurse.Visible = true;
                btn_NewNurse.Text = "Add new nurse";
            }
        }

        private void btn_AddNurse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Nurse_Name.Text) || string.IsNullOrEmpty(txt_Nurse_Mob.Text))
            {
                MessageBox.Show("A required field is empty", "Warning");
            }
            else
            {
                DialogResult confirm = MessageBox.Show("Are you sure of all the information?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string query = ("EXEC admins.addNurse " +
          "@name= N'" + txt_Nurse_Name.Text.Trim() +
          "',@mobile= N'" + txt_Nurse_Mob.Text.Trim() +
          "',@info= N'" + richTxt_Nurse_Details.Text + "'");


                    cmd(query);
                    txt_Nurse_ID.Text = null;
                    txt_Nurse_Name.Text = null;
                    txt_Nurse_Mob.Text = null;
                    richTxt_Nurse_Details.Text = null;
                    btn_AddNurse.Visible = false;
                    txt_Nurse_ID.Visible = true;
                    label50.Visible = true;
                    btn_DelNurse.Visible = true;
                    btn_EditNurse.Visible = true;
                    btn_NewNurse.Text = "Add new nurse";
                    btn_NewNurse.Visible = true;

                    NursesManagePage_Enter(null, null);
                    MessageBox.Show("The Nurse has been added successfully");
                }
            }
        }

        private void btn_EditNurse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_Nurse_Name.Text) || (string.IsNullOrEmpty(txt_Nurse_Mob.Text)))
            {
                MessageBox.Show("A required field is empty", "Warning");
            }
            else
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to edit the information?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    btn_AddNurse.Visible = false;
                    string query = ("EXEC admins.editNurse " +
                          "@id= '" + txt_Nurse_ID.Text.Trim() +
          "',@name= N'" + txt_Nurse_Name.Text.Trim() +
          "',@mobile= N'" + txt_Nurse_Mob.Text.Trim() +
          "',@info= N'" + richTxt_Nurse_Details.Text + "'");

                    cmd(query);
                    NursesManagePage_Enter(null, null);
                    MessageBox.Show("The information has been edited successfully");

                }
            }
        }
        private void btn_DelNurse_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this nurse?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                string query = ("EXEC admins.delNurse @id= " + txt_Nurse_ID.Text);

                cmd(query);
                NursesManagePage_Enter(null, null);
                MessageBox.Show("The nurse has been deleted successfully");

            }
        }

        private void DelPatPage_Enter(object sender, EventArgs e)
        {
            dgv_DelPatient.Rows.Clear();
            string select = "select * from admins.getPatient()";
            ds3 = new DataSet();
            Trans(ds3, select);
            foreach (DataRow dr in ds3.Tables[0].Rows)
            {
                dgv_DelPatient.Rows.Add(dr["ID"], dr["Login ID"], dr["First Name"], dr["Last Name"]);
            }
        }
        private void dgv_DelPatient_SelectionChanged(object sender, EventArgs e)
        {
            txt_DelPat_ID.Text = null;
            txt_DelPat_LoginID.Text = null;
            txt_DelPat_FullName.Text = null;
            txt_DelPat_dob.Text = null;
            txt_DelPat_Sex.Text = null;
            richTxt_DelPat_MedRec.Text = null;
            if (dgv_DelPatient.Rows.Count != 0)
            {
                txt_DelPat_ID.Text = ds3.Tables[0].Rows[dgv_DelPatient.CurrentCell.RowIndex][0].ToString();
                txt_DelPat_LoginID.Text = ds3.Tables[0].Rows[dgv_DelPatient.CurrentCell.RowIndex][1].ToString();
                txt_DelPat_FullName.Text = ds3.Tables[0].Rows[dgv_DelPatient.CurrentCell.RowIndex][2].ToString()
                + "   " + ds3.Tables[0].Rows[dgv_DelPatient.CurrentCell.RowIndex][3].ToString();
                txt_DelPat_dob.Text = ds3.Tables[0].Rows[dgv_DelPatient.CurrentCell.RowIndex][4].ToString();
                txt_DelPat_Sex.Text = ds3.Tables[0].Rows[dgv_DelPatient.CurrentCell.RowIndex][5].ToString();
                txt_DelPat_Mob.Text = ds3.Tables[0].Rows[dgv_DelPatient.CurrentCell.RowIndex][6].ToString();
                txt_DelPat_Mail.Text = ds3.Tables[0].Rows[dgv_DelPatient.CurrentCell.RowIndex][7].ToString();
                richTxt_DelPat_MedRec.Text = ds3.Tables[0].Rows[dgv_DelPatient.CurrentCell.RowIndex][8].ToString();
            }
        }
        private void btn_DelPatient_Click(object sender, EventArgs e)
        {
            if (dgv_DelPatient.Rows.Count != 0)
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete the selected patient?", "Warning", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string id = dgv_DelPatient[0, dgv_DelPatient.CurrentCell.RowIndex].Value.ToString();
                    string query = ("EXEC admins.delPat @id= " + id);
                    cmd(query);
                    DelPatPage_Enter(null, null);
                    MessageBox.Show("The selected patient has been deleted successfully");
                }
            }
            else
                MessageBox.Show("There is no patient to be deleted");
        }

        private void CentersManagePage_Enter(object sender, EventArgs e)
        {
            dgv_ManageCenters.Rows.Clear();
            string select = "select * from admins.getCenter()";
            ds = new DataSet();
            Trans(ds, select);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                dgv_ManageCenters.Rows.Add(dr["ID"], dr["Name"], dr["Area"], dr["Phone"], dr["Type"], dr["Details"]);
            }
        }
        private void dgv_ManageCenters_SelectionChanged(object sender, EventArgs e)
        {  
            btn_AddCenter.Visible = false;
            txt_ManCenters_ID.Text = dgv_ManageCenters[0, dgv_ManageCenters.CurrentCell.RowIndex].Value.ToString();
            txt_ManCenters_Name.Text = dgv_ManageCenters[1, dgv_ManageCenters.CurrentCell.RowIndex].Value.ToString();
            comBox_ManCenters_Addr.Text = dgv_ManageCenters[2, dgv_ManageCenters.CurrentCell.RowIndex].Value.ToString();
            txt_ManCenters_Mob.Text = dgv_ManageCenters[3, dgv_ManageCenters.CurrentCell.RowIndex].Value.ToString();
            comBox_ManCenters_Type.SelectedItem = dgv_ManageCenters[4, dgv_ManageCenters.CurrentCell.RowIndex].Value.ToString();
            richTxt_ManCenters_Details.Text = dgv_ManageCenters[5, dgv_ManageCenters.CurrentCell.RowIndex].Value.ToString();       
        }

        private void btn_NewCenter_Click(object sender, EventArgs e)
        {
            if (btn_NewCenter.Text == "Add new center")
            {
                comBox_ManCenters_Type.SelectedIndex = 0;
                txt_ManCenters_ID.Text = null;
                txt_ManCenters_Name.Text = null;
                txt_ManCenters_Mob.Text = null;
                richTxt_ManCenters_Details.Text = null;
                comBox_ManCenters_Addr.Text = null;
                btn_AddCenter.Visible = true;
                btn_DelCenter.Visible = false;
                btn_EditCenter.Visible = false;
                btn_NewCenter.Text = "Back";
            }
            else if (btn_NewCenter.Text == "Back")
            {
                dgv_ManageCenters.Rows[1].Selected = true;
                btn_NewCenter.Text = "Add new center";
                btn_DelCenter.Visible = true;
                btn_EditCenter.Visible = true;
                dgv_ManageCenters_SelectionChanged(null, null);
            }
        }

        private void btn_AddCenter_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure of all the information?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                string query = ("EXEC admins.addCen " +
      "@name= N'" + txt_ManCenters_Name.Text.Trim() +
      "',@location= N'" + comBox_ManCenters_Addr.Text.Trim() +
      "',@mobile= N'" + txt_ManCenters_Mob.Text.Trim() +
      "',@type= N'" + comBox_ManCenters_Type.SelectedItem +
      "',@info= N'" + richTxt_ManCenters_Details.Text + "'");

                cmd(query);
                txt_ManCenters_ID.Text = null;
                txt_ManCenters_Name.Text = null;
                txt_ManCenters_Mob.Text = null;
                richTxt_ManCenters_Details.Text = null;
                comBox_ManCenters_Type.Text = null;
                comBox_ManCenters_Addr.Text = null;
                btn_AddCenter.Visible = false;
                btn_DelCenter.Visible = true;
                btn_EditCenter.Visible = true;
                btn_NewCenter.Text = "Add new center";
                CentersManagePage_Enter(null, null);
                MessageBox.Show("The center has been added successfully");
            }
        }
        private void btn_EditCenter_Click(object sender, EventArgs e)
        {
            if (dgv_ManageCenters.Rows.Count != 0)
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to edit the selected center information?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string query = ("EXEC admins.editCen " +
                  "@id= " + txt_ManCenters_ID.Text.Trim() +
                  ",@name= N'" + txt_ManCenters_Name.Text.Trim() +
                  "',@addr= N'" + comBox_ManCenters_Addr.Text +
                  "',@mobile= N'" + txt_ManCenters_Mob.Text.Trim() +
                  "',@type= N'" + comBox_ManCenters_Type.SelectedItem +
                  "',@details= N'" + richTxt_ManCenters_Details.Text + "'");
                    cmd(query);
                    CentersManagePage_Enter(null, null);
                    MessageBox.Show("The center information has been edited successfully");
                }
            }
            else
                MessageBox.Show("There is no center to be edited");
        }

        private void btn_DelCenter_Click(object sender, EventArgs e)
        {
            if (dgv_ManageCenters.Rows.Count != 0)
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete the selected center?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string query = ("EXEC admins.delCen @id= " + txt_ManCenters_ID.Text);
                    cmd(query);
                    txt_ManCenters_ID.Text = null;
                    txt_ManCenters_Name.Text = null;
                    txt_ManCenters_Mob.Text = null;
                    richTxt_ManCenters_Details.Text = null;
                    comBox_ManCenters_Type.Text = null;
                    comBox_ManCenters_Addr.Text = null;
                    CentersManagePage_Enter(null, null);
                    MessageBox.Show("The center has been deleted successfully");
                }
            }
            else
                MessageBox.Show("There is no center to be deleted");
        }

        private void ManageNewsPage_Enter(object sender, EventArgs e)
        {
            dgv_ManageNews.Rows.Clear();
            string select = "select * from admins.getInfo()";
            ds7 = new DataSet();
            Trans(ds7, select);
            foreach (DataRow dr in ds7.Tables[0].Rows)
            {
                dgv_ManageNews.Rows.Add(dr["ID"], dr["Title"], dr["Date"], dr["Type"]);
            }
        }

        private void dgv_ManageNews_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_ManageNews.Rows.Count != 0)
            {
                txt_ManNews_Title.Text = dgv_ManageNews[1, dgv_ManageNews.CurrentCell.RowIndex].Value.ToString();
                datePicker_ManNews_Date.Value = DateTime.Parse(dgv_ManageNews[2, dgv_ManageNews.CurrentCell.RowIndex].Value.ToString());
                comBox_ManNews_Type.SelectedItem = dgv_ManageNews[3, dgv_ManageNews.CurrentCell.RowIndex].Value.ToString();
                richTxt_ManNews_Data.Text = ds7.Tables[0].Rows[dgv_ManageNews.CurrentCell.RowIndex][4].ToString();
            }
        }

        private void btn_NewNews_Click(object sender, EventArgs e)
        {
            if (btn_NewNews.Text == "Add news/instructions")
            {
                comBox_ManNews_Type.SelectedIndex = 0;
                datePicker_ManNews_Date.Value = DateTime.Now;
                txt_ManNews_Title.Text = null;
                richTxt_ManNews_Data.Text = null;
                btn_DelNews.Visible = false;
                btn_EditNews.Visible = false;
                btn_AddNews.Visible = true;
                btn_NewNews.Text = "Back";
            }
            else if (btn_NewNews.Text == "Back")
            {
                //  dataGridView12.Rows[1].Selected = true;
                btn_DelNews.Visible = true;
                btn_EditNews.Visible = true;
                btn_AddNews.Visible = false;
                btn_NewNews.Text = "Add news/instructions";
                dgv_ManageNews_SelectionChanged(null, null);
            }
        }

        private void btn_AddNews_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure of all the information?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                string query = ("EXEC admins.addInfo " +
      "@title= N'" + txt_ManNews_Title.Text.Trim() +
      "',@date= N'" + datePicker_ManNews_Date.Text.Trim() +
      "',@data= N'" + richTxt_ManNews_Data.Text.Trim() +
      "',@type= N'" + comBox_ManNews_Type.SelectedItem + "'");
                cmd(query);
                txt_ManNews_Title.Text = null;
                richTxt_ManNews_Data.Text = null;
                btn_DelNews.Visible = true;
                btn_EditNews.Visible = true;
                btn_AddNews.Visible = false;
                btn_NewNews.Text = "Add news/instructions";
                ManageNewsPage_Enter(null, null);
                MessageBox.Show("The news has been added successfully");
            }
        }

        private void btn_DelNews_Click(object sender, EventArgs e)
        {
            if (dgv_ManageNews.Rows.Count != 0)
            {
                DialogResult confirm = MessageBox.Show("Are you sure of deleted the selected new?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string query = ("EXEC admins.delInfo @id= " + dgv_ManageNews[0, dgv_ManageNews.CurrentCell.RowIndex].Value.ToString());
                    cmd(query);
                    txt_ManNews_Title.Text = null;
                    richTxt_Nurse_Details.Text = null;
                    ManageNewsPage_Enter(null, null);
                    MessageBox.Show("The news has been deleted successfully");
                }
            }
            else
                MessageBox.Show("There is no news to be deleted");
        }

        private void btn_EditNews_Click(object sender, EventArgs e)
        {
            if (dgv_ManageNews.Rows.Count != 0)
            {
                DialogResult confirm = MessageBox.Show("Are you sure of edit the selected news information?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string query = ("EXEC admins.editInfo " +
                  "@id= " + dgv_ManageNews[0, dgv_ManageNews.CurrentCell.RowIndex].Value.ToString() +
                  ",@title= N'" + txt_ManNews_Title.Text.Trim() +
                  "',@date= N'" + datePicker_ManNews_Date.Text +
                  "',@data= N'" + richTxt_ManNews_Data.Text +
                  "',@type= N'" + comBox_ManNews_Type.SelectedItem + "'");
                    cmd(query);
                    ManageNewsPage_Enter(null, null);
                    MessageBox.Show("The news information has been edited successfully");
                }
            }
            else
                MessageBox.Show("There is no news to be edited");
        }

        ///////////////////////////////// ADMIN PAGES SECTION END
        ///////////////////////////////// ADMIN PAGES SECTION END
        ///////////////////////////////// ADMIN PAGES SECTION END
        ///////////////////////////////// ADMIN PAGES SECTION END


        ///////////////////////////////////////////////////////////////////////////////////////////////////


        ///////////////////////////Doctors and Patients PAGES SECTION BEGIN
        ///////////////////////////Doctors and Patients PAGES SECTION BEGIN
        ///////////////////////////Doctors and Patients PAGES SECTION BEGIN

        private void ProfilePage_Enter(object sender, EventArgs e)
        {
            string query;
            if (LoginForm.accType == "doctor")
            {
                label14.Visible = true;
                richTxt_Profile_DocDetails.Visible = true;
                btn_EditDetails.Visible = true;
                query = "select * from Doctors.getDocProfile(" + LoginForm.id + ")";
                ds = new DataSet();
                Trans(ds, query);
                txt_Profile_Mail.Text = ds.Tables[0].Rows[0][7].ToString();
                txt_Profile_Mob.Text = ds.Tables[0].Rows[0][6].ToString();
                txt_Profile_dob.Text = ds.Tables[0].Rows[0][10].ToString();
                txt_Profile_Sex.Text = ds.Tables[0].Rows[0][9].ToString();
                richTxt_Profile_DocDetails.Text = ds.Tables[0].Rows[0][8].ToString();
            }
            if (LoginForm.accType == "patient")
            {
                query = "select * from pub.getPatProfile(" + LoginForm.id + ")";
                ds = new DataSet();
                Trans(ds, query);
                txt_Profile_Mail.Text = ds.Tables[0].Rows[0][6].ToString();
                txt_Profile_Mob.Text = ds.Tables[0].Rows[0][4].ToString();
                txt_Profile_dob.Text = ds.Tables[0].Rows[0][8].ToString();
                txt_Profile_Sex.Text = ds.Tables[0].Rows[0][7].ToString();

            }
            txt_Profile_ID.Text = LoginForm.id;
            txt_Profile_FullName.Text = ds.Tables[0].Rows[0][2].ToString()
                + "  " + ds.Tables[0].Rows[0][3].ToString();
        }
        private void btn_NewPass_Click(object sender, EventArgs e)
        {
            grouBox_ChPass.Visible = true;
            panel3.Visible = false;
        }
        private void btn_EditPass_Click(object sender, EventArgs e)
        {
            if (txt_NewPassword.Text == txt_ConfirmPass.Text)
            {
                if (txt_NewPassword.Text.Length < 8)
                {
                    MessageBox.Show("Password is short, it must be at least 8 character", "Warning");
                }
                else
                {
                    DialogResult confirm = MessageBox.Show("Are you sure you want to change the password?", "Confirm", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        string query = "EXEC pub.ChPass " +
                            "@log_id = '" + LoginForm.logid +
                            "',@password= N'" + useMD5Hash(txt_NewPassword.Text) + "'";
                        cmd(query);
                        grouBox_ChPass.Visible = false;
                        panel3.Visible = true;
                        MessageBox.Show("Password has been changed successfully");
                    }
                }
            }
            else
                MessageBox.Show("Passwords do NOT match");
        }
        private void btn_ProfileBack_Click(object sender, EventArgs e)
        {
            grouBox_ChPass.Visible = false;
            panel3.Visible = true;
        }


        private void btn_EditMail_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to change the email?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                if (LoginForm.accType == "doctor")
                {
                    string query = "EXEC Doctors.chMail " +
                        "@id = '" + LoginForm.id +
                        "',@mail= N'" + txt_Profile_Mail.Text.Trim() + "'";
                    cmd(query);
                    MessageBox.Show("Email has been edited successfully");
                }
                if (LoginForm.accType == "patient")
                {
                    string query = "EXEC pub.chMail " +
                        "@id = '" + LoginForm.id +
                        "',@mail= N'" + txt_Profile_Mail.Text.Trim() + "'";
                    cmd(query);
                    MessageBox.Show("Email has been changed successfully");
                }
            }
        }
        private void btn_EditMob_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to change the mobile?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                if (LoginForm.accType == "doctor")
                {
                    string query = "EXEC Doctors.chMob " +
                        "@id = '" + LoginForm.id +
                        "',@mob= N'" + txt_Profile_Mob.Text.Trim() + "'";
                    cmd(query);
                    MessageBox.Show("Mobile has been changed successfully");
                }
                if (LoginForm.accType == "patient")
                {
                    string query = "EXEC pub.chMob " +
                        "@id = '" + LoginForm.id +
                        "',@mob= N'" + txt_Profile_Mob.Text.Trim() + "'";
                    cmd(query);
                    MessageBox.Show("Mobile has been changed successfully");
                }
            }
        }
        private void btn_EditDetails_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to edit the additional information?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                string query = "EXEC Doctors.chInfo " +
                    "@id = '" + LoginForm.id +
                    "',@info= N'" + richTxt_Profile_DocDetails.Text + "'";
                cmd(query);
                MessageBox.Show("The additional information has been edited successfully");
            }
        }
        /////////////////////////// Doctors and Patients PAGES SECTION END
        /////////////////////////// Doctors and Patients PAGES SECTION END
        /////////////////////////// Doctors and Patients PAGES SECTION END


        ///////////////////////////////////////////////////////////////////////////////////////////////////


        //////////////////// Only Doctors  PAGES SECTION BEGIN
        //////////////////// Only Doctors  PAGES SECTION BEGIN
        //////////////////// Only Doctors  PAGES SECTION BEGIN
        private void DocAppoManagePage_Enter(object sender, EventArgs e)
        {
            dgv_ManageDocAppo.DataSource = null;
            string select = "select * from Doctors.getApp(" + LoginForm.logid + ")";
            ds = new DataSet();
            Trans(ds, select);
            dgv_ManageDocAppo.DataSource = ds.Tables[0];
            if (dgv_ManageDocAppo.Rows.Count != 0)
            {
                DataView view = ds.Tables[0].DefaultView;
                view.Sort = "Date,Time asc";
                dgv_ManageDocAppo.DataSource = view;
            }
            foreach (DataGridViewColumn dc in dgv_ManageDocAppo.Columns)
            {
                dc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private async void dgv_ManageDocAppo_SelectionChanged(object sender, EventArgs e)
        {
            string pat_id = dgv_ManageDocAppo[1, dgv_ManageDocAppo.CurrentCell.RowIndex].Value.ToString();
            if (!string.IsNullOrEmpty(pat_id))
            {
                string select = "select * from Doctors.getPat('" + pat_id + "')";
                ds6 = new DataSet();
                Trans(ds6, select);

                txt_PatDeta_Name.Text = ds6.Tables[0].Rows[0][1].ToString() + "  " + ds6.Tables[0].Rows[0][2].ToString();
                txt_PatDeta_Mob.Text = ds6.Tables[0].Rows[0][3].ToString();
                richTxt_PatDeta_MedRecord.Text = ds6.Tables[0].Rows[0][4].ToString();
                txt_PatDeta_Mail.Text = ds6.Tables[0].Rows[0][5].ToString();
                txt_PatDeta_Sex.Text = ds6.Tables[0].Rows[0][6].ToString();
                int age;
                age = int.Parse(DateTime.Now.Year.ToString()) - (int.Parse(ds6.Tables[0].Rows[0][7].ToString()));

                txt_PatDeta_Age.Text = age.ToString(); //

            }
            else if (string.IsNullOrEmpty(pat_id))
            {
                txt_PatDeta_Name.Text = null;
                txt_PatDeta_Mob.Text = null;
                richTxt_PatDeta_MedRecord.Text = null;
                txt_PatDeta_Mail.Text = null;
                txt_PatDeta_Sex.Text = null;
                txt_PatDeta_Age.Text = null;
            }
        }

        private void btn_DocNewApp_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            panel4.Visible = true;
            btn_DocAddOrEditApp.Text = "Add";
            btn_DocNewApp.Visible = false;
            btn_DocEditApp.Visible = false;
            btn_DocDelApp.Visible = false;
        }

        private void btn_DocEditApp_Click(object sender, EventArgs e)
        {
            if (dgv_ManageDocAppo.Rows.Count != 0)
            {
                string s = dgv_ManageDocAppo[4, dgv_ManageDocAppo.CurrentCell.RowIndex].Value.ToString();
                if (s == "False")
                {
                    groupBox2.Visible = false;
                    panel4.Visible = true;
                    btn_DocAddOrEditApp.Text = "Edit";
                    datePicker_AppoDate.Value = Convert.ToDateTime(dgv_ManageDocAppo[2, dgv_ManageDocAppo.CurrentCell.RowIndex].Value.ToString());
                    datePicker_AppoTime.Value = Convert.ToDateTime(dgv_ManageDocAppo[3, dgv_ManageDocAppo.CurrentCell.RowIndex].Value.ToString());
                    btn_DocNewApp.Visible = false;
                    btn_DocEditApp.Visible = false;
                    btn_DocDelApp.Visible = false;
                }
                else if (s == "True")
                    MessageBox.Show("You cannot delete a booked appointment, contact the patient to cancel it");
            }
            else
            {
                MessageBox.Show("Select an appointment to edit");
            }
        }

        private void btn_DocDelApp_Click(object sender, EventArgs e)
        {
            if (dgv_ManageDocAppo.Rows.Count != 0)
            {
                string s = dgv_ManageDocAppo[4, dgv_ManageDocAppo.CurrentCell.RowIndex].Value.ToString();
                if (s == "False")
                {
                    DialogResult confirm = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        string appid = dgv_ManageDocAppo[0, dgv_ManageDocAppo.CurrentCell.RowIndex].Value.ToString();
                        string query = ("EXEC  Doctors.delAppo @id= " + appid);

                        cmd(query);
                        DocAppoManagePage_Enter(null, null);
                        MessageBox.Show("The appointment has been deleted successfully");

                    }
                }
                else if (s == "True")
                    MessageBox.Show("You cannot delete a booked appointment, contact the paitent to cancel it");
            }
            else
            {
                MessageBox.Show("Select an appointment to delete");
            }
        }

        private void btn_DocAddOrEditApp_Click(object sender, EventArgs e)
        {
            if (btn_DocAddOrEditApp.Text == "Add")
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    if (CheckAppoExist(LoginForm.logid, datePicker_AppoDate.Value, datePicker_AppoTime.Value))
                    {
                        MessageBox.Show("You have a scheduled appointment on the same date and time, please choose another option", "Warning");
                    }
                    else
                    {
                        string query = ("EXEC Doctors.addAppo @id= " + LoginForm.logid + ", @date= '" + datePicker_AppoDate.Value + "', @time= '" + datePicker_AppoTime.Value + "'");
                        cmd(query);
                        DocAppoManagePage_Enter(null, null);
                        MessageBox.Show("The appointment has been added successfully");
                    }
                }
            }
            else if (btn_DocAddOrEditApp.Text == "Edit")
            {
                string appid = dgv_ManageDocAppo[0, dgv_ManageDocAppo.CurrentCell.RowIndex].Value.ToString();
                DialogResult confirm = MessageBox.Show("Are you sure you want to edit this appointment?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    if (CheckAppoExist(LoginForm.logid, datePicker_AppoDate.Value, datePicker_AppoTime.Value))
                    {
                        MessageBox.Show("You have a scheduled appointment on the same date and time, please choose another option", "Warning");
                    }
                    else
                    {
                        string query = ("EXEC  Doctors.editAppo @id= " + appid + ", @date= '" + datePicker_AppoDate.Value + "', @time= '" + datePicker_AppoTime.Value + "'");
                        cmd(query);
                        groupBox2.Visible = true;
                        panel4.Visible = false;
                        btn_DocNewApp.Visible = true;
                        btn_DocEditApp.Visible = true;
                        btn_DocDelApp.Visible = true;
                        DocAppoManagePage_Enter(null, null);
                        MessageBox.Show("The information has been edited successfully");
                    }
                }
            }
        }

        private void btn_DocAppManageBack_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
            panel4.Visible = false;
            btn_DocNewApp.Visible = true;
            btn_DocEditApp.Visible = true;
            btn_DocDelApp.Visible = true;
        }
        //////////////////// Only Doctors  PAGES SECTION END
        //////////////////// Only Doctors  PAGES SECTION END
        //////////////////// Only Doctors  PAGES SECTION END


        ///////////////////////////////////////////////////////////////////////////////////////////////////


        /////////////// Only Patients  PAGES SECTION BEGIN
        /////////////// Only Patients  PAGES SECTION BEGIN
        /////////////// Only Patients  PAGES SECTION BEGIN

        private void MedRecPage_Enter(object sender, EventArgs e)
        {
            string query = "select * from pub.getMedRec(" + LoginForm.id + ")";
            ds = new DataSet();
            Trans(ds, query);
            richTxt_PatientMedRecord.Text = ds.Tables[0].Rows[0][0].ToString();
        }
        private void btn_EditMedRec_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to edit the medical record?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                string query = ("EXEC pub.editMedRec @id= " + LoginForm.id + ",@med_record= N'" + richTxt_PatientMedRecord.Text + "'");
                cmd(query);
                MessageBox.Show("Medical record has been edited successfully!");
            }
        }

        private void myAppoPage_Enter(object sender, EventArgs e)
        {
            dgv_PatientAppo.DataSource = null;
            txt_DocDeta_Name.Text = null;
            txt_DocDeta_addr.Text = null;
            txt_DocDeta_Sex.Text = null;
            txt_DocDeta_Mail.Text = null;
            txt_DocDeta_Mob.Text = null;
            txt_DocDeta_Age.Text = null;
            richTxt_DocDeta_Details.Text = null;
            string select = "select * from pub.getMyApp('" + LoginForm.id + "');";
            ds = new DataSet();
            Trans(ds, select);
            dgv_PatientAppo.DataSource = ds.Tables[0];
            foreach (DataGridViewColumn dc in dgv_PatientAppo.Columns)
            {
                dc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            if (dgv_PatientAppo.Rows.Count != 0)
            {
                DataView view = ds.Tables[0].DefaultView;
                view.Sort = "Date, Time ASC";
                dgv_PatientAppo.DataSource = view;
            }
        }

        private void dgv_PatientAppo_SelectionChanged(object sender, EventArgs e)
        {
            string doc_id = dgv_PatientAppo[1, dgv_PatientAppo.CurrentCell.RowIndex].Value.ToString();
            if (!string.IsNullOrEmpty(doc_id))
            {

                string select = "select * from pub.getDocInfo('" + doc_id + "')";
                ds5 = new DataSet();
                Trans(ds5, select);
                string name = ds5.Tables[0].Rows[0][0].ToString() + "  " + ds5.Tables[0].Rows[0][1].ToString();
                txt_DocDeta_Name.Text = name;
                txt_DocDeta_addr.Text = ds5.Tables[0].Rows[0][2].ToString();
                txt_DocDeta_Sex.Text = ds5.Tables[0].Rows[0][4].ToString();
                txt_DocDeta_Mail.Text = ds5.Tables[0].Rows[0][5].ToString();
                txt_DocDeta_Mob.Text = ds5.Tables[0].Rows[0][6].ToString();
                richTxt_DocDeta_Details.Text = ds5.Tables[0].Rows[0][7].ToString();
                int age;
                age = int.Parse(DateTime.Now.Year.ToString()) - (int.Parse(ds5.Tables[0].Rows[0][3].ToString()));
                txt_DocDeta_Age.Text = age.ToString();
            }
            else if (string.IsNullOrEmpty(doc_id))
            {
                txt_DocDeta_Name.Text = null;
                txt_DocDeta_addr.Text = null;
                txt_DocDeta_Sex.Text = null;
                txt_DocDeta_Mail.Text = null;
                txt_DocDeta_Mob.Text = null;
                txt_DocDeta_Age.Text = null;
                richTxt_DocDeta_Details.Text = null;
            }
        }

        private void btn_CancelAppo_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure you want to delete the selected appointment?", "Confirm", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                if (dgv_PatientAppo.Rows.Count != 0)
                {
                    string appid = dgv_PatientAppo[0, dgv_PatientAppo.CurrentCell.RowIndex].Value.ToString();
                    string query = "EXEC pub.cancelAppo " +
            "@appo_id = '" + appid + "'";
                    cmd(query);

                    txt_DocDeta_Name.Text = null;
                    txt_DocDeta_addr.Text = null;
                    txt_DocDeta_Sex.Text = null;
                    txt_DocDeta_Mail.Text = null;
                    txt_DocDeta_Mob.Text = null;
                    txt_DocDeta_Age.Text = null;
                    richTxt_DocDeta_Details.Text = null;
                    myAppoPage_Enter(null, null);
                    MessageBox.Show("The appointment has been deleted successfully");
                }
                else
                    MessageBox.Show("Select an appointment to be deleted");
            }
        }
        /////////////// Only Patients  PAGES SECTION END
        /////////////// Only Patients  PAGES SECTION END
        /////////////// Only Patients  PAGES SECTION END


        ///////////////////////////////////////////////////////////////////////////////////////////////////


        //////////SHARED PAGES SECTION BEGIN
        //////////SHARED PAGES SECTION BEGIN
        //////////SHARED PAGES SECTION BEGIN
        private void DocAppoPage_Enter(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            dgv_DocList.Rows.Clear();
            comBox_DocSpec.Items.Clear();
            string select = "select * from pub.getDocSpec()";
            ds = new DataSet();
            Trans(ds, select);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                comBox_DocSpec.Items.Add(dr[0]);
            }
        }

        private void DocAppoPage_Leave(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            dgv_DocList.Rows.Clear();
        }

        private void comBox_DocSpec_SelectedValueChanged(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            dgv_DocList.Rows.Clear();
            string select = "select * from pub.getDocList(N'" + comBox_DocSpec.SelectedItem + "');";
            ds2 = new DataSet();
            Trans(ds2, select);
            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                dgv_DocList.Rows.Add(dr["ID"], dr["First Name"], dr["Last Name"], dr["Address"]);
            }
        }

        private void dgv_DocList_SelectionChanged(object sender, EventArgs e)
        {
            panel1.Visible = false;
            if (dgv_DocList.Rows.Count != 0)
                groupBox1.Visible = true;

            string name = dgv_DocList[1, dgv_DocList.CurrentCell.RowIndex].Value.ToString()
                + "  " + dgv_DocList[2, dgv_DocList.CurrentCell.RowIndex].Value.ToString();
            txt_DocInfo_Name.Text = name;
            txt_DocInfo_Addr.Text = ds2.Tables[0].Rows[dgv_DocList.CurrentCell.RowIndex][3].ToString();
            txt_DocInfo_Sex.Text = ds2.Tables[0].Rows[dgv_DocList.CurrentCell.RowIndex][4].ToString();
            txt_DocInfo_Mail.Text = ds2.Tables[0].Rows[dgv_DocList.CurrentCell.RowIndex][5].ToString();
            txt_DocInfo_Mob.Text = ds2.Tables[0].Rows[dgv_DocList.CurrentCell.RowIndex][6].ToString();
            richTxt_DocInfo_Details.Text = ds2.Tables[0].Rows[dgv_DocList.CurrentCell.RowIndex][7].ToString();
            int age;
            age = int.Parse(DateTime.Now.Year.ToString()) - (int.Parse(ds2.Tables[0].Rows[dgv_DocList.CurrentCell.RowIndex][8].ToString()));
            txt_DocInfo_Age.Text = age.ToString();
        }

        private void btn_ShowAppo_Click(object sender, EventArgs e)
        {
            if (LoginForm.accType == "doctor")
                label_CantAsDoctor.Visible = true;
            doc_id = ds2.Tables[0].Rows[dgv_DocList.CurrentCell.RowIndex][9].ToString();

            string select = "select * from pub.getDocApp('" + doc_id + "');";
            ds = new DataSet();
            Trans(ds, select);
            dgv_DocAppointments.DataSource = ds.Tables[0];
            foreach (DataGridViewColumn dc in dgv_DocAppointments.Columns)
            {
                dc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            groupBox1.Visible = false;
            panel1.Visible = true;

            if (dgv_DocAppointments.Rows.Count != 0)
            {
                DataView view = ds.Tables[0].DefaultView;
                view.Sort = "Appointment Date ASC, Appointment Time ASC";
                dgv_DocAppointments.DataSource = view;
            }
        }

        private void btn_BookAnAppo_Click(object sender, EventArgs e)
        {
            if (dgv_DocAppointments.Rows.Count == 0)
                MessageBox.Show("There is no appointments to be booked");
            else
            {
                DialogResult confirm = MessageBox.Show("Are you sure you want to book the selected appointment?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string appid = dgv_DocAppointments[0, dgv_DocAppointments.CurrentCell.RowIndex].Value.ToString();
                    string query = "EXEC pub.bookAppo " +
            "@appo_id = '" + appid +
            "',@pat_id= '" + LoginForm.id + "'";
                    cmd(query);
                    comBox_DocSpec_SelectedValueChanged(comBox_DocSpec, EventArgs.Empty);
                    MessageBox.Show("The appointment has been booked successfully");
                }
            }
        }

        private void btn_DocAppBack_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            groupBox1.Visible = true;
        }
        private void FarmaPage_Enter(object sender, EventArgs e)
        {
            dgv_FarmasList.Rows.Clear();
            comBox_FarmaArea.Items.Clear();
            string select = "select * from pub.getFarmaLoc()";
            ds = new DataSet();
            Trans(ds, select);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                comBox_FarmaArea.Items.Add(dr[0]);
            }
        }

        private void comBox_FarmaArea_SelectedValueChanged(object sender, EventArgs e)
        {
            dgv_FarmasList.Rows.Clear();
            string select = "select * from pub.getCenter('farma')";
            ds = new DataSet();
            Trans(ds, select);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["location"].ToString() == comBox_FarmaArea.SelectedItem.ToString())
                    dgv_FarmasList.Rows.Add(dr["name"], dr["location"], dr["mobile"]);
            }
        }

        private void LabPage_Enter(object sender, EventArgs e)
        {
            dgv_LabsList.Rows.Clear();
            comBox_LabArea.Items.Clear();
            string select = "select * from pub.getLabLoc()";

            ds = new DataSet();
            Trans(ds, select);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                comBox_LabArea.Items.Add(dr[0]);
            }
        }

        private void comBox_LabArea_SelectedValueChanged_1(object sender, EventArgs e)
        {
            dgv_LabsList.Rows.Clear();
            string select = "select * from pub.getCenter('lab')";
            ds = new DataSet();
            Trans(ds, select);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["location"].ToString() == comBox_LabArea.SelectedItem.ToString())
                    dgv_LabsList.Rows.Add(dr["name"], dr["location"], dr["mobile"]);
            }
        }

        private void NursePage_Enter(object sender, EventArgs e)
        {
            dgv_NursesList.Rows.Clear();
            dgv_NursesList.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            string select = "select * from pub.getNurse()";
            ds = new DataSet();
            Trans(ds, select);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dgv_NursesList.Rows.Add(dr["ID"], dr["Name"], dr["Phone"], dr["Details"]);
            }
            dgv_NursesList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
        }

        private void OxyPage_Enter(object sender, EventArgs e)
        {
            dgv_OxygenCenters.Rows.Clear();
            dgv_OxygenCenters.Columns[3].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            string sel = "select * from pub.getCenter('oxy')";
            ds = new DataSet();
            Trans(ds, sel);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dgv_OxygenCenters.Rows.Add(dr["name"], dr["location"], dr["mobile"], dr["details"]);
            }
            dgv_OxygenCenters.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
        }

        private void InfoNewsPage_Enter(object sender, EventArgs e)
        {
            label_news_Link_Clicked(null, null);
        }

        private void label_news_Link_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            btn_PrevNews.Visible = false;
            dgv_NewsInfo.Rows.Clear();
            richTxt_NewsDetails.Visible = false;
            dgv_NewsInfo.Visible = true;
            string select = "select * from pub.getInfo('news')";
            ds1 = new DataSet();
            Trans(ds1, select);
            dgv_NewsInfo.Columns[0].DefaultCellStyle.Font = new Font("Simplified Arabic", 15, FontStyle.Bold);
            dgv_NewsInfo.Columns[1].DefaultCellStyle.Font = new Font("Simplified Arabic", 10, FontStyle.Bold);
            dgv_NewsInfo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_NewsInfo.Columns[1].Width = 200;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                dgv_NewsInfo.Rows.Add(dr[0], dr[1], dr[2]);
            }
            dgv_NewsInfo.Sort(dgv_NewsInfo.Columns[1], ListSortDirection.Descending);
        }

        private void labe_Info_Link_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            btn_PrevNews.Visible = false;
            dgv_NewsInfo.Rows.Clear();
            richTxt_NewsDetails.Visible = false;
            dgv_NewsInfo.Visible = true;
            string select = "select * from pub.getInfo('info')";
            ds1 = new DataSet();
            Trans(ds1, select);
            dgv_NewsInfo.Columns[0].DefaultCellStyle.Font = new Font("Simplified Arabic", 15, FontStyle.Bold);
            dgv_NewsInfo.Columns[1].DefaultCellStyle.Font = new Font("Simplified Arabic", 10, FontStyle.Bold);
            dgv_NewsInfo.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_NewsInfo.Columns[1].Width = 200;
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                dgv_NewsInfo.Rows.Add(dr[0], dr[1],dr[2]);
            }
        }

        private void dgv_NewsInfo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_PrevNews.Visible = true;
            richTxt_NewsDetails.Visible = true;
            dgv_NewsInfo.Visible = false;
            richTxt_NewsDetails.Text = dgv_NewsInfo[2, dgv_NewsInfo.CurrentCell.RowIndex].Value.ToString();
        }

        private void btn_PrevNews_Click(object sender, EventArgs e)
        {
            dgv_NewsInfo.Visible = true;
            richTxt_NewsDetails.Visible = false;
            btn_PrevNews.Visible = false;
        }
        //////////SHARED PAGES SECTION END
        //////////SHARED PAGES SECTION END
        //////////SHARED PAGES SECTION END

        private void txt_Profile_Mob_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txt_EditDoc_mob_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txt_NewDoc_mob_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txt_Nurse_Mob_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txt_ManCenters_Mob_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void dgv_EditDoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void richTxt_EditDoc_details_TextChanged(object sender, EventArgs e)
        {

        }

        private void label51_Click(object sender, EventArgs e)
        {

        }

        private void txt_Nurse_Name_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void txt_Profile_ID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comBox_LabArea_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void grouBox_ChPass_Enter(object sender, EventArgs e)
        {

        }

        private void label57_Click(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void DocAppoManagePage_Click(object sender, EventArgs e)
        {

        }
    }
}