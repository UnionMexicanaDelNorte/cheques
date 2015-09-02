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

using System.Diagnostics;
using System.IO;

namespace Cheques
{
    public partial class Form1 : Form
    {
        private class Item
        {
            public string Name;
            public int Value;
            public string Extra;
            public Item(string name, int value, String extra)
            {
                Name = name; Value = value;
                Extra = extra;
            }
            public Item(string name, int value)
            {
                Name = name; Value = value;
            }
            public override string ToString()
            {
                // Generates the text shown in the combo box
                return Name;
            }
        }
        public String connStringSun { get; set; }

        
        public List<Dictionary<string, object>> listaFinal { get; set; }
        public List<Dictionary<string, object>> listaFinalconDatos { get; set; }
      
       
        public Form1()
        {
            InitializeComponent();
        this.cancelar2.Click += new System.EventHandler(this.cancelar_Click);
        this.aceptar2.Click += new System.EventHandler(this.aceptar_Click);


        this.connStringSun = "Database=" + Properties.Settings.Default.sunDatabase + ";Data Source=" + Properties.Settings.Default.sunDatasource + ";Integrated Security=False;MultipleActiveResultSets=true;User ID='" + Properties.Settings.Default.sunUser + "';Password='" + Properties.Settings.Default.sunPassword + "';connect timeout = 10";     
            int empiezo = 1;
            comboBanco2.Items.Add(new Item("ScotiaBank", empiezo));
            comboBanco2.SelectedIndex = 0;
            empiezo++;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Hola", "Este programa fue hecho por Fernando Alonso Pecina", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      
        }
        private void auxaux()
        {
            this.connStringSun = "Database=" + Properties.Settings.Default.sunDatabase + ";Data Source="+Properties.Settings.Default.sunDatasource+";Integrated Security=False;MultipleActiveResultSets=true;User ID='" + Properties.Settings.Default.sunUser + "';Password='" + Properties.Settings.Default.sunPassword + "';connect timeout = 10";
       
            if(numeroDiario2.Text.Trim().Equals("") )
            {
                DialogResult dialogResult = MessageBox.Show("Debes de escribir un numero de diario", "Verificar datos", MessageBoxButtons.YesNo);
          
            }
            else
            {

            
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            listaFinal = new List<Dictionary<string, object>>();
            listaFinalconDatos = new List<Dictionary<string, object>>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connStringSun))
                {
                    connection.Open();
                    String queryDiario = "SELECT DESCRIPTN, AMOUNT, TREFERENCE, ANAL_T0, JRNAL_SRCE, ACCNT_CODE, D_C FROM [" + Properties.Settings.Default.sunDatabase + "].[dbo].[" + Properties.Settings.Default.sunUnidadDeNegocio + "_" + Properties.Settings.Default.sunLibro + "_SALFLDG] WHERE JRNAL_NO = " + numeroDiario2.Text + "  order by JRNAL_LINE asc";
                    SqlCommand cmdCheck = new SqlCommand(queryDiario, connection);
                    SqlDataReader reader = cmdCheck.ExecuteReader();
                    if (reader.HasRows)
                    {
                        String nombre = "";
                        String amount = "";
                        String reference = "";
                        String referenceAnterior = "";
                        String anal = "";
                        String JRNAL_SRCE = "";
                        String ACCNT_CODE = "";
                        String D_C = "";
                       
                        long entero = 0;
                        int decimales = 0;
                        String decimalesP = "";
                        while (reader.Read())
                        {
                            reference = reader.GetString(2).Trim();
                            anal = reader.GetString(3);
                            D_C = reader.GetString(6);
                            nombre = Convert.ToString(reader.GetString(0));
                            JRNAL_SRCE = Convert.ToString(reader.GetString(4));
                            ACCNT_CODE = Convert.ToString(reader.GetString(5));
                            double nro = 0;

                            try
                            {
                                nro = Convert.ToDouble(reader.GetDecimal(1));
                            }
                            catch
                            {
                                //return "";
                            }
                            entero = Convert.ToInt64(Math.Truncate(nro));
                            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
                            if (decimales.ToString().Length == 1)
                            {
                                decimalesP = decimales + "0";
                            }
                            else
                            {
                                decimalesP = "" + decimales;
                            }
                            amount = Math.Abs(entero).ToString("#,##0");
                            String total = amount + "." + decimalesP;

                            Dictionary<string, object> dictionary = new Dictionary<string, object>();
                            dictionary.Add("nombre", nombre);
                            dictionary.Add("total", total);
                            dictionary.Add("reference", reference);
                            dictionary.Add("anal", anal);
                            dictionary.Add("JRNAL_SRCE", JRNAL_SRCE);
                            dictionary.Add("ACCNT_CODE", ACCNT_CODE);
                            dictionary.Add("D_C", D_C);



                            if (!anal.Equals(referenceAnterior))
                            {
                                referenceAnterior = anal;
                               
                                listaFinal.Add(dictionary);

                            }
                            else
                            {
                                listaFinalconDatos.Add(dictionary);
                            }


                        }
                        listView1.Clear();
                        listView1.View = View.Details;
                        listView1.GridLines = true;
                        listView1.FullRowSelect = true;
                        listView1.Columns.Add("Nombre", 180);
                        listView1.Columns.Add("Cantidad", 100);
                        listView1.Columns.Add("Referencia", 100);
                        listView1.Columns.Add("Numero", 100);
                        listView1.Columns.Add("JRNAL_SRCE", 0);
                        listView1.Columns.Add("ACCNT_CODE", 0);

                        deboEntrar = true;
                        foreach (Dictionary<string, object> dic in listaFinal)
                        {
                            if (dic.ContainsKey("nombre"))
                            {
                                string[] arr = new string[6];
                                ListViewItem itm2;
                                //add items to ListView
                                arr[0] = Convert.ToString(dic["nombre"]);
                                arr[1] = Convert.ToString(dic["total"]);
                                arr[2] = Convert.ToString(dic["reference"]);
                                arr[3] = Convert.ToString(dic["anal"]);
                                arr[4] = Convert.ToString(dic["JRNAL_SRCE"]);
                                arr[5] = Convert.ToString(dic["ACCNT_CODE"]);

                                itm2 = new ListViewItem(arr);
                                listView1.Items.Add(itm2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Error Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }
        private void aceptar_Click(object sender, EventArgs e)
        {
            auxaux();
          
        
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
                 
        }

        private void cancelar2_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
                 
        }

        private void aceptar2_Click(object sender, EventArgs e)
        {

        }
        private bool deboEntrar;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
           
            if(deboEntrar)
            {
                string path = "S:" + (object)Path.DirectorySeparatorChar + "cheques";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                DateTime dateTime = dateTimePicker1.Value;// DateTime.UtcNow.Date;

                String fecha = dateTime.ToString("dd 'de' MMMM 'de' yyyy");
                // add text to the text box
                String dia = dateTime.ToString("dd");
                String mes = dateTime.ToString("MMMM");
                String ano = dateTime.ToString("yyyy");
                

                String total = listView1.SelectedItems[0].SubItems[1].Text.ToString().Trim();
                String nombre = listView1.SelectedItems[0].SubItems[0].Text.ToString().Trim();
                String reference = listView1.SelectedItems[0].SubItems[2].Text.ToString().Trim();
                String anal = listView1.SelectedItems[0].SubItems[3].Text.ToString().Trim();
                String JRNAL_SRCE = listView1.SelectedItems[0].SubItems[4].Text.ToString().Trim();
                String ACCNT_CODE = listView1.SelectedItems[0].SubItems[5].Text.ToString().Trim();

                //    String total = last.SubItems[1].Text.ToString();
                //  String nombre = last.SubItems[0].Text.ToString();

                String numeroAletras = "(SON: " + Conversiones.NumeroALetras(total) + " M.N.)";


                confirm formC = new confirm();
                formC.connStringSun = this.connStringSun;
                formC.nombre = nombre;
                formC.fecha = fecha;
                formC.dia = dia;
                formC.ano = ano;
                formC.mes = mes;
                formC.listaFinalconDebitos = listaFinalconDatos;
                formC.total = total;
                formC.reference = reference;
                formC.anal = anal;
                formC.JRNAL_SRCE = JRNAL_SRCE;
                formC.ACCNT_CODE = ACCNT_CODE;
                formC.numeroAletras = numeroAletras;
               
                String preconcepto = "";
                bool first = true;
                try
                {
                    using (SqlConnection connection = new SqlConnection(connStringSun))
                    {
                        connection.Open();
                        foreach (Dictionary<string, object> dic in listaFinalconDatos)
                        {
                            if (dic.ContainsKey("nombre"))
                            {
                                String analPrima = Convert.ToString(dic["anal"]).Trim();
                                String totalPrima = Convert.ToString(dic["total"]).Trim();

                                String ACCNT_CODEPrima = Convert.ToString(dic["ACCNT_CODE"]).Trim();
                                if (analPrima.Equals(anal) && first)
                                {
                                    first = false;
                                    preconcepto = Convert.ToString(dic["nombre"]).Trim();
                               /*
                                    String paraVer2 = Convert.ToString(dic["ACCNT_CODE"]).Trim();

                                    String queryDiario = "SELECT DESCR FROM [" + Properties.Settings.Default.sunDatabase + "].[dbo].[" + Properties.Settings.Default.sunUnidadDeNegocio + "_ACNT] WHERE ACNT_CODE = '" + paraVer2 + "'";
                                    SqlCommand cmdCheck = new SqlCommand(queryDiario, connection);
                                    SqlDataReader reader = cmdCheck.ExecuteReader();
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            preconcepto = reader.GetString(0);
                                        }
                                    }*/
                                }
                            }
                        }
                       
                        
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString(), "Error Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }



                formC.numeroDiario = numeroDiario2.Text.Trim();
                formC.preconcepto = preconcepto;
                formC.rellena();
                formC.Show();

               
                    if (listView1.SelectedIndices.Count > 0)
                    {
                        for (int i = 0; i < listView1.SelectedIndices.Count; i++)
                        {
                            int indexA = listView1.SelectedIndices[i];
                            ListViewItem hola = listView1.Items[indexA];
                            deboEntrar = false;
                            hola.Selected = false;
                        }
                    }
                
            }
            else
            {
                deboEntrar = true;
            }
              this.Cursor = System.Windows.Forms.Cursors.Arrow;
          
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void servidorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            config form2 = new config();
            form2.Show();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Para errores y soporte contactar a f.pecina@unav.edu.mx", "Sunplusito", MessageBoxButtons.OK, MessageBoxIcon.Information);   
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                System.Environment.Exit(1);
            }
        }

        private void cambiarEspaciosDeChequeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            previewCheque formP = new previewCheque();
            formP.Show();
        }

        private void numeroDiario2_TextChanged(object sender, EventArgs e)
        {
            auxaux();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.numeroDiario2.Focus();
     
            numeroDiario2.KeyPress += numeroDiario_KeyPress;
           
        }

        private void numeroDiario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                

            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void porNúmeroDeDiarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PorNumeroDeDiario form = new PorNumeroDeDiario();
            form.Show();
        }

        private void porMesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PorMes form = new PorMes();
            form.Show();
        }

    }
}
