using System;
using System.Windows.Forms;
using ProyectoFinal.Forms; 

namespace ProyectoFinal.Forms
{
    public partial class fmrMenuGestionUsuarios : Form
    {
        public fmrMenuGestionUsuarios()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Hide();

            fmrMenuAdmin menuAdmin = new fmrMenuAdmin();
            menuAdmin.Show();

        }

        private void btnGestionDocente_Click(object sender, EventArgs e)
        {
            fmrGestionDocentes gestionDocentes = new fmrGestionDocentes();
            gestionDocentes.ShowDialog(); 
        }

        private void btnGestionEstudiantes_Click(object sender, EventArgs e)
        {
            fmrGestionEstudiantes gestionEstudiantes = new fmrGestionEstudiantes();
            gestionEstudiantes.ShowDialog();
        }

        private void fmrMenuGestionUsuarios_Load(object sender, EventArgs e)
        {

        }
    }
}