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
using System.Web.UI;
namespace Cheques
{
    public partial class previewCheque : Form
    {
        public String FileNameToPDF { get; set; }
        public int tipoDeBancoGlobal { get; set; }
      
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
        public previewCheque(int tipoDeBanco)
        {
            InitializeComponent();
            tipoDeBancoGlobal = tipoDeBanco;
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
            string path = Properties.Settings.Default.letra+ ":" + (object)Path.DirectorySeparatorChar + "cheques";
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
            String fileImage = path + (object)Path.DirectorySeparatorChar + "cheque" + tipoDeBancoGlobal+".jpg";

            Document = new PdfDocument(PaperType.sobreprima, false, UnitOfMeasure.Point, FileName);
            DefineFontResources();
            DefineTilingPatternResource();
            PdfPage Page = new PdfPage(Document);
            PdfContents Contents = new PdfContents(Page);
           
            Contents.SaveGraphicsState();
            Contents.Translate(0.1, 0.1);

            const Double FontSize = 9.0;
         
            if(conImagen)
            {
                PdfImage Image = new PdfImage(Document, fileImage, 72.0, 50);
                Contents.SaveGraphicsState();
                int top = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "topCheque" + tipoDeBancoGlobal));
                int left = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "leftCheque" + tipoDeBancoGlobal));
                int ancho = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "anchoCheque" + tipoDeBancoGlobal));
                int largo = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "largoCheque" + tipoDeBancoGlobal));
                Contents.DrawImage(Image, left, top, ancho, largo);
                Contents.RestoreGraphicsState();
            }


            int fechaX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "fechaX" + tipoDeBancoGlobal));
            int fechaY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "fechaY" + tipoDeBancoGlobal));
            Contents.MoveTo(new PointD(fechaX, fechaY));
            Contents.DrawText(ArialNormal, FontSize, fechaX, fechaY, fecha);
            Contents.RestoreGraphicsState();
            Contents.SaveGraphicsState();

            int nombreX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "nombreX" + tipoDeBancoGlobal));
            int nombreY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "nombreY" + tipoDeBancoGlobal));
            Contents.MoveTo(new PointD(nombreX, nombreY));
            Contents.DrawText(ArialNormal, FontSize, nombreX, nombreY, nombre);
            Contents.RestoreGraphicsState();
            Contents.SaveGraphicsState();

            int cantidadX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "cantidadX" + tipoDeBancoGlobal));
            int cantidadY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "cantidadY" + tipoDeBancoGlobal));
            Contents.MoveTo(new PointD(cantidadX, cantidadY));
            Contents.DrawText(ArialNormal, FontSize, cantidadX, cantidadY, total);
            Contents.RestoreGraphicsState();
            Contents.SaveGraphicsState();

            int letraX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "letraX" + tipoDeBancoGlobal));
            int letraY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "letraY" + tipoDeBancoGlobal));
            Contents.MoveTo(new PointD(letraX, letraY));
            Contents.DrawText(ArialNormal, FontSize, letraX, letraY, numeroAletras);
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
            entersPrimeros.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "fechaX" + tipoDeBancoGlobal));
            espaciosIzquierda.KeyPress += espaciosIzquierda_KeyPress;
            espaciosIzquierda.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "nombreX" + tipoDeBancoGlobal));
                

            espaciosDeFecha.KeyPress += espaciosDeFecha_KeyPress;
            espaciosDeFecha.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "cantidadX" + tipoDeBancoGlobal));

            entersEntreFechaYNombre.KeyPress += entersEntreFechaYNombre_KeyPress;
            entersEntreFechaYNombre.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "cantidadY" + tipoDeBancoGlobal));

            entersEntreNombreYLetras.KeyPress += entersEntreNombreYLetras_KeyPress;
            entersEntreNombreYLetras.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "fechaY" + tipoDeBancoGlobal));

            espaciosEntreNombreYTotal.KeyPress += espaciosEntreNombreYTotal_KeyPress;
            espaciosEntreNombreYTotal.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "nombreY" + tipoDeBancoGlobal));

            leftCheque.KeyPress += leftCheque_KeyPress;
            leftCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "leftCheque" + tipoDeBancoGlobal));

            topCheque.KeyPress += topCheque_KeyPress;
            topCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "topCheque" + tipoDeBancoGlobal));

            anchoCheque.KeyPress += anchoCheque_KeyPress;
            anchoCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "anchoCheque" + tipoDeBancoGlobal));

            largoCheque.KeyPress += largoCheque_KeyPress;
            largoCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "largoCheque" + tipoDeBancoGlobal));

            letraXText.KeyPress += letraXText_KeyPress;
            letraXText.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "letraX" + tipoDeBancoGlobal));

            letraYText.KeyPress += letraYText_KeyPress;
            letraYText.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "letraY" + tipoDeBancoGlobal));

            
          
        }
        private void letraYText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int letraY = Convert.ToInt32(letraYText.Text);
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.letraY1 = "" + letraY;
                        break;
                    case 2:
                        Properties.Settings.Default.letraY2 = "" + letraY;
                        break;
                    case 3:
                        Properties.Settings.Default.letraY3 = "" + letraY;
                        break;
                    case 4:
                        Properties.Settings.Default.letraY4 = "" + letraY;
                        break;
                }
                letraYText.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "letraY" + tipoDeBancoGlobal));
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void letraXText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int letraX = Convert.ToInt32(letraXText.Text);
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.letraX1 = "" + letraX;
                        break;
                    case 2:
                        Properties.Settings.Default.letraX2 = "" + letraX;
                        break;
                    case 3:
                        Properties.Settings.Default.letraX3 = "" + letraX;
                        break;
                    case 4:
                        Properties.Settings.Default.letraX4 = "" + letraX;
                        break;
                }
                letraXText.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "letraX" + tipoDeBancoGlobal));
                vuelveAHacerElCheque(true);
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void largoCheque_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int cuantos = Convert.ToInt32(largoCheque.Text);
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.largoCheque1 = "" + cuantos;
                        break;
                    case 2:
                        Properties.Settings.Default.largoCheque2 = "" + cuantos;
                        break;
                    case 3:
                        Properties.Settings.Default.largoCheque3 = "" + cuantos;
                        break;
                }
                largoCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "largoCheque" + tipoDeBancoGlobal));
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
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.anchoCheque1 = "" + cuantos;
                        break;
                    case 2:
                        Properties.Settings.Default.anchoCheque2 = "" + cuantos;
                        break;
                    case 3:
                        Properties.Settings.Default.anchoCheque3 = "" + cuantos;
                        break;
                }
                anchoCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "anchoCheque" + tipoDeBancoGlobal));
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
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.topCheque1 = "" + cuantos;
                        break;
                    case 2:
                        Properties.Settings.Default.topCheque2 = "" + cuantos;
                        break;
                    case 3:
                        Properties.Settings.Default.topCheque3 = "" + cuantos;
                        break;
                }
                topCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "topCheque" + tipoDeBancoGlobal));
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
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.leftCheque1 = "" + cuantos;
                        break;
                    case 2:
                        Properties.Settings.Default.leftCheque2 = "" + cuantos;
                        break;
                    case 3:
                        Properties.Settings.Default.leftCheque3 = "" + cuantos;
                        break;
                }
                leftCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "leftCheque" + tipoDeBancoGlobal));
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
                int nombreY = Convert.ToInt32(espaciosEntreNombreYTotal.Text);
               switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.nombreY1 = "" + nombreY;
                        break;
                    case 2:
                        Properties.Settings.Default.nombreY2 = "" + nombreY;
                        break;
                    case 3:
                        Properties.Settings.Default.nombreY3 = "" + nombreY;
                        break;
                    case 4:
                        Properties.Settings.Default.nombreY4 = "" + nombreY;
                        break;
                }
               espaciosEntreNombreYTotal.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "nombreY" + tipoDeBancoGlobal));
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
                int fechaY = Convert.ToInt32(entersEntreNombreYLetras.Text);
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.fechaY1 = "" + fechaY;
                        break;
                    case 2:
                        Properties.Settings.Default.fechaY2 = "" + fechaY;
                        break;
                    case 3:
                        Properties.Settings.Default.fechaY3 = "" + fechaY;
                        break;
                    case 4:
                        Properties.Settings.Default.fechaY4 = "" + fechaY;
                        break;
                }
                entersEntreNombreYLetras.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "fechaY" + tipoDeBancoGlobal));
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
                int cantidadY = Convert.ToInt32(entersEntreFechaYNombre.Text);
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.cantidadY1 = "" + cantidadY;
                        break;
                    case 2:
                        Properties.Settings.Default.cantidadY2 = "" + cantidadY;
                        break;
                    case 3:
                        Properties.Settings.Default.cantidadY3 = "" + cantidadY;
                        break;
                    case 4:
                        Properties.Settings.Default.cantidadY4 = "" + cantidadY;
                        break;
                }
                entersEntreFechaYNombre.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "cantidadY" + tipoDeBancoGlobal));
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
                int cantidadX = Convert.ToInt32(espaciosDeFecha.Text);
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.cantidadX1 = "" + cantidadX;
                        break;
                    case 2:
                        Properties.Settings.Default.cantidadX2 = "" + cantidadX;
                        break;
                    case 3:
                        Properties.Settings.Default.cantidadX3 = "" + cantidadX;
                        break;
                    case 4:
                        Properties.Settings.Default.cantidadX4 = "" + cantidadX;
                        break;
                }
                espaciosDeFecha.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "cantidadX" + tipoDeBancoGlobal));
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
                int nombreX = Convert.ToInt32(espaciosIzquierda.Text);
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.nombreX1 = "" + nombreX;
                        break;
                    case 2:
                        Properties.Settings.Default.nombreX2 = "" + nombreX;
                        break;
                    case 3:
                        Properties.Settings.Default.nombreX3 = "" + nombreX;
                        break;
                    case 4:
                        Properties.Settings.Default.nombreX4 = "" + nombreX;
                        break;
                }
                espaciosIzquierda.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "nombreX" + tipoDeBancoGlobal));
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
                int fechaX = Convert.ToInt32(entersPrimeros.Text);
                switch (tipoDeBancoGlobal)
                {
                    case 1:
                        Properties.Settings.Default.fechaX1 = "" + fechaX;
                        break;
                    case 2:
                        Properties.Settings.Default.fechaX2 = "" + fechaX;
                        break;
                    case 3:
                        Properties.Settings.Default.fechaX3 = "" + fechaX;
                        break;
                    case 4:
                        Properties.Settings.Default.fechaX4 = "" + fechaX;
                        break;
                }
                entersPrimeros.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "fechaX" + tipoDeBancoGlobal));
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
            int fechaX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "fechaX" + tipoDeBancoGlobal));
            fechaX++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.fechaX1 = "" + fechaX;
                    break;
                case 2:
                    Properties.Settings.Default.fechaX2 = "" + fechaX;
                    break;
                case 3:
                    Properties.Settings.Default.fechaX3 = "" + fechaX;
                    break;
                case 4:
                    Properties.Settings.Default.fechaX4 = "" + fechaX;
                    break;
            }
            entersPrimeros.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "fechaX" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void menosEntersPrimeros_Click(object sender, EventArgs e)
        {
            int fechaX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "fechaX" + tipoDeBancoGlobal));
            fechaX--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.fechaX1 = "" + fechaX;
                    break;
                case 2:
                    Properties.Settings.Default.fechaX2 = "" + fechaX;
                    break;
                case 3:
                    Properties.Settings.Default.fechaX3 = "" + fechaX;
                    break;
                case 4:
                    Properties.Settings.Default.fechaX4 = "" + fechaX;
                    break;
            }
            entersPrimeros.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "fechaX" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void menosEspaciosIzquierda_Click(object sender, EventArgs e)
        {
            int nombreX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "nombreX" + tipoDeBancoGlobal));
            nombreX--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.nombreX1 = "" + nombreX;
                    break;
                case 2:
                    Properties.Settings.Default.nombreX2 = "" + nombreX;
                    break;
                case 3:
                    Properties.Settings.Default.nombreX3 = "" + nombreX;
                    break;
                case 4:
                    Properties.Settings.Default.nombreX4 = "" + nombreX;
                    break;
            }
            espaciosIzquierda.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "nombreX" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masEspaciosIzquierda_Click(object sender, EventArgs e)
        {
            int nombreX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "nombreX" + tipoDeBancoGlobal));
            nombreX++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.nombreX1 = "" + nombreX;
                    break;
                case 2:
                    Properties.Settings.Default.nombreX2 = "" + nombreX;
                    break;
                case 3:
                    Properties.Settings.Default.nombreX3 = "" + nombreX;
                    break;
                case 4:
                    Properties.Settings.Default.nombreX4 = "" + nombreX;
                    break;
            }
            espaciosIzquierda.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "nombreX" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void menosEspaciosDeFecha_Click(object sender, EventArgs e)
        {
            int cantidadX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "cantidadX" + tipoDeBancoGlobal));
            cantidadX--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.cantidadX1 = "" + cantidadX;
                    break;
                case 2:
                    Properties.Settings.Default.cantidadX2 = "" + cantidadX;
                    break;
                case 3:
                    Properties.Settings.Default.cantidadX3 = "" + cantidadX;
                    break;
                case 4:
                    Properties.Settings.Default.cantidadX4 = "" + cantidadX;
                    break;
            }
            espaciosDeFecha.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "cantidadX" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masEspaciosDeFecha_Click(object sender, EventArgs e)
        {
            int cantidadX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "cantidadX" + tipoDeBancoGlobal));
            cantidadX++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.cantidadX1 = "" + cantidadX;
                    break;
                case 2:
                    Properties.Settings.Default.cantidadX2 = "" + cantidadX;
                    break;
                case 3:
                    Properties.Settings.Default.cantidadX3 = "" + cantidadX;
                    break;
                case 4:
                    Properties.Settings.Default.cantidadX4 = "" + cantidadX;
                    break;
            }
            espaciosDeFecha.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "cantidadX" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void menosEntersEntreFechaYNombre_Click(object sender, EventArgs e)
        {
            int cantidadY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "cantidadY" + tipoDeBancoGlobal));
            cantidadY--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.cantidadY1 = "" + cantidadY;
                    break;
                case 2:
                    Properties.Settings.Default.cantidadY2 = "" + cantidadY;
                    break;
                case 3:
                    Properties.Settings.Default.cantidadY3 = "" + cantidadY;
                    break;
                case 4:
                    Properties.Settings.Default.cantidadY4 = "" + cantidadY;
                    break;
            }
            entersEntreFechaYNombre.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "cantidadY" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masEntersEntreFechaYNombre_Click(object sender, EventArgs e)
        {
            int cantidadY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "cantidadY" + tipoDeBancoGlobal));
            cantidadY++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.cantidadY1 = "" + cantidadY;
                    break;
                case 2:
                    Properties.Settings.Default.cantidadY2 = "" + cantidadY;
                    break;
                case 3:
                    Properties.Settings.Default.cantidadY3 = "" + cantidadY;
                    break;
                case 4:
                    Properties.Settings.Default.cantidadY4 = "" + cantidadY;
                    break;
            }
            Properties.Settings.Default.entersEntreFechaYNombre1 = "" + cantidadY;
            entersEntreFechaYNombre.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "cantidadY" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void menosEntersEntreNombreYLetras_Click(object sender, EventArgs e)
        {
            int fechaY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "fechaY" + tipoDeBancoGlobal));
            fechaY--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.fechaY1 = "" + fechaY;
                    break;
                case 2:
                    Properties.Settings.Default.fechaY2 = "" + fechaY;
                    break;
                case 3:
                    Properties.Settings.Default.fechaY3 = "" + fechaY;
                    break;
                case 4:
                    Properties.Settings.Default.fechaY4 = "" + fechaY;
                    break;
            }
            entersEntreNombreYLetras.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "fechaY" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masEntersEntreNombreYLetras_Click(object sender, EventArgs e)
        {
            int fechaY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "fechaY" + tipoDeBancoGlobal));
            fechaY++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.fechaY1 = "" + fechaY;
                    break;
                case 2:
                    Properties.Settings.Default.fechaY2 = "" + fechaY;
                    break;
                case 3:
                    Properties.Settings.Default.fechaY3 = "" + fechaY;
                    break;
                case 4:
                    Properties.Settings.Default.fechaY4 = "" + fechaY;
                    break;
            }
            entersEntreNombreYLetras.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "fechaY" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void menosEspaciosEntreNombreYTotal_Click(object sender, EventArgs e)
        {
            int nombreY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "nombreY" + tipoDeBancoGlobal));
            nombreY--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.nombreY1 = "" + nombreY;
                    break;
                case 2:
                    Properties.Settings.Default.nombreY2 = "" + nombreY;
                    break;
                case 3:
                    Properties.Settings.Default.nombreY3 = "" + nombreY;
                    break;
                case 4:
                    Properties.Settings.Default.nombreY4 = "" + nombreY;
                    break;
            }
            espaciosEntreNombreYTotal.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "nombreY" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masEspaciosEntreNombreYTotal_Click(object sender, EventArgs e)
        {
            int nombreY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "nombreY" + tipoDeBancoGlobal));
            nombreY++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.nombreY1 = "" + nombreY;
                    break;
                case 2:
                    Properties.Settings.Default.nombreY2 = "" + nombreY;
                    break;
                case 3:
                    Properties.Settings.Default.nombreY3 = "" + nombreY;
                    break;
                case 4:
                    Properties.Settings.Default.nombreY4 = "" + nombreY;
                    break;
            }
            espaciosEntreNombreYTotal.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "nombreY" + tipoDeBancoGlobal));
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
            int cuantos = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "leftCheque" + tipoDeBancoGlobal));
            cuantos--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.leftCheque1 = "" + cuantos;
                    break;
                case 2:
                    Properties.Settings.Default.leftCheque2 = "" + cuantos;
                    break;
                case 3:
                    Properties.Settings.Default.leftCheque3 = "" + cuantos;
                    break;
            }
            leftCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "leftCheque" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masLeftCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "leftCheque" + tipoDeBancoGlobal));
            cuantos++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.leftCheque1 = "" + cuantos;
                    break;
                case 2:
                    Properties.Settings.Default.leftCheque2 = "" + cuantos;
                    break;
                case 3:
                    Properties.Settings.Default.leftCheque3 = "" + cuantos;
                    break;
            }
            leftCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "leftCheque" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void menosTopCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "topCheque" + tipoDeBancoGlobal));
            cuantos--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.topCheque1 = "" + cuantos;
                    break;
                case 2:
                    Properties.Settings.Default.topCheque2 = "" + cuantos;
                    break;
                case 3:
                    Properties.Settings.Default.topCheque3 = "" + cuantos;
                    break;
            }
            topCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "topCheque" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masTopCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "topCheque" + tipoDeBancoGlobal));
            cuantos++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.topCheque1 = "" + cuantos;
                    break;
                case 2:
                    Properties.Settings.Default.topCheque2 = "" + cuantos;
                    break;
                case 3:
                    Properties.Settings.Default.topCheque3 = "" + cuantos;
                    break;
            }
            topCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "topCheque" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void menosAnchoCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "anchoCheque" + tipoDeBancoGlobal));
            cuantos--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.anchoCheque1 = "" + cuantos;
                    break;
                case 2:
                    Properties.Settings.Default.anchoCheque2 = "" + cuantos;
                    break;
                case 3:
                    Properties.Settings.Default.anchoCheque3 = "" + cuantos;
                    break;
            }
            anchoCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "anchoCheque" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masAnchoCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "anchoCheque" + tipoDeBancoGlobal));
            cuantos++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.anchoCheque1 = "" + cuantos;
                    break;
                case 2:
                    Properties.Settings.Default.anchoCheque2 = "" + cuantos;
                    break;
                case 3:
                    Properties.Settings.Default.anchoCheque3 = "" + cuantos;
                    break;
            }
            anchoCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "anchoCheque" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void menosLargoCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "largoCheque" + tipoDeBancoGlobal));
            cuantos--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.largoCheque1 = "" + cuantos;
                    break;
                case 2:
                    Properties.Settings.Default.largoCheque2 = "" + cuantos;
                    break;
                case 3:
                    Properties.Settings.Default.largoCheque3 = "" + cuantos;
                    break;
            }
            largoCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "largoCheque" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masLargoCheque_Click(object sender, EventArgs e)
        {
            int cuantos = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "largoCheque" + tipoDeBancoGlobal));
            cuantos++;
            switch(tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.largoCheque1 =""+ cuantos;
                break;
                case 2:
                    Properties.Settings.Default.largoCheque2 = "" + cuantos;
                break;
                case 3:
                    Properties.Settings.Default.largoCheque3 = "" + cuantos;
                break;
            }
            largoCheque.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "largoCheque" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void entersPrimeros_TextChanged(object sender, EventArgs e)
        {

        }

        private void menosLetraX_Click(object sender, EventArgs e)
        {
            int letraX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "letraX" + tipoDeBancoGlobal));
            letraX--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.letraX1 = "" + letraX;
                    break;
                case 2:
                    Properties.Settings.Default.letraX2 = "" + letraX;
                    break;
                case 3:
                    Properties.Settings.Default.letraX3 = "" + letraX;
                    break;
                case 4:
                    Properties.Settings.Default.letraX4 = "" + letraX;
                    break;
            }
            letraXText.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "letraX" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masLetraX_Click(object sender, EventArgs e)
        {
            int letraX = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "letraX" + tipoDeBancoGlobal));
            letraX++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.letraX1 = "" + letraX;
                    break;
                case 2:
                    Properties.Settings.Default.letraX2 = "" + letraX;
                    break;
                case 3:
                    Properties.Settings.Default.letraX3 = "" + letraX;
                    break;
                case 4:
                    Properties.Settings.Default.letraX4 = "" + letraX;
                    break;
            }
            letraXText.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "letraX" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void letraXText_TextChanged(object sender, EventArgs e)
        {

        }

        private void menosLetraY_Click(object sender, EventArgs e)
        {
            int letraY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "letraY" + tipoDeBancoGlobal));
            letraY--;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.letraY1 = "" + letraY;
                    break;
                case 2:
                    Properties.Settings.Default.letraY2 = "" + letraY;
                    break;
                case 3:
                    Properties.Settings.Default.letraY3 = "" + letraY;
                    break;
                case 4:
                    Properties.Settings.Default.letraY4 = "" + letraY;
                    break;
            }
            letraYText.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "letraY" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }

        private void masLetraY_Click(object sender, EventArgs e)
        {
            int letraY = Convert.ToInt32(DataBinder.Eval(Properties.Settings.Default, "letraY" + tipoDeBancoGlobal));
            letraY++;
            switch (tipoDeBancoGlobal)
            {
                case 1:
                    Properties.Settings.Default.letraY1 = "" + letraY;
                    break;
                case 2:
                    Properties.Settings.Default.letraY2 = "" + letraY;
                    break;
                case 3:
                    Properties.Settings.Default.letraY3 = "" + letraY;
                    break;
                case 4:
                    Properties.Settings.Default.letraY4 = "" + letraY;
                    break;
            }
            letraYText.Text = Convert.ToString(DataBinder.Eval(Properties.Settings.Default, "letraY" + tipoDeBancoGlobal));
            vuelveAHacerElCheque(true);
        }


     }

       
    }

