using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrMenuGestionCatalogos : Form
    {
        public fmrMenuGestionCatalogos()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnGestionEspecializaciones_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Abriendo formulario: Gestión de Especializaciones (Carreras Técnicas)", "Gestión de Catálogos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGestionAsignaturas_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Abriendo formulario: Gestión de Asignaturas (Materias)", "Gestión de Catálogos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            fmrMenuAdmin menuAdmin = new fmrMenuAdmin();
            menuAdmin.Show();
            this.Close();
        }
    }
}