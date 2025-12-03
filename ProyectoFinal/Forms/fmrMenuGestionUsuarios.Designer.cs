namespace ProyectoFinal.Forms
{
    partial class fmrMenuGestionUsuarios
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
            this.btnRegresar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGestionEstudiantes = new System.Windows.Forms.Button();
            this.btnGestionDocente = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::ProyectoFinal.Properties.Resources.FondoLogin2;
            this.panel1.Controls.Add(this.btnRegresar);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnGestionEstudiantes);
            this.panel1.Controls.Add(this.btnGestionDocente);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 1;
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
            this.label1.Size = new System.Drawing.Size(307, 38);
            this.label1.TabIndex = 31;
            this.label1.Text = "Gestión de usuarios";
            // 
            // btnGestionEstudiantes
            // 
            this.btnGestionEstudiantes.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGestionEstudiantes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGestionEstudiantes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGestionEstudiantes.ForeColor = System.Drawing.Color.Transparent;
            this.btnGestionEstudiantes.Location = new System.Drawing.Point(242, 209);
            this.btnGestionEstudiantes.Name = "btnGestionEstudiantes";
            this.btnGestionEstudiantes.Size = new System.Drawing.Size(340, 46);
            this.btnGestionEstudiantes.TabIndex = 30;
            this.btnGestionEstudiantes.Text = "Gestion Estudiantes";
            this.btnGestionEstudiantes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGestionEstudiantes.UseVisualStyleBackColor = false;
            this.btnGestionEstudiantes.Click += new System.EventHandler(this.btnGestionEstudiantes_Click);
            // 
            // btnGestionDocente
            // 
            this.btnGestionDocente.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGestionDocente.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGestionDocente.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGestionDocente.ForeColor = System.Drawing.Color.Transparent;
            this.btnGestionDocente.Location = new System.Drawing.Point(242, 146);
            this.btnGestionDocente.Name = "btnGestionDocente";
            this.btnGestionDocente.Size = new System.Drawing.Size(340, 46);
            this.btnGestionDocente.TabIndex = 29;
            this.btnGestionDocente.Text = "Gestion Docente";
            this.btnGestionDocente.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGestionDocente.UseVisualStyleBackColor = false;
            this.btnGestionDocente.Click += new System.EventHandler(this.btnGestionDocente_Click);
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
            // fmrMenuGestionUsuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Name = "fmrMenuGestionUsuarios";
            this.Text = "fmrMenuGestionUsuarios";
            this.Load += new System.EventHandler(this.fmrMenuGestionUsuarios_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRegresar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGestionEstudiantes;
        private System.Windows.Forms.Button btnGestionDocente;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}