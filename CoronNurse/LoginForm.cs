using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
namespace CoronNurse
{
    public partial class LoginForm : Form
    {
        static private string connString = "Data Source=SQL8004.site4now.net;Initial Catalog=db_a8b69a_coronanurse;User Id=db_a8b69a_coronanurse_admin;Password=ZIAD123dawoodi";
        static public SqlConnection conn = new SqlConnection(connString);
        static public string id { get; private set; }
        static public string logid { get; private set; }
        static public string accType { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            loadCaptchaImage();
        }

        string capt;
        private void loadCaptchaImage()
        {
            Random r1 = new Random();
            int num = r1.Next(10000, 99999);
            Random r2 = new Random();
            int n = r2.Next(0, 25);
            var image = new Bitmap(this.picCaptcha.Width, this.picCaptcha.Height);
            var font = new Font("TimesNewRoman", 25, FontStyle.Italic, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(image);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            capt = chars[n].ToString() + num;
            graphics.DrawString(capt, font, Brushes.Teal, new Point(10, 10));
            picCaptcha.Image = image;
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
            //
        }

        private static DataSet Trans(DataSet dataset, string queryString)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, conn);
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
                return dataset;
            }
        }

        private static bool CheckUsernameExist(string username)
        {
            try
            {
                string select = "select * from gue.CheckUname('" + username + "')";
                DataSet ds = new DataSet();
                Trans(ds, select);
                if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    return false;
                }
                else
                    return true;
            }
            catch (SqlException)
            {
                DialogResult confirmClose = MessageBox.Show("Cannot connect to the server\n The application will be shoutdown", "Warning", MessageBoxButtons.OK);
                if (confirmClose == DialogResult.OK)
                {
                    Application.Exit();
                }
                return false;
            }
        }
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtCaptcha.Text == capt)
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("select * from gue.login('" + txtLoginUsername.Text.Trim() + "', '" + useMD5Hash(txtLoginPassword.Text.Trim()) + "')", conn);
                    SqlCommandBuilder cb = new SqlCommandBuilder(adapter);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    if (String.IsNullOrEmpty((ds.Tables[0].Rows[0][0].ToString())))
                    {
                        MessageBox.Show("Username or password is uncorrect", "Warning");
                        loadCaptchaImage();
                    }
                    else
                    {
                        logid = ds.Tables[0].Rows[0][0].ToString();
                        accType = ds.Tables[0].Rows[0][1].ToString();
                        //
                        adapter = new SqlDataAdapter("select * from pub.getID(" + logid + ",'" + accType + "')", conn);
                        cb = new SqlCommandBuilder(adapter);
                        ds = new DataSet();
                        adapter.Fill(ds);
                        id = ds.Tables[0].Rows[0][0].ToString();
                        MainForm mainf = new MainForm();
                        this.Hide();
                        mainf.Closed += (s, args) => this.Close();
                        mainf.Show();
                    }
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
            else
            {
                MessageBox.Show("Please enter the code correctly", "Error");
                loadCaptchaImage();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckUsernameExist(txtUsername.Text.Trim()))
                {
                    MessageBox.Show("Username is taken, please try another one", "Warning");
                }
                else if (txtPassword.Text.Length < 8)
                {
                    MessageBox.Show("Password is short, it must be at least 8 character");
                }
                else if (txtPassword.Text == txtConfirmPw.Text)
                {
                    DialogResult confirm = MessageBox.Show("Are you sure of all the information?", "Confirm", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        if (String.IsNullOrEmpty(txtUsername.Text) || String.IsNullOrEmpty(txtPassword.Text) || String.IsNullOrEmpty(txtUfname.Text)
                            || String.IsNullOrEmpty(txtUlname.Text) || String.IsNullOrEmpty(txtUmob.Text) || String.IsNullOrEmpty(txtUmail.Text)
                            || String.IsNullOrEmpty(comBoxUsex.Text) || String.IsNullOrEmpty(datePickerUdob.Text))
                        {
                            MessageBox.Show("There is an empty field, please fill all the fields");
                        }
                        else
                        {
                            string query = ("EXEC gue.newUser " +
            "@username= N'" + txtUsername.Text.Trim() +
            "',@password= N'" + useMD5Hash(txtPassword.Text.Trim()) +
            "',@first_name= N'" + txtUfname.Text.Trim() +
            "',@last_name= N'" + txtUlname.Text.Trim() +
            "',@mobile= N'" + txtUmob.Text.Trim() +
            "',@mail= N'" + txtUmail.Text.Trim() +
            "',@gender= N'" + comBoxUsex.SelectedItem +
            "',@dob= N'" + datePickerUdob.Text + "'");
                            conn.Open();
                            SqlCommand sqlcmd = new SqlCommand(query, conn);
                            sqlcmd.ExecuteNonQuery();
                            groupBoxLogin.Visible = true;
                            panelNewAccInfo.Visible = false;
                            LabelContactUs.Visible = true;
                            btn_newAcc.Text = "Create patient account";
                            MessageBox.Show("The new account has been successfully registered");
                            conn.Close();
                        }

                    }
                }
                else
                    MessageBox.Show("Passwords do NOT match", "Warning");
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

        private void txtUmob_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtCaptcha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(this, new EventArgs());
            }

        }

        private void txtUfname_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtUlname_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btn_newAcc_Click(object sender, EventArgs e)
        {
            if (btn_newAcc.Text == "Create patient account")
            {
                txtUsername.Text = null;
                txtPassword.Text = null;
                txtConfirmPw.Text = null;
                txtUfname.Text = null;
                txtUlname.Text = null;
                txtUmob.Text = null;
                txtUmail.Text = null;
                comBoxUsex.Text = null;
                datePickerUdob.Text = null;
                groupBoxLogin.Visible = false;
                panelNewAccInfo.Visible = true;
                LabelContactUs.Visible = false;
                datePickerUdob.MaxDate = DateTime.Today;
                btn_newAcc.Text = "Back";
            }
            else if (btn_newAcc.Text == "Back")
            {
                btn_newAcc.Text = "Create patient account";
                groupBoxLogin.Visible = true;
                panelNewAccInfo.Visible = false;
                LabelContactUs.Visible = true;
            }
        }

        private void btn_CreateNewAcc_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult confirm = MessageBox.Show("Are you sure of all the information?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    if (txtPassword.Text.Length < 8)
                    {
                        MessageBox.Show("Password is short, it must be at least 8 character");
                    }
                    else if (txtPassword.Text != txtConfirmPw.Text)
                    {
                        MessageBox.Show("Passwords do NOT match", "Warning");
                    }
                    else if (!IsValidEmail(txtUmail.Text))
                    {
                        MessageBox.Show("Invalid email address", "Warning");
                    }
                    else if (String.IsNullOrEmpty(txtUsername.Text) || String.IsNullOrEmpty(txtPassword.Text) || String.IsNullOrEmpty(txtUfname.Text)
                        || String.IsNullOrEmpty(txtUlname.Text) || String.IsNullOrEmpty(txtUmob.Text) || String.IsNullOrEmpty(txtUmail.Text)
                        || String.IsNullOrEmpty(comBoxUsex.Text) || String.IsNullOrEmpty(datePickerUdob.Text))
                    {
                        MessageBox.Show("There is an empty field, please fill all the fields");
                    }
                    else if (CheckUsernameExist(txtUsername.Text.Trim()))
                    {
                        MessageBox.Show("Username is taken, please try another one", "Warning");
                    }
                    else
                    {
                        string query = ("EXEC gue.newUser " +
        "@username= N'" + txtUsername.Text.Trim() +
        "',@password= N'" + useMD5Hash(txtPassword.Text.Trim()) +
        "',@first_name= N'" + txtUfname.Text.Trim() +
        "',@last_name= N'" + txtUlname.Text.Trim() +
        "',@mobile= N'" + txtUmob.Text.Trim() +
        "',@mail= N'" + txtUmail.Text.Trim() +
        "',@gender= N'" + comBoxUsex.SelectedItem +
        "',@dob= N'" + datePickerUdob.Text + "'");
                        conn = new SqlConnection(connString);
                        conn.Open();
                        SqlCommand sqlcmd = new SqlCommand(query, conn);
                        sqlcmd.ExecuteNonQuery();
                        groupBoxLogin.Visible = true;
                        panelNewAccInfo.Visible = false;
                        LabelContactUs.Visible = true;
                        btn_newAcc.Text = "Create patient account";
                        MessageBox.Show("The new account has been successfully registered");
                        conn.Close();
                    }
                }
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

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
