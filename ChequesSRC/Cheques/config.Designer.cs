namespace Cheques
{
    partial class config
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(config));
            this.database = new System.Windows.Forms.TextBox();
            this.datasource = new System.Windows.Forms.TextBox();
            this.user = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.libro = new System.Windows.Forms.TextBox();
            this.unidad = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.revisar = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.DatabaseCheques = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.letraText = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.campo = new System.Windows.Forms.TextBox();
            this.calle = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.colonia = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ciudad = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // database
            // 
            this.database.Location = new System.Drawing.Point(68, 113);
            this.database.Name = "database";
            this.database.Size = new System.Drawing.Size(366, 31);
            this.database.TabIndex = 0;
            // 
            // datasource
            // 
            this.datasource.Location = new System.Drawing.Point(68, 215);
            this.datasource.Name = "datasource";
            this.datasource.Size = new System.Drawing.Size(366, 31);
            this.datasource.TabIndex = 1;
            // 
            // user
            // 
            this.user.Location = new System.Drawing.Point(68, 305);
            this.user.Name = "user";
            this.user.Size = new System.Drawing.Size(366, 31);
            this.user.TabIndex = 2;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(68, 392);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(366, 31);
            this.password.TabIndex = 3;
            this.password.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // libro
            // 
            this.libro.Location = new System.Drawing.Point(68, 483);
            this.libro.Name = "libro";
            this.libro.Size = new System.Drawing.Size(366, 31);
            this.libro.TabIndex = 4;
            // 
            // unidad
            // 
            this.unidad.Location = new System.Drawing.Point(68, 579);
            this.unidad.Name = "unidad";
            this.unidad.Size = new System.Drawing.Size(366, 31);
            this.unidad.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 26);
            this.label1.TabIndex = 6;
            this.label1.Text = "Database:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 26);
            this.label2.TabIndex = 7;
            this.label2.Text = "Datasource:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 26);
            this.label3.TabIndex = 8;
            this.label3.Text = "User:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 348);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 26);
            this.label4.TabIndex = 9;
            this.label4.Text = "Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 438);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 26);
            this.label5.TabIndex = 10;
            this.label5.Text = "Libro:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 531);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(210, 26);
            this.label6.TabIndex = 11;
            this.label6.Text = "Unidad de negocios:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(78, 798);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(142, 43);
            this.button1.TabIndex = 15;
            this.button1.Text = "Aceptar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(285, 798);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(149, 43);
            this.button2.TabIndex = 16;
            this.button2.Text = "Cerrar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // revisar
            // 
            this.revisar.Location = new System.Drawing.Point(68, 892);
            this.revisar.Name = "revisar";
            this.revisar.Size = new System.Drawing.Size(366, 48);
            this.revisar.TabIndex = 17;
            this.revisar.Text = "Revisar Conexión";
            this.revisar.UseVisualStyleBackColor = true;
            this.revisar.Click += new System.EventHandler(this.revisar_Click_1);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(63, 630);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(199, 26);
            this.label7.TabIndex = 18;
            this.label7.Text = "Database cheques:";
            // 
            // DatabaseCheques
            // 
            this.DatabaseCheques.Location = new System.Drawing.Point(68, 680);
            this.DatabaseCheques.Name = "DatabaseCheques";
            this.DatabaseCheques.Size = new System.Drawing.Size(366, 31);
            this.DatabaseCheques.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(73, 734);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 26);
            this.label8.TabIndex = 20;
            this.label8.Text = "Letra:";
            // 
            // letraText
            // 
            this.letraText.Location = new System.Drawing.Point(173, 734);
            this.letraText.Name = "letraText";
            this.letraText.Size = new System.Drawing.Size(261, 31);
            this.letraText.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(499, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 26);
            this.label9.TabIndex = 22;
            this.label9.Text = "Campo:";
            // 
            // campo
            // 
            this.campo.Location = new System.Drawing.Point(504, 113);
            this.campo.Name = "campo";
            this.campo.Size = new System.Drawing.Size(366, 31);
            this.campo.TabIndex = 23;
            // 
            // calle
            // 
            this.calle.Location = new System.Drawing.Point(504, 236);
            this.calle.Name = "calle";
            this.calle.Size = new System.Drawing.Size(366, 31);
            this.calle.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(499, 175);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(165, 26);
            this.label10.TabIndex = 24;
            this.label10.Text = "Calle y numero:";
            // 
            // colonia
            // 
            this.colonia.Location = new System.Drawing.Point(504, 371);
            this.colonia.Name = "colonia";
            this.colonia.Size = new System.Drawing.Size(366, 31);
            this.colonia.TabIndex = 27;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(499, 310);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 26);
            this.label11.TabIndex = 26;
            this.label11.Text = "Colonia:";
            // 
            // ciudad
            // 
            this.ciudad.Location = new System.Drawing.Point(507, 513);
            this.ciudad.Name = "ciudad";
            this.ciudad.Size = new System.Drawing.Size(366, 31);
            this.ciudad.TabIndex = 29;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(502, 452);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(175, 26);
            this.label12.TabIndex = 28;
            this.label12.Text = "Ciudad y estado:";
            // 
            // config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1046, 1030);
            this.Controls.Add(this.ciudad);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.colonia);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.calle);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.campo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.letraText);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.DatabaseCheques);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.revisar);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.unidad);
            this.Controls.Add(this.libro);
            this.Controls.Add(this.password);
            this.Controls.Add(this.user);
            this.Controls.Add(this.datasource);
            this.Controls.Add(this.database);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "config";
            this.Text = "Configuración";
            this.Load += new System.EventHandler(this.config_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox database;
        private System.Windows.Forms.TextBox datasource;
        private System.Windows.Forms.TextBox user;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox libro;
        private System.Windows.Forms.TextBox unidad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button revisar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox DatabaseCheques;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox letraText;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox campo;
        private System.Windows.Forms.TextBox calle;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox colonia;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox ciudad;
        private System.Windows.Forms.Label label12;
    }
}