namespace Cheques
{
    partial class confirm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(confirm));
            this.label1 = new System.Windows.Forms.Label();
            this.nombreT = new System.Windows.Forms.TextBox();
            this.fechaT = new System.Windows.Forms.TextBox();
            this.totalT = new System.Windows.Forms.TextBox();
            this.numeroALetrasT = new System.Windows.Forms.TextBox();
            this.si = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.preconceptoT = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 23F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(125, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(806, 71);
            this.label1.TabIndex = 0;
            this.label1.Text = "¿Son estos datos correctos?";
            // 
            // nombreT
            // 
            this.nombreT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nombreT.Location = new System.Drawing.Point(85, 211);
            this.nombreT.Name = "nombreT";
            this.nombreT.Size = new System.Drawing.Size(928, 44);
            this.nombreT.TabIndex = 1;
            // 
            // fechaT
            // 
            this.fechaT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fechaT.Location = new System.Drawing.Point(85, 314);
            this.fechaT.Name = "fechaT";
            this.fechaT.Size = new System.Drawing.Size(928, 44);
            this.fechaT.TabIndex = 2;
            // 
            // totalT
            // 
            this.totalT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalT.Location = new System.Drawing.Point(85, 402);
            this.totalT.Name = "totalT";
            this.totalT.Size = new System.Drawing.Size(928, 44);
            this.totalT.TabIndex = 3;
            // 
            // numeroALetrasT
            // 
            this.numeroALetrasT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeroALetrasT.Location = new System.Drawing.Point(85, 498);
            this.numeroALetrasT.Name = "numeroALetrasT";
            this.numeroALetrasT.Size = new System.Drawing.Size(928, 44);
            this.numeroALetrasT.TabIndex = 4;
            // 
            // si
            // 
            this.si.Location = new System.Drawing.Point(85, 740);
            this.si.Name = "si";
            this.si.Size = new System.Drawing.Size(330, 58);
            this.si.TabIndex = 5;
            this.si.Text = "Si son correctos";
            this.si.UseVisualStyleBackColor = true;
            this.si.Click += new System.EventHandler(this.si_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(694, 740);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(319, 58);
            this.button1.TabIndex = 6;
            this.button1.Text = "No";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // preconceptoT
            // 
            this.preconceptoT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.preconceptoT.Location = new System.Drawing.Point(85, 602);
            this.preconceptoT.Name = "preconceptoT";
            this.preconceptoT.Size = new System.Drawing.Size(928, 44);
            this.preconceptoT.TabIndex = 7;
            // 
            // confirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1132, 859);
            this.Controls.Add(this.preconceptoT);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.si);
            this.Controls.Add(this.numeroALetrasT);
            this.Controls.Add(this.totalT);
            this.Controls.Add(this.fechaT);
            this.Controls.Add(this.nombreT);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "confirm";
            this.Text = "Confirmación";
            this.Load += new System.EventHandler(this.confirm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nombreT;
        private System.Windows.Forms.TextBox fechaT;
        private System.Windows.Forms.TextBox totalT;
        private System.Windows.Forms.TextBox numeroALetrasT;
        private System.Windows.Forms.Button si;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox preconceptoT;
    }
}