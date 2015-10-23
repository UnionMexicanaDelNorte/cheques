using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PdfFileWriter;
using System.Threading.Tasks;
using System.Data.SqlClient;

using System.Diagnostics;
using System.IO;
namespace Cheques
{
    public partial class confirm : Form
    {
        public PdfFont ArialNormalToPDF { get; set; }
        public PdfFont ArialBoldToPDF { get; set; }
        public PdfFont ArialItalicToPDF { get; set; }
        public PdfFont ArialBoldItalicToPDF { get; set; }
        public PdfFont TimesNormalToPDF { get; set; }
        public PdfFont ComicToPDF { get; set; }
        public int tipoDeBancoGlobal { get; set; }

        
        public PdfFont ArialNormal { get; set; }
        public PdfFont ArialBold { get; set; }
        public PdfFont ArialItalic { get; set; }
        public PdfFont ArialBoldItalic { get; set; }
        public PdfFont TimesNormal { get; set; }
        public PdfFont Comic { get; set; }
        public PdfDocument Document { get; set; }
        public PdfDocument DocumentToPDF { get; set; }

        public PdfTilingPattern WaterMark { get; set; }
        public PdfTilingPattern WaterMarkToPDF { get; set; }

        public String preconcepto { get; set; }
        public String dia { get; set; }
        public String mes { get; set; }
        public String ano { get; set; }
     
        public String connStringSun { get; set; }
        public String fecha { get; set; }
        public String nombre { get; set; }
        public String total { get; set; }
        public String numeroAletras { get; set; }
        public String reference { get; set; }
        public String JRNAL_SRCE { get; set; }
        public String anal { get; set; }
        public String ACCNT_CODE { get; set; }
        public String numeroDiario { get; set; }
        public List<Dictionary<string, object>> listaFinalconDebitos { get; set; }
      

        // Define Tiling Pattern Resource
        private void DefineTilingPatternResource()
        {

            // create empty tiling pattern
            WaterMark = new PdfTilingPattern(Document);
            WaterMarkToPDF = new PdfTilingPattern(DocumentToPDF);

            // the pattern will be PdfFileWriter layed out in brick pattern
            String Mark = "PdfFileWriter";

            // text width and height for Arial bold size 18 points
            Double FontSize = 18.0;
            Double TextWidth = ArialBold.TextWidth(FontSize, Mark);
            Double TextHeight = ArialBold.LineSpacing(FontSize);

            // text base line
            Double BaseLine = ArialBold.DescentPlusLeading(FontSize);

            // the overall pattern box (we add text height value as left and right text margin)
            Double BoxWidth = TextWidth + 2 * TextHeight;
            Double BoxHeight = 4 * TextHeight;


            WaterMark.SetTileBox(BoxWidth, BoxHeight);
            WaterMark.SaveGraphicsState();
            WaterMark.SetColorNonStroking(Color.FromArgb(230, 244, 255));
            WaterMark.DrawRectangle(0, 0, BoxWidth, BoxHeight, PaintOp.Fill);
            WaterMark.SetColorNonStroking(Color.White);
            WaterMark.DrawText(ArialBold, FontSize, BoxWidth / 2, BaseLine, TextJustify.Center, Mark);

            WaterMarkToPDF.SetTileBox(BoxWidth, BoxHeight);
            WaterMarkToPDF.SaveGraphicsState();
            WaterMarkToPDF.SetColorNonStroking(Color.FromArgb(230, 244, 255));
            WaterMarkToPDF.DrawRectangle(0, 0, BoxWidth, BoxHeight, PaintOp.Fill);
            WaterMarkToPDF.SetColorNonStroking(Color.White);
            WaterMarkToPDF.DrawText(ArialBold, FontSize, BoxWidth / 2, BaseLine, TextJustify.Center, Mark);

            BaseLine += BoxHeight / 2;

            WaterMark.DrawText(ArialBold, FontSize, 0.0, BaseLine, TextJustify.Center, Mark);
            WaterMark.DrawText(ArialBold, FontSize, BoxWidth, BaseLine, TextJustify.Center, Mark);
            WaterMark.RestoreGraphicsState();

            WaterMarkToPDF.DrawText(ArialBold, FontSize, 0.0, BaseLine, TextJustify.Center, Mark);
            WaterMarkToPDF.DrawText(ArialBold, FontSize, BoxWidth, BaseLine, TextJustify.Center, Mark);
            WaterMarkToPDF.RestoreGraphicsState();

            return;
        }

