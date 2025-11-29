namespace ProyectoFinal.Forms
{
    partial class fmrRegistroAsignaturas
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
            this.cmbNivel = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbEspecializacion = new System.Windows.Forms.ComboBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGuardarMateria = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNombreMateria = new System.Windows.Forms.TextBox();
            this.lblNombreEspecializacion = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::ProyectoFinal.Properties.Resources.FondoLogin2;
            this.panel1.Controls.Add(this.cmbNivel);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmbEspecializacion);
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnGuardarMateria);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtNombreMateria);
            this.panel1.Controls.Add(this.lblNombreEspecializacion);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(851, 333);
            this.panel1.TabIndex = 2;
            // 
            // cmbNivel
            // 
            this.cmbNivel.FormattingEnabled = true;
            this.cmbNivel.Location = new System.Drawing.Point(107, 168);
            this.cmbNivel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbNivel.Name = "cmbNivel";
            this.cmbNivel.Size = new System.Drawing.Size(243, 24);
            this.cmbNivel.TabIndex = 46;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Calisto MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(410, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 23);
            this.label4.TabIndex = 45;
            this.label4.Text = "Especializacion:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Calisto MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(34, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 23);
            this.label3.TabIndex = 44;
            this.label3.Text = "Nivel:";
            // 
            // cmbEspecializacion
            // 
            this.cmbEspecializacion.FormattingEnabled = true;
            this.cmbEspecializacion.Location = new System.Drawing.Point(577, 166);
            this.cmbEspecializacion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbEspecializacion.Name = "cmbEspecializacion";
            this.cmbEspecializacion.Size = new System.Drawing.Size(243, 24);
            this.cmbEspecializacion.TabIndex = 43;
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Firebrick;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.Transparent;
            this.btnCancelar.Location = new System.Drawing.Point(258, 287);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(153, 34);
            this.btnCancelar.TabIndex = 35;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnGuardarMateria
            // 
            this.btnGuardarMateria.BackColor = System.Drawing.Color.SlateGray;
            this.btnGuardarMateria.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardarMateria.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardarMateria.ForeColor = System.Drawing.Color.Transparent;
            this.btnGuardarMateria.Location = new System.Drawing.Point(435, 287);
            this.btnGuardarMateria.Name = "btnGuardarMateria";
            this.btnGuardarMateria.Size = new System.Drawing.Size(153, 34);
            this.btnGuardarMateria.TabIndex = 34;
            this.btnGuardarMateria.Text = "Guardar";
            this.btnGuardarMateria.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardarMateria.UseVisualStyleBackColor = false;
            this.btnGuardarMateria.Click += new System.EventHandler(this.btnGuardarMateria_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calisto MT", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(287, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 38);
            this.label1.TabIndex = 33;
            this.label1.Text = "Agregar materias";
            // 
            // txtNombreMateria
            // 
            this.txtNombreMateria.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNombreMateria.Font = new System.Drawing.Font("Microsoft PhagsPa", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombreMateria.Location = new System.Drawing.Point(361, 70);
            this.txtNombreMateria.Name = "txtNombreMateria";
            this.txtNombreMateria.Size = new System.Drawing.Size(370, 26);
            this.txtNombreMateria.TabIndex = 18;
            // 
            // lblNombreEspecializacion
            // 
            this.lblNombreEspecializacion.AutoSize = true;
            this.lblNombreEspecializacion.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblNombreEspecializacion.Font = new System.Drawing.Font("Microsoft PhagsPa", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreEspecializacion.Location = new System.Drawing.Point(106, 70);
            this.lblNombreEspecializacion.Name = "lblNombreEspecializacion";
            this.lblNombreEspecializacion.Size = new System.Drawing.Size(255, 26);
            this.lblNombreEspecializacion.TabIndex = 17;
            this.lblNombreEspecializacion.Text = "Nombre de la materia:     ";
            // 
            // fmrRegistroAsignaturas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 333);
            this.Controls.Add(this.panel1);
            this.Name = "fmrRegistroAsignaturas";
            this.Text = "fmrAltaAsignatura";
            this.Load += new System.EventHandler(this.fmrAltaAsignatura_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmbNivel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbEspecializacion;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGuardarMateria;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNombreMateria;
        private System.Windows.Forms.Label lblNombreEspecializacion;
    }
}