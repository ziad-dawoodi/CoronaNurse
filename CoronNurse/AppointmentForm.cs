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

namespace CoronNurse
{
    public partial class AppointmentForm : Form
    {
        
        public AppointmentForm()
        {
            InitializeComponent();
        }
        //private DoctorsForm mainForm = null;

        private void AppointmentForm_Load(object sender, EventArgs e)
        {

            string select = "select * from cn.getDocApp('" + DoctorsForm.doc_id + "');";
            // Your Connection String here
            var dataAdapter = new SqlDataAdapter(select, Form1.conn);
            var commandBuilder = new SqlCommandBuilder(dataAdapter);
            var ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataAdapter = null;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
        }
    }
}