        private void DefineFontResources()
        {
            // Define font resources
            // Arguments: PdfDocument class, font family name, font style, embed flag
            // Font style (must be: Regular, Bold, Italic or Bold | Italic) All other styles are invalid.
            // Embed font. If true, the font file will be embedded in the PDF file.
            // If false, the font will not be embedded
            String FontName1 = "Arial";
            String FontName2 = "Times New Roman";


            ArialNormal = new PdfFont(Document, FontName1, System.Drawing.FontStyle.Regular, true);
            ArialBold = new PdfFont(Document, FontName1, System.Drawing.FontStyle.Bold, true);
            ArialItalic = new PdfFont(Document, FontName1, System.Drawing.FontStyle.Italic, true);
            ArialBoldItalic = new PdfFont(Document, FontName1, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, true);
            TimesNormal = new PdfFont(Document, FontName2, System.Drawing.FontStyle.Regular, true);
            Comic = new PdfFont(Document, "Comic Sans MS", System.Drawing.FontStyle.Bold, true);

            ArialNormalToPDF = new PdfFont(DocumentToPDF, FontName1, System.Drawing.FontStyle.Regular, true);
            ArialBoldToPDF = new PdfFont(DocumentToPDF, FontName1, System.Drawing.FontStyle.Bold, true);
            ArialItalicToPDF = new PdfFont(DocumentToPDF, FontName1, System.Drawing.FontStyle.Italic, true);
            ArialBoldItalicToPDF = new PdfFont(DocumentToPDF, FontName1, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, true);
            TimesNormalToPDF = new PdfFont(DocumentToPDF, FontName2, System.Drawing.FontStyle.Regular, true);
            ComicToPDF = new PdfFont(DocumentToPDF, "Comic Sans MS", System.Drawing.FontStyle.Bold, true);

            // substitute one character for another
            // this program supports characters 32 to 126 and 160 to 255
            // if a font has a character outside these ranges that is required by the application,
            // you can replace an unused character with this character
            // Note: space (32) and non breaking space (160) cannot be replaced
            /*ArialNormal.CharSubstitution(9679, 9679, 161);		// euro
            ArialNormal.CharSubstitution(1488, 1514, 162);		// hebrew
            ArialNormal.CharSubstitution(1040, 1045, 189);		// russian
            ArialNormal.CharSubstitution(945, 950, 195);		// greek

            ArialNormalToPDF.CharSubstitution(9679, 9679, 161);		// euro
            ArialNormalToPDF.CharSubstitution(1488, 1514, 162);		// hebrew
            ArialNormalToPDF.CharSubstitution(1040, 1045, 189);		// russian
            ArialNormalToPDF.CharSubstitution(945, 950, 195);		// greek
             * */
            return;
        }
        public void rellena()
        {
            nombreT.Text = nombre;
            fechaT.Text = fecha;
            totalT.Text = total;
            numeroALetrasT.Text = numeroAletras;
            preconceptoT.Text = preconcepto;
        }
        public confirm()
        {
            InitializeComponent();
           
        }

        public confirm(int tipoDeBanco)
        {
            InitializeComponent();
            tipoDeBancoGlobal = tipoDeBanco;
        }

