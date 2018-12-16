using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DGVPrinterHelper;

namespace Contacts
{
    public partial class Form1 : Form
    {

        public SqlConnection constr = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ContactsDB.mdf;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void peopleBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.peopleBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.contactsDBDataSet);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.peopleTableAdapter.Fill(this.contactsDBDataSet.People);
        }

        private void Search_Click(object sender, EventArgs e)
        {
            string dbCommand = "SELECT * FROM People";

            if (tbClientName.Text != "")
                dbCommand += " WHERE NAME = '" + tbClientName.Text + "'";
            if (tbCompany.Text != "")
                dbCommand += " OR Company = '" + tbCompany.Text + "'";
            if (tbPhone.TextLength < 11)
                dbCommand += " OR Telephone = '" + tbPhone.Text + "'";
            if (tbEmail.Text != "")
                dbCommand += " OR Email = '" + tbEmail.Text + "'";
            if (dateTimePicker1.Checked == true)
                dbCommand += " OR LastCall = '" + dateTimePicker1.Text + "'";

            try
            {


                SqlCommand com = new SqlCommand(dbCommand, constr);
                SqlDataAdapter adapter = new SqlDataAdapter(com);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Поле ФИО является обязательным!", "Ошибка!", MessageBoxButtons.OK);
            }


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                gbEdit.Enabled = true;
            }
            else
                gbEdit.Enabled = false;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PrintToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DGVPrinter printer = new DGVPrinter();
            printer.PageSettings.Landscape = true;
            printer.Title = "Clients";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date.ToString("dd/MM/yyyy"));
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.PrintDataGridView(dataGridView1);


        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developed by \"tarurs67@gmail.com\" for \"A24\" with best wishes.", "About", MessageBoxButtons.OK);
        }


    }
}