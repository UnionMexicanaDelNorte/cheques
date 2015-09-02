using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PdfFileWriter;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.SqlClient;
namespace Cheques
{
    public partial class previewCheque : Form
    {
        public String FileNameToPDF { get; set; }
       
        public String fecha { get; set; }
        
        public String nombre { get; set; }
        public String total { get; set; }
        public String numeroAletras { get; set; }
        public String origen { get; set; }
        public String folio { get; set; }
        public String orden { get; set; }
        public String dia { get; set; }
        public String mes { get; set; }
        public String ano { get; set; }
        public String concepto { get; set; }
        public String numeroDiario { get; set; }
       
        

        public String FileName { get; set; }
        public PdfFont ArialNormal { get; set; }
        public PdfFont ArialBold { get; set; }
        public PdfFont ArialItalic { get; set; }
        public PdfFont ArialBoldItalic { get; set; }
        public PdfFont TimesNormal { get; set; }
        public PdfFont Comic { get; set; }
        public PdfDocument Document { get; set; }
        public PdfTilingPattern WaterMark { get; set; }
     
        public previewCheque()
        {
            InitializeComponent();
        }

        // Define Tiling Pattern Resource
        private void DefineTilingPatternResource()
        {

            // create empty tiling pattern
            WaterMark = new PdfTilingPattern(Document);
           
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

        
            BaseLine += BoxHeight / 2;

            WaterMark.DrawText(ArialBold, FontSize, 0.0, BaseLine, TextJustify.Center, Mark);
            WaterMark.DrawText(ArialBold, FontSize, BoxWidth, BaseLine, TextJustify.Center, Mark);
            WaterMark.RestoreGraphicsState();

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

          
            // substitute one character for another
            // this program supports characters 32 to 126 and 160 to 255
            // if a font has a character outside these ranges that is required by the application,
            // you can replace an unused character with this character
            // Note: space (32) and non breaking space (160) cannot be replaced
            ArialNormal.CharSubstitution(9679, 9679, 161);		// euro
            ArialNormal.CharSubstitution(1488, 1514, 162);		// hebrew
            ArialNormal.CharSubstitution(1040, 1045, 189);		// russian
            ArialNormal.CharSubstitution(945, 950, 195);		// greek

           
            return;
        }

        public void vuelveAHacerElCheque(bool conImagen)
        {
            string path = "S:" + (object)Path.DirectorySeparatorChar + "cheques";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
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
            String fileImage = path +  (object)Path.DirectorySeparatorChar+"cheque.jpg";
            Document = new PdfDocument(PaperType.sobreprima, false, UnitOfMeasure.Point, FileName);
            DefineFontResources();
            DefineTilingPatternResource();
            PdfPage Page = new PdfPage(Document);
            PdfContents Contents = new PdfContents(Page);

            Contents.SaveGraphicsState();
            Contents.Translate(0.1, 0.1);

            const Double Width = 632;
            const Double Height = 288;
            const Double FontSize = 9.0;
            StringBuilder espacios = new StringBuilder("");
            int j = 0;
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.sunEspacios); j++)
            {
                espacios.Append(" ");
            }
            StringBuilder entersPrimeros = new StringBuilder("");
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.entersPrimeros); j++)
            {
                entersPrimeros.Append("\n");
            }
            StringBuilder espaciosDeFecha = new StringBuilder("");
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.espaciosDeFecha); j++)
            {
                espaciosDeFecha.Append(" ");
            }
            StringBuilder entersEntreFechaYNombre = new StringBuilder("");
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.entersEntreFechaYNombre); j++)
            {
                entersEntreFechaYNombre.Append("\n");
            }
            StringBuilder entersEntreNombreYLetras = new StringBuilder("");
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.entersEntreNombreYLetras); j++)
            {
                entersEntreNombreYLetras.Append("\n");
            }
            StringBuilder espaciosEntreNombreYTotal = new StringBuilder("");
            for (j = 0; j < Convert.ToInt32(Properties.Settings.Default.espaciosEntreNombreYTotal); j++)
            {
                espaciosEntreNombreYTotal.Append(" ");
            }

            if(conImagen)
            {
                PdfImage Image = new PdfImage(Document, fileImage, 72.0, 50);
                // save graphics state
                Contents.SaveGraphicsState();
                // translate coordinate origin to the center of the picture
                //Contents.Translate(2.6, 5.0);
               // ImageSizePos NewSize = Image.ImageSizePosition(1.75, 1.5, ContentAlignment.MiddleCenter);
                
                // clipping path
               // Contents.DrawOval(50, 50, 469, 292, PaintOp.ClipPathEor);

                // draw image
                
                Contents.DrawImage(Image, Convert.ToInt32( Properties.Settings.Default.leftCheque),Convert.ToInt32( Properties.Settings.Default.topCheque), Convert.ToInt32(Properties.Settings.Default.anchoCheque), Convert.ToInt32(Properties.Settings.Default.largoCheque));

                // restore graphics state
                Contents.RestoreGraphicsState();

            }
