namespace Cheques
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.cancelar = new System.Windows.Forms.Button();
            this.numeroDiario = new System.Windows.Forms.TextBox();
            this.aceptar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBanco = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cancelar2 = new System.Windows.Forms.Button();
            this.comboBanco2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numeroDiario2 = new System.Windows.Forms.TextBox();
            this.aceptar2 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configuracionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.servidorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chequesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.porNúmeroDeDiarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.porMesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // cancelar
            // 
            this.cancelar.Location = new System.Drawing.Point(0, 0);
            this.cancelar.Name = "cancelar";
            this.cancelar.Size = new System.Drawing.Size(75, 23);
            this.cancelar.TabIndex = 0;
            // 
            // numeroDiario
            // 
            this.numeroDiario.Location = new System.Drawing.Point(87, 122);
            this.numeroDiario.Name = "numeroDiario";
            this.numeroDiario.Size = new System.Drawing.Size(278, 31);
            this.numeroDiario.TabIndex = 2;
            // 
            // aceptar
            // 
            this.aceptar.Location = new System.Drawing.Point(457, 222);
            this.aceptar.Name = "aceptar";
            this.aceptar.Size = new System.Drawing.Size(144, 52);
            this.aceptar.TabIndex = 3;
            this.aceptar.Text = "Aceptar";
            this.aceptar.UseVisualStyleBackColor = true;
            this.aceptar.Click += new System.EventHandler(this.aceptar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-17, -4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 26);
            this.label2.TabIndex = 4;
            this.label2.Text = "       ";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // comboBanco
            // 
            this.comboBanco.FormattingEnabled = true;
            this.comboBanco.Location = new System.Drawing.Point(457, 120);
            this.comboBanco.Name = "comboBanco";
            this.comboBanco.Size = new System.Drawing.Size(233, 34);
            this.comboBanco.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(452, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 26);
            this.label3.TabIndex = 6;
            this.label3.Text = "Banco:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(51, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(275, 38);
            this.label4.TabIndex = 0;
            this.label4.Text = "Número de diario:";
            // 
            // cancelar2
            // 
            this.cancelar2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelar2.Location = new System.Drawing.Point(416, 188);
            this.cancelar2.Name = "cancelar2";
            this.cancelar2.Size = new System.Drawing.Size(278, 63);
            this.cancelar2.TabIndex = 1;
            this.cancelar2.Text = "Cancelar";
            this.cancelar2.UseVisualStyleBackColor = true;
            this.cancelar2.Click += new System.EventHandler(this.cancelar2_Click);
            // 
            // comboBanco2
            // 
            this.comboBanco2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBanco2.FormattingEnabled = true;
            this.comboBanco2.Location = new System.Drawing.Point(416, 118);
            this.comboBanco2.Name = "comboBanco2";
            this.comboBanco2.Size = new System.Drawing.Size(278, 45);
            this.comboBanco2.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(416, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 38);
            this.label5.TabIndex = 3;
            this.label5.Text = "Banco:";
            // 
            // numeroDiario2
            // 
            this.numeroDiario2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeroDiario2.Location = new System.Drawing.Point(56, 118);
            this.numeroDiario2.Name = "numeroDiario2";
            this.numeroDiario2.Size = new System.Drawing.Size(278, 44);
            this.numeroDiario2.TabIndex = 4;
            this.numeroDiario2.TextChanged += new System.EventHandler(this.numeroDiario2_TextChanged);
            // 
            // aceptar2
            // 
            this.aceptar2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aceptar2.Location = new System.Drawing.Point(56, 188);
            this.aceptar2.Name = "aceptar2";
            this.aceptar2.Size = new System.Drawing.Size(278, 63);
            this.aceptar2.TabIndex = 5;
            this.aceptar2.Text = "Aceptar";
            this.aceptar2.UseVisualStyleBackColor = true;
            this.aceptar2.Click += new System.EventHandler(this.aceptar2_Click);
            // 
            // listView1
            // 
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.Location = new System.Drawing.Point(56, 308);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1274, 501);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(768, 116);
            this.dateTimePicker1.MaxDate = new System.DateTime(2030, 12, 31, 0, 0, 0, 0);
            this.dateTimePicker1.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(562, 44);
            this.dateTimePicker1.TabIndex = 7;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(763, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(285, 38);
            this.label6.TabIndex = 8;
            this.label6.Text = "Fecha del cheque:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configuracionToolStripMenuItem,
            this.verToolStripMenuItem,
            this.ayudaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1366, 43);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configuracionToolStripMenuItem
            // 
            this.configuracionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.servidorToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.configuracionToolStripMenuItem.Name = "configuracionToolStripMenuItem";
            this.configuracionToolStripMenuItem.Size = new System.Drawing.Size(183, 39);
            this.configuracionToolStripMenuItem.Text = "Configuracion";
            // 
            // servidorToolStripMenuItem
            // 
            this.servidorToolStripMenuItem.Name = "servidorToolStripMenuItem";
            this.servidorToolStripMenuItem.Size = new System.Drawing.Size(288, 40);
            this.servidorToolStripMenuItem.Text = "Cambiar variables";
            this.servidorToolStripMenuItem.Click += new System.EventHandler(this.servidorToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(288, 40);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // verToolStripMenuItem
            // 
            this.verToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chequesToolStripMenuItem});
            this.verToolStripMenuItem.Name = "verToolStripMenuItem";
            this.verToolStripMenuItem.Size = new System.Drawing.Size(65, 39);
            this.verToolStripMenuItem.Text = "Ver";
            // 
            // chequesToolStripMenuItem
            // 
            this.chequesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.porNúmeroDeDiarioToolStripMenuItem,
            this.porMesToolStripMenuItem});
            this.chequesToolStripMenuItem.Name = "chequesToolStripMenuItem";
            this.chequesToolStripMenuItem.Size = new System.Drawing.Size(185, 40);
            this.chequesToolStripMenuItem.Text = "Cheques";
            // 
            // porNúmeroDeDiarioToolStripMenuItem
            // 
            this.porNúmeroDeDiarioToolStripMenuItem.Name = "porNúmeroDeDiarioToolStripMenuItem";
            this.porNúmeroDeDiarioToolStripMenuItem.Size = new System.Drawing.Size(328, 40);
            this.porNúmeroDeDiarioToolStripMenuItem.Text = "Por número de diario";
            this.porNúmeroDeDiarioToolStripMenuItem.Click += new System.EventHandler(this.porNúmeroDeDiarioToolStripMenuItem_Click);
            // 
            // porMesToolStripMenuItem
            // 
            this.porMesToolStripMenuItem.Name = "porMesToolStripMenuItem";
            this.porMesToolStripMenuItem.Size = new System.Drawing.Size(328, 40);
            this.porMesToolStripMenuItem.Text = "Por mes";
            this.porMesToolStripMenuItem.Click += new System.EventHandler(this.porMesToolStripMenuItem_Click);
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acercaDeToolStripMenuItem});
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(97, 39);
            this.ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // acercaDeToolStripMenuItem
            // 
            this.acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            this.acercaDeToolStripMenuItem.Size = new System.Drawing.Size(200, 40);
            this.acercaDeToolStripMenuItem.Text = "Acerca de";
            this.acercaDeToolStripMenuItem.Click += new System.EventHandler(this.acercaDeToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(744, 170);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(586, 121);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1337, 806);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 26);
            this.label7.TabIndex = 11;
            this.label7.Text = ".";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1366, 835);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.aceptar2);
            this.Controls.Add(this.numeroDiario2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBanco2);
            this.Controls.Add(this.cancelar2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "SunPlus Cheques";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelar;
        private System.Windows.Forms.TextBox numeroDiario;
        private System.Windows.Forms.Button aceptar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBanco;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cancelar2;
        private System.Windows.Forms.ComboBox comboBanco2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox numeroDiario2;
        private System.Windows.Forms.Button aceptar2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configuracionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem servidorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acercaDeToolStripMenuItem;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chequesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem porNúmeroDeDiarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem porMesToolStripMenuItem;
        private System.Windows.Forms.Label label7;
    }
}

