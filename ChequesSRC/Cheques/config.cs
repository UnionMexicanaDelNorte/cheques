using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Cheques
{
    public partial class config : Form
    {
        public String connString { get; set; }
     
        public config()
        {
            InitializeComponent();
            this.connString = "Database=" + Properties.Settings.Default.sunDatabase + ";Data Source=" + Properties.Settings.Default.sunDatasource + ";Integrated Security=False;User ID='" + Properties.Settings.Default.sunUser + "';Password='" + Properties.Settings.Default.sunPassword + "';connect timeout = 10";

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void revisar_Click(object sender, EventArgs e)
        {
           

        }
        private void button2_Click(object sender, EventArgs e)
        {
           
        }
        private void config_Load(object sender, EventArgs e)
        {
            database.Text = Properties.Settings.Default.sunDatabase;
            datasource.Text = Properties.Settings.Default.sunDatasource;
            user.Text = Properties.Settings.Default.sunUser;
            password.Text = Properties.Settings.Default.sunPassword;
            DatabaseCheques.Text = Properties.Settings.Default.DatabaseCheques;
            libro.Text = Properties.Settings.Default.sunLibro;
            letraText.Text = Properties.Settings.Default.letra;
            unidad.Text = Properties.Settings.Default.sunUnidadDeNegocio;
          
            


        }

        private void revisar_Click_1(object sender, EventArgs e)
        {
            save();
            this.connString = "Database=" + Properties.Settings.Default.sunDatabase + ";Data Source=" + Properties.Settings.Default.sunDatasource + ";Integrated Security=False;User ID='" + Properties.Settings.Default.sunUser + "';Password='" + Properties.Settings.Default.sunPassword + "';connect timeout = 10";
            String queryCheck = "USE [" + Properties.Settings.Default.sunDatabase + "] SELECT name FROM sys.tables";
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();

                    SqlCommand cmdCheck = new SqlCommand(queryCheck, connection);
                    SqlDataReader reader = cmdCheck.ExecuteReader();
                    if (reader.HasRows)
                    {
                        System.Windows.Forms.MessageBox.Show("Conexión Establecida satisfactoriamente", "SunPlusXML", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Sin conexión", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Sin conexión", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void save()
        {
            Properties.Settings.Default.sunDatabase = database.Text;
            Properties.Settings.Default.sunDatasource = datasource.Text;
            Properties.Settings.Default.sunUser = user.Text;
            Properties.Settings.Default.sunPassword = password.Text;
            Properties.Settings.Default.sunLibro = libro.Text;
            Properties.Settings.Default.sunUnidadDeNegocio = unidad.Text;
            Properties.Settings.Default.DatabaseCheques = DatabaseCheques.Text;
            Properties.Settings.Default.letra = letraText.Text;
            Properties.Settings.Default.Save();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            save();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
