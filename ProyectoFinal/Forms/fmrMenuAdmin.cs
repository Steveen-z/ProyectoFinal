using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrMenuAdmin : Form
    {
        public fmrMenuAdmin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnGestionUsuarios_Click(object sender, EventArgs e)
        {
            //  Abre el menú secundario de Gestión de Usuarios
            fmrMenuGestionUsuarios menuUsuarios = new fmrMenuGestionUsuarios();
            menuUsuarios.Show();
            this.Hide();
        }

        private void btnGestionCatalogos_Click(object sender, EventArgs e)
        {
            //Abre el menú secundario de Gestión de Catálogos
            fmrMenuGestionCatalogos menuCatalogos = new fmrMenuGestionCatalogos();
            menuCatalogos.Show();
            this.Hide();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // La funcion para volver al Login.
            fmrLogin loginForm = new fmrLogin();
            loginForm.Show();
            this.Close();
        }
    }
}