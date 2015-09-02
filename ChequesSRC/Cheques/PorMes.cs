using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
namespace Cheques
{
    public partial class PorMes : Form
    {
        public PorMes()
        {
            InitializeComponent();
        }
        public String connStringSun { get; set; }
        public List<Dictionary<string, object>> listaFinal { get; set; }
     
        private void PorMes_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM yyyy";
            dateTimePicker1.ShowUpDown = true; // to prevent the calendar from being displayed
            vesPorElMes();      
        }
        private void vesPorElMes()
        {
            DateTime dateTime = dateTimePicker1.Value;// DateTime.UtcNow.Date;

            String mesP = dateTime.ToString("MMMM");
            String anoP = dateTime.ToString("yyyy");
            this.connStringSun = "Database=" + Properties.Settings.Default.DatabaseCheques + ";Data Source=" + Properties.Settings.Default.sunDatasource + ";Integrated Security=False;MultipleActiveResultSets=true;User ID='" + Properties.Settings.Default.sunUser + "';Password='" + Properties.Settings.Default.sunPassword + "';connect timeout = 10";
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            listaFinal = new List<Dictionary<string, object>>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connStringSun))
                {
                    connection.Open();
                    String queryDiario = "SELECT origen, folio, orden, archivo, nombre, cantidad, numeroALetras, dia,mes,ano,concepto,numeroDiario FROM [" + Properties.Settings.Default.DatabaseCheques + "].[dbo].[cheques] WHERE mes = '" + mesP + "' and ano = '" + anoP + "'  order by idCheque asc";
                    SqlCommand cmdCheck = new SqlCommand(queryDiario, connection);
                    SqlDataReader reader = cmdCheck.ExecuteReader();
                    if (reader.HasRows)
                    {
                        String origen = "";
                        String folio = "";
                        String orden = "";
                        String archivo = "";
                        String nombre = "";
                        String cantidad = "";
                        String numeroALetras = "";
                        String dia = "";
                        String mes = "";
                        String ano = "";
                        String concepto = "";
                        String numeroDiarioS = "";

                        while (reader.Read())
                        {
                            origen = reader.GetString(0).Trim();
                            folio = reader.GetString(1).Trim();
                            orden = reader.GetString(2).Trim();
                            archivo = reader.GetString(3).Trim();
                            nombre = reader.GetString(4).Trim();
                            cantidad = reader.GetString(5).Trim();
                            numeroALetras = reader.GetString(6).Trim();
                            dia = reader.GetString(7).Trim();
                            mes = reader.GetString(8).Trim();
                            ano = reader.GetString(9).Trim();
                            concepto = reader.GetString(10).Trim();
                            numeroDiarioS = reader.GetString(11).Trim();



                            Dictionary<string, object> dictionary = new Dictionary<string, object>();
                            dictionary.Add("origen", origen);
                            dictionary.Add("folio", folio);
                            dictionary.Add("orden", orden);
                            dictionary.Add("archivo", archivo);
                            dictionary.Add("nombre", nombre);
                            dictionary.Add("cantidad", cantidad);
                            dictionary.Add("numeroALetras", numeroALetras);
                            dictionary.Add("dia", dia);
                            dictionary.Add("mes", mes);
                            dictionary.Add("ano", ano);
                            dictionary.Add("concepto", concepto);
                            dictionary.Add("numeroDiarioS", numeroDiarioS);




                            listaFinal.Add(dictionary);


                        }
                        listView1.Clear();
                        listView1.View = View.Details;
                        listView1.GridLines = true;
                        listView1.FullRowSelect = true;
                        listView1.Columns.Add("Nombre", 250);
                        listView1.Columns.Add("Cantidad", 80);
                        listView1.Columns.Add("Dia", 40);
                        listView1.Columns.Add("Mes", 80);
                        listView1.Columns.Add("Año", 40);
                        listView1.Columns.Add("Concepto", 270);
                        listView1.Columns.Add("Diario", 80);
                        listView1.Columns.Add("Folio", 80);
                        listView1.Columns.Add("Origen", 80);
                        listView1.Columns.Add("orden", 0);
                        listView1.Columns.Add("archivo", 0);


                        foreach (Dictionary<string, object> dic in listaFinal)
                        {
                            if (dic.ContainsKey("nombre"))
                            {
                                string[] arr = new string[11];
                                ListViewItem itm2;
                                //add items to ListView
                                arr[0] = Convert.ToString(dic["nombre"]);
                                arr[1] = Convert.ToString(dic["cantidad"]);
                                arr[2] = Convert.ToString(dic["dia"]);
                                arr[3] = Convert.ToString(dic["mes"]);
                                arr[4] = Convert.ToString(dic["ano"]);
                                arr[5] = Convert.ToString(dic["concepto"]);
                                arr[6] = Convert.ToString(dic["numeroDiarioS"]);
                                arr[7] = Convert.ToString(dic["folio"]);
                                arr[8] = Convert.ToString(dic["origen"]);
                                arr[9] = Convert.ToString(dic["orden"]);
                                arr[10] = Convert.ToString(dic["archivo"]);
                                itm2 = new ListViewItem(arr);
                                listView1.Items.Add(itm2);
                            }
                        }
                    }
                    else
                    {
                        //no rows for now
                        listView1.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Error Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            vesPorElMes();      
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String archivo = listView1.SelectedItems[0].SubItems[10].Text.ToString().Trim();
            String url = "file:" + (object)Path.DirectorySeparatorChar + (object)Path.DirectorySeparatorChar + archivo;
            this.webView1.ZoomFactor = 1.0;
            this.webView1.Url = url;
        }
    }
}
