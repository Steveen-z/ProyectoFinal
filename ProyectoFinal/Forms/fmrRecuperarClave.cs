using ProyectoFinal.Clases;
using ProyectoFinal.Repositorios;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrRecuperarClave : Form
    {
        private readonly DocentesRepository _docentesRepository;
        private readonly string _codigoAcceso;
        private readonly string _connectionString;

        public fmrRecuperarClave() : this(null, string.Empty)
        {
        }

        public fmrRecuperarClave(string connectionString, string codigoAcceso)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            _connectionString = connectionString ?? string.Empty;
            _codigoAcceso = codigoAcceso;

            if (!string.IsNullOrEmpty(_connectionString))
            {
                _docentesRepository = new DocentesRepository(_connectionString);
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (_docentesRepository == null)
            {
                MessageBox.Show("Error de configuración: No se pudo establecer la conexión a la base de datos.", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string claveNueva = txtClaveNueva.Text.Trim();
            string confirmarClave = txtConfirmarClave.Text.Trim();

            if (string.IsNullOrWhiteSpace(claveNueva) || string.IsNullOrWhiteSpace(confirmarClave))
            {
                MessageBox.Show("Debe ingresar la nueva contraseña y su confirmación.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (claveNueva != confirmarClave)
            {
                MessageBox.Show("La nueva contraseña y la confirmación no coinciden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmarClave.Focus();
                return;
            }
            if (claveNueva.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_docentesRepository.ActualizarClavePorCodigoAcceso(_codigoAcceso, claveNueva))
                {
                    MessageBox.Show("Contraseña actualizada exitosamente. Ahora puede iniciar sesión.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la contraseña. Verifique si el usuario existe.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado al actualizar la clave: {ex.Message}", "Error General", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fmrRecuperarClave_Load(object sender, EventArgs e)
        {
        }
    }
}