        public confirm(int tipoDeBanco, String conn, String nombr, String fech, String di, String an, String me, List<Dictionary<string, object>> lista, String tota, String refe, String ana, String source, String acnt, String numero, String numeroDiario2, String pre)
        {
            InitializeComponent();
            tipoDeBancoGlobal = tipoDeBanco;
            connStringSun = conn;
            nombre = nombr;
            fecha = fech;
            dia = di;
            ano = an;
            mes = me;
            listaFinalconDebitos = lista;
            total = tota;
            reference = refe;
            anal = ana;
            JRNAL_SRCE = source;
            ACCNT_CODE = acnt;
            numeroAletras = numero;
            numeroDiario = numeroDiario2;
            preconcepto = pre;
        }
                
    
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void si_Click(object sender, EventArgs e)
        {
            preconcepto = preconceptoT.Text;
            nombre = nombreT.Text;
            fecha = fechaT.Text;
            total = totalT.Text;
            numeroAletras = numeroALetrasT.Text;

            string path = Properties.Settings.Default.letra+":" + (object)Path.DirectorySeparatorChar + "cheques";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            String numeroDeCheque = "";
            if(anal.Length>11)
            {
                numeroDeCheque = anal.Substring(5, 7);
            }
            String FileName = path + (object)Path.DirectorySeparatorChar + numeroDiario + "_" + numeroDeCheque + ".pdf";
            String FileNameToPDF = path + (object)Path.DirectorySeparatorChar + numeroDiario + "_" + numeroDeCheque + "-Orden_de_cheque.pdf";
            if (File.Exists(FileName))
            {
                try
                {
                    File.Delete(FileName);
                }
                catch (IOException ex2)
                {
                    System.Windows.Forms.MessageBox.Show(ex2.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            if (File.Exists(FileNameToPDF))
            {
                try
                {
                    File.Delete(FileNameToPDF);
                }
                catch (IOException ex2)
                {
                     System.Windows.Forms.MessageBox.Show(ex2.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            Document = new PdfDocument(PaperType.sobreprima, false, UnitOfMeasure.Point, FileName);

            DocumentToPDF = new PdfDocument(PaperType.A4, false, UnitOfMeasure.Point, FileNameToPDF);

            DefineFontResources();
            DefineTilingPatternResource();
            PdfPage Page = new PdfPage(Document);
            PdfContents Contents = new PdfContents(Page);


            PdfPage PageToPDF = new PdfPage(DocumentToPDF);
            PdfContents ContentsToPDF = new PdfContents(PageToPDF);


            Contents.SaveGraphicsState();
            Contents.Translate(0.1, 0.1);


            ContentsToPDF.SaveGraphicsState();
            ContentsToPDF.Translate(0.1, 0.1);

            const Double Width = 632;
            const Double Height = 288;
            const Double FontSize = 9.0;

            const Double WidthToPDF = 632;//1200;
            const Double HeightToPDF = 850;// 288;//2138;
            const Double FontSizeToPDF = 12.0;

           string[] arrayCuentas = new string[100] ;
            string[] arrayDescr = new string[100] ;
            string[] arrayCantidad = new string[100]; 
          
            int con=0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connStringSun))
                {
                    connection.Open();
                    foreach (Dictionary<string, object> dic in listaFinalconDebitos)
                    {
                        if (dic.ContainsKey("nombre"))
                        {
                            String analPrima = Convert.ToString(dic["anal"]).Trim();
                            String totalPrima = Convert.ToString(dic["total"]);
                            totalPrima = totalPrima.Replace("-", "");
                            
                            String ACCNT_CODEPrima = Convert.ToString(dic["ACCNT_CODE"]).Trim();
                            if(analPrima.Equals(anal))
                            {
                                String paraVer = ACCNT_CODEPrima.Trim();
                                
                                String queryDiario = "SELECT DESCR FROM [" + Properties.Settings.Default.sunDatabase + "].[dbo].[" + Properties.Settings.Default.sunUnidadDeNegocio + "_ACNT] WHERE ACNT_CODE = '" + paraVer + "'";
                                SqlCommand cmdCheck = new SqlCommand(queryDiario, connection);
                                SqlDataReader reader = cmdCheck.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        arrayCuentas[con] = paraVer;
                                        arrayCantidad[con] = totalPrima;
                                        arrayDescr[con] = reader.GetString(0);
                                        con++;
                                    }
                                }
                            }

                        }
                    }

                    

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Error Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            String descr_acntC = "";


          
            try
            {
                using (SqlConnection connection = new SqlConnection(connStringSun))
                {
                    connection.Open();
                    String paraVer2 = Convert.ToString(ACCNT_CODE).Trim();
                    String queryDiario = "SELECT DESCR FROM [" + Properties.Settings.Default.sunDatabase + "].[dbo].[" + Properties.Settings.Default.sunUnidadDeNegocio + "_ACNT] WHERE ACNT_CODE = '" + paraVer2 + "'";
                    SqlCommand cmdCheck = new SqlCommand(queryDiario, connection);
                    SqlDataReader reader = cmdCheck.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            descr_acntC = reader.GetString(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Error Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }



            String space = "                                                          ";
            PdfFileWriter.TextBox BoxToPDF = new PdfFileWriter.TextBox(WidthToPDF, 0.25);
            int l;
            StringBuilder debitos = new StringBuilder("");
            for(l=0;l<con;l++)
            {
                debitos.Append("\n         Debito: $" + arrayCantidad[l] +" - " +arrayCuentas[l] + " - " + arrayDescr[l]);
            }
            String aver = "\n\n\n\n\n" +
            space +
            "Unión Mexicana del Norte" +
            "\n" + space +
            "Carretera Nacional Km 205" + "\n" + space +
            "       El Desague                                        Fecha: " + fecha + "\n" + space +
            "Montemorelos, Nuevo León                     Reference: " + anal + "\n" +
            "\n\n" +
            "\n\n" +
            "        Pagarse a: " + nombre +
            "\n" +
            "        Concepto: " + preconcepto + "\n" +
            "        Cantidad: " + total + "\n"+
            "        Bueno por: " + numeroAletras + "\n\n" +"\n\n\n\n\n\n" +
            "      ______________________                        ____________________               ____________________\n" +
            "           Processed by: " + JRNAL_SRCE + "                                         Approved by                                    Received by " +
            "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" +
            "         Crédito: $" +total+" - "+ ACCNT_CODE + " - " + descr_acntC.Trim() +debitos.ToString();
            BoxToPDF.AddText(ArialNormalToPDF, FontSizeToPDF,aver);





            StringBuilder espacios = new StringBuilder("");
            int j = 0;
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.sunEspacios1); j++)
            {
                espacios.Append(" ");
            }
            StringBuilder entersPrimeros = new StringBuilder("");
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.entersPrimeros1); j++)
            {
                entersPrimeros.Append("\n");
            }
            StringBuilder espaciosDeFecha = new StringBuilder("");
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.espaciosDeFecha1); j++)
            {
                espaciosDeFecha.Append(" ");
            }
            StringBuilder entersEntreFechaYNombre = new StringBuilder("");
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.entersEntreFechaYNombre1); j++)
            {
                entersEntreFechaYNombre.Append("\n");
            }
            StringBuilder entersEntreNombreYLetras = new StringBuilder("");
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.entersEntreNombreYLetras1); j++)
            {
                entersEntreNombreYLetras.Append("\n");
            }
            StringBuilder espaciosEntreNombreYTotal = new StringBuilder("");
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.espaciosEntreNombreYTotal1); j++)
            {
                espaciosEntreNombreYTotal.Append(" ");
            }

            PdfFileWriter.TextBox Box = new PdfFileWriter.TextBox(Width, 0.25);
            Box.AddText(ArialNormal, FontSize,
             entersPrimeros.ToString() +
            espaciosDeFecha.ToString() +
             fecha +
             entersEntreFechaYNombre.ToString() +
           espacios.ToString() +//64
             nombre + espaciosEntreNombreYTotal.ToString() + total +
             entersEntreNombreYLetras.ToString() +
            espacios.ToString() +
             numeroAletras);

            BoxToPDF.AddText(ArialNormalToPDF, FontSizeToPDF, "\n");
            Double PosYToPDF = HeightToPDF;
            ContentsToPDF.DrawText(0.0, ref PosYToPDF, 0.0, 0, 0.015, 0.05, TextBoxJustify.FitToWidth, BoxToPDF);
            ContentsToPDF.RestoreGraphicsState();
            ContentsToPDF.SaveGraphicsState();
            ContentsToPDF.RestoreGraphicsState();
            DocumentToPDF.CreateFile();

            Box.AddText(ArialNormal, FontSize, "\n");
            Double PosY = Height;
            Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.015, 0.05, TextBoxJustify.FitToWidth, Box);
            Contents.RestoreGraphicsState();
            Contents.SaveGraphicsState();
            Contents.RestoreGraphicsState();
            Document.CreateFile();


            previewCheque form = new previewCheque(tipoDeBancoGlobal);
                form.nombre = nombre;
                form.total = total;
                form.concepto = preconcepto;
                form.numeroAletras = numeroAletras;
                form.fecha = fecha;
                form.dia = dia;
                form.ano = ano;
                form.mes = mes;
                form.origen = JRNAL_SRCE;
                
                form.folio = numeroDeCheque;
                form.numeroDiario = numeroDiario;
                form.FileName = FileName;
                form.FileNameToPDF = FileNameToPDF;
                form.vuelveAHacerElCheque(true);
                form.ShowDialog();
            

           
        }

        private void confirm_Load(object sender, EventArgs e)
        {
           
        }
    }
}
