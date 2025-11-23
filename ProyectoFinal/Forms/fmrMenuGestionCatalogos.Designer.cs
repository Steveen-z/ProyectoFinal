namespace ProyectoFinal.Forms
{
    partial class fmrMenuGestionCatalogos
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRegresar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGestionAsignaturas = new System.Windows.Forms.Button();
            this.btnGestionEspecializaciones = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::ProyectoFinal.Properties.Resources.FondoLogin2;
            this.panel2.Controls.Add(this.btnRegresar);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnGestionAsignaturas);
            this.panel2.Controls.Add(this.btnGestionEspecializaciones);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 450);
            this.panel2.TabIndex = 2;
            // 
            // btnRegresar
            // 
            this.btnRegresar.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRegresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegresar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegresar.ForeColor = System.Drawing.Color.Transparent;
            this.btnRegresar.Location = new System.Drawing.Point(242, 275);
            this.btnRegresar.Name = "btnRegresar";
            this.btnRegresar.Size = new System.Drawing.Size(340, 46);
            this.btnRegresar.TabIndex = 33;
            this.btnRegresar.Text = "Regresar";
            this.btnRegresar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRegresar.UseVisualStyleBackColor = false;
            this.btnRegresar.Click += new System.EventHandler(this.btnRegresar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calisto MT", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(259, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(306, 38);
            this.label1.TabIndex = 31;
            this.label1.Text = "Gestión de catalogo";
            // 
            // btnGestionAsignaturas
            // 
            this.btnGestionAsignaturas.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGestionAsignaturas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGestionAsignaturas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGestionAsignaturas.ForeColor = System.Drawing.Color.Transparent;
            this.btnGestionAsignaturas.Location = new System.Drawing.Point(242, 209);
            this.btnGestionAsignaturas.Name = "btnGestionAsignaturas";
            this.btnGestionAsignaturas.Size = new System.Drawing.Size(340, 46);
            this.btnGestionAsignaturas.TabIndex = 30;
            this.btnGestionAsignaturas.Text = "Gestion de asignaturas";
            this.btnGestionAsignaturas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGestionAsignaturas.UseVisualStyleBackColor = false;
            this.btnGestionAsignaturas.Click += new System.EventHandler(this.btnGestionAsignaturas_Click);
            // 
            // btnGestionEspecializaciones
            // 
            this.btnGestionEspecializaciones.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGestionEspecializaciones.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGestionEspecializaciones.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGestionEspecializaciones.ForeColor = System.Drawing.Color.Transparent;
            this.btnGestionEspecializaciones.Location = new System.Drawing.Point(242, 146);
            this.btnGestionEspecializaciones.Name = "btnGestionEspecializaciones";
            this.btnGestionEspecializaciones.Size = new System.Drawing.Size(340, 46);
            this.btnGestionEspecializaciones.TabIndex = 29;
            this.btnGestionEspecializaciones.Text = "Gestion de especializaciones";
            this.btnGestionEspecializaciones.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGestionEspecializaciones.UseVisualStyleBackColor = false;
            this.btnGestionEspecializaciones.Click += new System.EventHandler(this.btnGestionEspecializaciones_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::ProyectoFinal.Properties.Resources.Blue_and_White_Illustrative_Education_Badge_Logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(144, 130);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
            // 
            // fmrMenuGestionCatalogos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "fmrMenuGestionCatalogos";
            this.Text = "fmrMenuGestionCatalogos";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnRegresar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGestionAsignaturas;
        private System.Windows.Forms.Button btnGestionEspecializaciones;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}