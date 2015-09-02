namespace Cheques
{
    partial class PorNumeroDeDiario
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PorNumeroDeDiario));
            this.label1 = new System.Windows.Forms.Label();
            this.numeroDiario = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.webControl1 = new EO.WebBrowser.WinForm.WebControl();
            this.webView1 = new EO.WebBrowser.WebView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(283, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Escribe el número de diario:";
            // 
            // numeroDiario
            // 
            this.numeroDiario.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeroDiario.Location = new System.Drawing.Point(368, 49);
            this.numeroDiario.Name = "numeroDiario";
            this.numeroDiario.Size = new System.Drawing.Size(183, 44);
            this.numeroDiario.TabIndex = 1;
            this.numeroDiario.TextChanged += new System.EventHandler(this.numeroDiario_TextChanged);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(44, 154);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1877, 388);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // webControl1
            // 
            this.webControl1.BackColor = System.Drawing.Color.White;
            this.webControl1.Location = new System.Drawing.Point(44, 589);
            this.webControl1.Name = "webControl1";
            this.webControl1.Size = new System.Drawing.Size(1877, 560);
            this.webControl1.TabIndex = 3;
            this.webControl1.Text = "webControl1";
            this.webControl1.WebView = this.webView1;
            // 
            // webView1
            // 
            this.webView1.AllowDropLoad = true;
            // 
            // PorNumeroDeDiario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1970, 1187);
            this.Controls.Add(this.webControl1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.numeroDiario);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PorNumeroDeDiario";
            this.Text = "Ver cheques por numero de diario";
            this.Load += new System.EventHandler(this.PorNumeroDeDiario_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox numeroDiario;
        private System.Windows.Forms.ListView listView1;
        private EO.WebBrowser.WinForm.WebControl webControl1;
        private EO.WebBrowser.WebView webView1;
    }
}