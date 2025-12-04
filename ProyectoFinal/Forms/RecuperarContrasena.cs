using ProyectoFinal.Clases;
using ProyectoFinal.Repositorios;
using System;
using System.Configuration;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProyectoFinal.Forms
{
    public partial class RecuperarContrasena : Form
    {
        private readonly EstudiantesGestionRepository _estudiantesRepository;
        private readonly int _idUsuario;

        public RecuperarContrasena(int idUsuario)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            _idUsuario = idUsuario;

            string connectionString = string.Empty;

            var connectionSetting = ConfigurationManager.ConnectionStrings["ProyectoDB"];

            if (connectionSetting != null)
            {
                connectionString = connectionSetting.ConnectionString;
            }

            if (!string.IsNullOrEmpty(connectionString))
            {
                _estudiantesRepository = new EstudiantesGestionRepository(connectionString);
            }
            else
            {
                MessageBox.Show("Error de configuración: La cadena 'ProyectoFinalDB' no se encontró o está vacía.", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public RecuperarContrasena() : this(-1)
        {
        }

        private void RecuperarContrasena_Load(object sender, EventArgs e)
        {
            txtContraNueva.UseSystemPasswordChar = true;
            txtConfirmarContra.UseSystemPasswordChar = true;

            if (_idUsuario <= 0)
            {
                MessageBox.Show("Error de flujo: No se pudo identificar al usuario para cambiar la clave.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (_estudiantesRepository == null || _idUsuario <= 0)
            {
                MessageBox.Show("Error crítico: El usuario no está identificado o la conexión falló.", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string claveNueva = txtContraNueva.Text.Trim();
            string confirmarClave = txtConfirmarContra.Text.Trim();

            if (string.IsNullOrWhiteSpace(claveNueva) || string.IsNullOrWhiteSpace(confirmarClave))
            {
                MessageBox.Show("Debe ingresar la nueva contraseña y su confirmación.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (claveNueva != confirmarClave)
            {
                MessageBox.Show("La nueva contraseña y la confirmación no coinciden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmarContra.Focus();
                return;
            }
            if (claveNueva.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string nuevoPasswordHash = Encriptador.EncriptarClave(claveNueva);

                if (_estudiantesRepository.ActualizarClave(_idUsuario, nuevoPasswordHash))
                {
                    MessageBox.Show("Contraseña actualizada exitosamente. Ahora puede iniciar sesión.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la contraseña. Ocurrió un error en la base de datos.", "Error de Actualización", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}