/*
          
          
            // adjust image size and preserve aspect ratio  ImageSizePos
            ImageSizePos NewSize = Image.ImageSizePosition(1.75, 1.5, ContentAlignment.MiddleCenter);
            
            // clipping path  469, 292
            Contents.DrawOval(NewSize.DeltaX,NewSize.DeltaY ,NewSize.Width, NewSize.Height, PaintOp.ClipPathEor);

            // draw image
            Contents.DrawImage(Image, NewSize.DeltaX, NewSize.DeltaY, NewSize.Width, NewSize.Height);

            // restore graphics state
            Contents.RestoreGraphicsState();
            */

            String aver = entersPrimeros.ToString() +
            espaciosDeFecha.ToString() +
             fecha +
             entersEntreFechaYNombre.ToString() +
           espacios.ToString() +
             nombre + espaciosEntreNombreYTotal.ToString() + total +
             entersEntreNombreYLetras.ToString() +
            espacios.ToString() +
             numeroAletras;

            PdfFileWriter.TextBox Box = new PdfFileWriter.TextBox(Width, 0.25);
            Box.AddText(ArialNormal, FontSize,aver);


            Box.AddText(ArialNormal, FontSize, "\n");
            Double PosY = Height;
            Contents.DrawText(0.0, ref PosY, 0.0, 0, 0.015, 0.05, TextBoxJustify.FitToWidth, Box);
            Contents.RestoreGraphicsState();
            Contents.SaveGraphicsState();
            Contents.RestoreGraphicsState();
            Document.CreateFile();
            
           
            String url = "file:" + (object)Path.DirectorySeparatorChar + (object)Path.DirectorySeparatorChar +FileName;
            // this.webBrowser1.Navigate(new Uri(url));
            this.webView1.ZoomFactor = 1.0;
            this.webView1.Url = url;
            Properties.Settings.Default.Save();
        }

        private void previewCheque_Load(object sender, EventArgs e)
        {
            entersPrimeros.KeyPress +=entersPrimeros_KeyPress;
            entersPrimeros.Text = Properties.Settings.Default.entersPrimeros;
            espaciosIzquierda.KeyPress += espaciosIzquierda_KeyPress;
            espaciosIzquierda.Text = Properties.Settings.Default.sunEspacios;

            espaciosDeFecha.KeyPress += espaciosDeFecha_KeyPress;
            espaciosDeFecha.Text = Properties.Settings.Default.espaciosDeFecha;

            entersEntreFechaYNombre.KeyPress += entersEntreFechaYNombre_KeyPress;
            entersEntreFechaYNombre.Text = Properties.Settings.Default.entersEntreFechaYNombre;

            entersEntreNombreYLetras.KeyPress += entersEntreNombreYLetras_KeyPress;
            entersEntreNombreYLetras.Text = Properties.Settings.Default.entersEntreNombreYLetras;

            espaciosEntreNombreYTotal.KeyPress += espaciosEntreNombreYTotal_KeyPress;
            espaciosEntreNombreYTotal.Text = Properties.Settings.Default.espaciosEntreNombreYTotal;

            leftCheque.KeyPress += leftCheque_KeyPress;
            leftCheque.Text = Properties.Settings.Default.leftCheque;

            topCheque.KeyPress += topCheque_KeyPress;
            topCheque.Text = Properties.Settings.Default.topCheque;

            anchoCheque.KeyPress += anchoCheque_KeyPress;
            anchoCheque.Text = Properties.Settings.Default.anchoCheque;

            largoCheque.KeyPress += largoCheque_KeyPress;
            largoCheque.Text = Properties.Settings.Default.largoCheque;


            
            
          
        }


        private void largoCheque_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int cuantos = Convert.ToInt32(largoCheque.Text);
                Properties.Settings.Default.largoCheque = "" + cuantos;
                largoCheque.Text = Properties.Settings.Default.largoCheque;
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void anchoCheque_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int cuantos = Convert.ToInt32(anchoCheque.Text);
                Properties.Settings.Default.anchoCheque = "" + cuantos;
                anchoCheque.Text = Properties.Settings.Default.anchoCheque;
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }


        private void topCheque_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int cuantos = Convert.ToInt32(topCheque.Text);
                Properties.Settings.Default.topCheque = "" + cuantos;
                topCheque.Text = Properties.Settings.Default.topCheque;
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
        }


        private void leftCheque_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int cuantos = Convert.ToInt32(leftCheque.Text);
                Properties.Settings.Default.leftCheque = "" + cuantos;
                leftCheque.Text = Properties.Settings.Default.leftCheque;
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }


           private void espaciosEntreNombreYTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int cuantos = Convert.ToInt32(espaciosEntreNombreYTotal.Text);
                Properties.Settings.Default.espaciosEntreNombreYTotal = "" + cuantos;
                espaciosEntreNombreYTotal.Text = Properties.Settings.Default.espaciosEntreNombreYTotal;
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
          private void entersEntreNombreYLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int cuantos = Convert.ToInt32(entersEntreNombreYLetras.Text);
                Properties.Settings.Default.entersEntreNombreYLetras = "" + cuantos;
                entersEntreNombreYLetras.Text = Properties.Settings.Default.entersEntreNombreYLetras;
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void entersEntreFechaYNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int cuantos = Convert.ToInt32(entersEntreFechaYNombre.Text);
                Properties.Settings.Default.entersEntreFechaYNombre = "" + cuantos;
                entersEntreFechaYNombre.Text = Properties.Settings.Default.entersEntreFechaYNombre;
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void espaciosDeFecha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int cuantos = Convert.ToInt32(espaciosDeFecha.Text);
                Properties.Settings.Default.espaciosDeFecha = "" + cuantos;
                espaciosDeFecha.Text = Properties.Settings.Default.espaciosDeFecha;
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void espaciosIzquierda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int cuantos = Convert.ToInt32(espaciosIzquierda.Text);
                Properties.Settings.Default.sunEspacios = "" + cuantos;
                espaciosIzquierda.Text = Properties.Settings.Default.sunEspacios;
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void entersPrimeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                int cuantos = Convert.ToInt32(entersPrimeros.Text);
               Properties.Settings.Default.entersPrimeros = "" + cuantos;
                entersPrimeros.Text = Properties.Settings.Default.entersPrimeros;
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }   
        }
       

        private void masEntersPrimeros_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32( Properties.Settings.Default.entersPrimeros);
            cuantos++;
            Properties.Settings.Default.entersPrimeros = "" + cuantos;
            entersPrimeros.Text = Properties.Settings.Default.entersPrimeros;
            vuelveAHacerElCheque(true);
        }

        private void menosEntersPrimeros_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.entersPrimeros);
            cuantos--;
            Properties.Settings.Default.entersPrimeros = "" + cuantos;
            entersPrimeros.Text = Properties.Settings.Default.entersPrimeros;
            vuelveAHacerElCheque(true);
        }

        private void menosEspaciosIzquierda_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.sunEspacios);
            cuantos--;
            Properties.Settings.Default.sunEspacios = "" + cuantos;
            espaciosIzquierda.Text = Properties.Settings.Default.sunEspacios;
            vuelveAHacerElCheque(true);
        }

        private void masEspaciosIzquierda_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.sunEspacios);
            cuantos++;
            Properties.Settings.Default.sunEspacios = "" + cuantos;
            espaciosIzquierda.Text = Properties.Settings.Default.sunEspacios;
            vuelveAHacerElCheque(true);
        }

        private void menosEspaciosDeFecha_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.espaciosDeFecha);
            cuantos--;
            Properties.Settings.Default.espaciosDeFecha = "" + cuantos;
            espaciosDeFecha.Text = Properties.Settings.Default.espaciosDeFecha;
            vuelveAHacerElCheque(true);
        }

        private void masEspaciosDeFecha_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.espaciosDeFecha);
            cuantos++;
            Properties.Settings.Default.espaciosDeFecha = "" + cuantos;
            espaciosDeFecha.Text = Properties.Settings.Default.espaciosDeFecha;
            vuelveAHacerElCheque(true);
        }

        private void menosEntersEntreFechaYNombre_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.entersEntreFechaYNombre);
            cuantos--;
            Properties.Settings.Default.entersEntreFechaYNombre = "" + cuantos;
            entersEntreFechaYNombre.Text = Properties.Settings.Default.entersEntreFechaYNombre;
            vuelveAHacerElCheque(true);
        }

        private void masEntersEntreFechaYNombre_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.entersEntreFechaYNombre);
            cuantos++;
            Properties.Settings.Default.entersEntreFechaYNombre = "" + cuantos;
            entersEntreFechaYNombre.Text = Properties.Settings.Default.entersEntreFechaYNombre;
            vuelveAHacerElCheque(true);
        }

        private void menosEntersEntreNombreYLetras_Click(object sender, EventArgs e)
        {
             int cuantos = Convert.ToInt32(Properties.Settings.Default.entersEntreNombreYLetras);
            cuantos--;
            Properties.Settings.Default.entersEntreNombreYLetras = "" + cuantos;
            entersEntreNombreYLetras.Text = Properties.Settings.Default.entersEntreNombreYLetras;
            vuelveAHacerElCheque(true);
        }

        private void masEntersEntreNombreYLetras_Click(object sender, EventArgs e)
        {
         int cuantos = Convert.ToInt32(Properties.Settings.Default.entersEntreNombreYLetras);
            cuantos++;
            Properties.Settings.Default.entersEntreNombreYLetras = "" + cuantos;
            entersEntreNombreYLetras.Text = Properties.Settings.Default.entersEntreNombreYLetras;
            vuelveAHacerElCheque(true);
        }

        private void menosEspaciosEntreNombreYTotal_Click(object sender, EventArgs e)
        {
         int cuantos = Convert.ToInt32(Properties.Settings.Default.espaciosEntreNombreYTotal);
            cuantos--;
            Properties.Settings.Default.espaciosEntreNombreYTotal = "" + cuantos;
            espaciosEntreNombreYTotal.Text = Properties.Settings.Default.espaciosEntreNombreYTotal;
            vuelveAHacerElCheque(true);
        }

        private void masEspaciosEntreNombreYTotal_Click(object sender, EventArgs e)
        {
          int cuantos = Convert.ToInt32(Properties.Settings.Default.espaciosEntreNombreYTotal);
            cuantos++;
            Properties.Settings.Default.espaciosEntreNombreYTotal = "" + cuantos;
            espaciosEntreNombreYTotal.Text = Properties.Settings.Default.espaciosEntreNombreYTotal;
            vuelveAHacerElCheque(true);
        }

        private void imprimir_Click(object sender, EventArgs e)
        {
            vuelveAHacerElCheque(false);


            String connStringCheques = "Database=" + Properties.Settings.Default.DatabaseCheques + ";Data Source=" + Properties.Settings.Default.sunDatasource + ";Integrated Security=False;MultipleActiveResultSets=true;User ID='" + Properties.Settings.Default.sunUser + "';Password='" + Properties.Settings.Default.sunPassword + "';connect timeout = 10";

            try
            {
                using (SqlConnection connection = new SqlConnection(connStringCheques))
                {
                    connection.Open();

                    String query = "INSERT INTO [" + Properties.Settings.Default.DatabaseCheques + "].[dbo].[cheques] (origen,folio,orden,archivo,nombre,cantidad,numeroALetras,dia,mes,ano,concepto,numeroDiario) VALUES ('" + origen + "', '" + folio + "', '" + FileNameToPDF + "', '" + FileName + "', '" + nombre + "', '" + total + "', '" + numeroAletras + "' ,'" + dia + "','" + mes + "','" + ano + "','" + concepto + "','" + numeroDiario + "')";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                    ProcessStartInfo info = new ProcessStartInfo();
                    info.Verb = "print";
                    info.FileName = FileName;
                    info.CreateNoWindow = true;
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    Process p = new Process();
                    p.StartInfo = info;
                    p.Start();
                    p.WaitForInputIdle();
                    System.Threading.Thread.Sleep(3000);
                    if (false == p.CloseMainWindow())
                        p.Kill();

                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        System.Environment.Exit(1);
                    }


                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "Sunplusito cheques", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

                   



           
        }

        private void imprimirOrden_Click(object sender, EventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = FileNameToPDF;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = new Process();
            p.StartInfo = info;
            p.Start();
            p.WaitForInputIdle();
            System.Threading.Thread.Sleep(3000);
            if (false == p.CloseMainWindow())
                p.Kill();

           
        }

        private void menosLeftCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.leftCheque );
            cuantos--;
            Properties.Settings.Default.leftCheque = "" + cuantos;
            leftCheque.Text = Properties.Settings.Default.leftCheque;
            vuelveAHacerElCheque(true);
        }

        private void masLeftCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.leftCheque);
            cuantos++;
            Properties.Settings.Default.leftCheque = "" + cuantos;
            leftCheque.Text = Properties.Settings.Default.leftCheque;
            vuelveAHacerElCheque(true);
        }

        private void menosTopCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.topCheque);
            cuantos--;
            Properties.Settings.Default.topCheque = "" + cuantos;
            topCheque.Text = Properties.Settings.Default.topCheque;
            vuelveAHacerElCheque(true);
        }

        private void masTopCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.topCheque);
            cuantos++;
            Properties.Settings.Default.topCheque = "" + cuantos;
            topCheque.Text = Properties.Settings.Default.topCheque;
            vuelveAHacerElCheque(true);
        }

        private void menosAnchoCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.anchoCheque);
            cuantos--;
            Properties.Settings.Default.anchoCheque = "" + cuantos;
            anchoCheque.Text = Properties.Settings.Default.anchoCheque;
            vuelveAHacerElCheque(true);
        }

        private void masAnchoCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.anchoCheque);
            cuantos++;
            Properties.Settings.Default.anchoCheque = "" + cuantos;
            anchoCheque.Text = Properties.Settings.Default.anchoCheque;
            vuelveAHacerElCheque(true);
        }

        private void menosLargoCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.largoCheque);
            cuantos--;
            Properties.Settings.Default.largoCheque = "" + cuantos;
            largoCheque.Text = Properties.Settings.Default.largoCheque;
            vuelveAHacerElCheque(true);
        }

        private void masLargoCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(Properties.Settings.Default.largoCheque);
            cuantos++;
            Properties.Settings.Default.largoCheque = "" + cuantos;
            largoCheque.Text = Properties.Settings.Default.largoCheque;
            vuelveAHacerElCheque(true);
        }
     }

       
    }

