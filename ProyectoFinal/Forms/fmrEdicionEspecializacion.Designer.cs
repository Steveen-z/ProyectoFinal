namespace ProyectoFinal.Forms
{
    partial class fmrEdicionEspecializacion
    {

        private System.ComponentModel.IContainer components = null;


       
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGuardarEspecializacion = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNombreEspecializacion = new System.Windows.Forms.TextBox();
            this.lblNombreEspecializacion = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::ProyectoFinal.Properties.Resources.FondoLogin2;
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Controls.Add(this.btnGuardarEspecializacion);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtNombreEspecializacion);
            this.panel1.Controls.Add(this.lblNombreEspecializacion);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(851, 170);
            this.panel1.TabIndex = 0;
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Firebrick;
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.Transparent;
            this.btnCancelar.Location = new System.Drawing.Point(217, 125);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(153, 34);
            this.btnCancelar.TabIndex = 35;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnGuardarEspecializacion
            // 
            this.btnGuardarEspecializacion.BackColor = System.Drawing.Color.SlateGray;
            this.btnGuardarEspecializacion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardarEspecializacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardarEspecializacion.ForeColor = System.Drawing.Color.Transparent;
            this.btnGuardarEspecializacion.Location = new System.Drawing.Point(394, 125);
            this.btnGuardarEspecializacion.Name = "btnGuardarEspecializacion";
            this.btnGuardarEspecializacion.Size = new System.Drawing.Size(153, 34);
            this.btnGuardarEspecializacion.TabIndex = 34;
            this.btnGuardarEspecializacion.Text = "Guardar";
            this.btnGuardarEspecializacion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardarEspecializacion.UseVisualStyleBackColor = false;
            this.btnGuardarEspecializacion.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Calisto MT", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(177, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(429, 38);
            this.label1.TabIndex = 33;
            this.label1.Text = "Edición de especializaciones";
            // 
            // txtNombreEspecializacion
            // 
            this.txtNombreEspecializacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNombreEspecializacion.Font = new System.Drawing.Font("Microsoft PhagsPa", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombreEspecializacion.Location = new System.Drawing.Point(394, 70);
            this.txtNombreEspecializacion.Name = "txtNombreEspecializacion";
            this.txtNombreEspecializacion.Size = new System.Drawing.Size(370, 26);
            this.txtNombreEspecializacion.TabIndex = 18;
            // 
            // lblNombreEspecializacion
            // 
            this.lblNombreEspecializacion.AutoSize = true;
            this.lblNombreEspecializacion.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblNombreEspecializacion.Font = new System.Drawing.Font("Microsoft PhagsPa", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreEspecializacion.Location = new System.Drawing.Point(75, 70);
            this.lblNombreEspecializacion.Name = "lblNombreEspecializacion";
            this.lblNombreEspecializacion.Size = new System.Drawing.Size(325, 26);
            this.lblNombreEspecializacion.TabIndex = 17;
            this.lblNombreEspecializacion.Text = "Nombre de la especializacion:     ";
            // 
            // fmrEdicionEspecializacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 170);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(869, 217);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(869, 217);
            this.Name = "fmrEdicionEspecializacion";
            this.Text = "Edicion Especializacion";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtNombreEspecializacion;
        private System.Windows.Forms.Label lblNombreEspecializacion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGuardarEspecializacion;
        private System.Windows.Forms.Button btnCancelar;
    }
}