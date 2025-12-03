using ProyectoFinal.Clases;
using ProyectoFinal.Repositorios;
using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProyectoFinal.Forms
{
    public partial class fmrEditarDocente : Form
    {
        private readonly DocentesRepository _docentesRepository;
        private readonly int _idDocente;
        private readonly string _connectionString; 

        private Docentes _docenteActual;

        public fmrEditarDocente(string connectionString, int idDocente)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _connectionString = connectionString; 
            _idDocente = idDocente;
            _docentesRepository = new DocentesRepository(connectionString);

            CargarDatosDocente();
        }

        private void CargarDatosDocente()
        {
            try
            {
                _docenteActual = _docentesRepository.ObtenerDocentePorId(_idDocente);

                if (_docenteActual != null)
                {
                    txtNombre.Text = _docenteActual.Nombre;
                    txtApellido.Text = _docenteActual.Apellido;
                    txtDUI.Text = _docenteActual.DUI;

                    txtNombre.ReadOnly = true;
                    txtApellido.ReadOnly = true;
                    txtDUI.ReadOnly = true;

                    txtEmail.Text = _docenteActual.Email;
                    txtTelefono.Text = _docenteActual.Telefono;

                }
                else
                {
                    MessageBox.Show("No se encontraron datos para el docente seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del docente: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            string nuevoEmail = txtEmail.Text.Trim();
            string nuevoTelefono = txtTelefono.Text.Trim();

            string claveActual = txtClave.Text;
            string claveNueva = txtClaveNueva.Text;
            string confirmarClave = txtConfirmarClave.Text;


            bool intentarCambioClave = !string.IsNullOrEmpty(claveActual) || !string.IsNullOrEmpty(claveNueva) || !string.IsNullOrEmpty(confirmarClave);

            if (string.IsNullOrWhiteSpace(nuevoEmail))
            {
                MessageBox.Show("El campo Email es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool datosActualizados = false;
                if (_docenteActual.Email != nuevoEmail || _docenteActual.Telefono != nuevoTelefono)
                {
                    if (_docentesRepository.ActualizarDocente(_idDocente, nuevoEmail, nuevoTelefono))
                    {
                        _docenteActual.Email = nuevoEmail;
                        _docenteActual.Telefono = nuevoTelefono;
                        datosActualizados = true;
                    }
                }

                if (intentarCambioClave)
                {
                    if (string.IsNullOrEmpty(claveActual))
                    {
                        MessageBox.Show("Debe ingresar la Clave Actual para poder cambiar la contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClave.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(claveNueva) || string.IsNullOrEmpty(confirmarClave))
                    {
                        MessageBox.Show("Debe ingresar y confirmar la Clave Nueva.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClaveNueva.Focus();
                        return;
                    }
                    if (claveNueva != confirmarClave)
                    {
                        MessageBox.Show("La Clave Nueva y la Confirmación no coinciden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtConfirmarClave.Focus();
                        return;
                    }

                    if (!_docentesRepository.VerificarClave(_docenteActual.IdDocente, claveActual))
                    {
                        MessageBox.Show("La Clave Actual ingresada es incorrecta. No se pudo cambiar la contraseña.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtClave.Focus();
                        return;
                    }

                    if (_docentesRepository.ActualizarClaveDocente(_idDocente, claveNueva))
                    {
                        datosActualizados = true;
                    }

                    txtClave.Clear();
                    txtClaveNueva.Clear();
                    txtConfirmarClave.Clear();
                }


                if (datosActualizados)
                {
                    MessageBox.Show("Los datos del docente fueron actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (!intentarCambioClave)
                {
                    MessageBox.Show("No se detectaron cambios en la información.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    MessageBox.Show("El Email ingresado ya existe en el sistema. Debe ser único.", "Dato Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Error de base de datos: {ex.Message}", "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al guardar los datos: {ex.Message}", "Error de Guardado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnContrasenaOlvidada_Click(object sender, EventArgs e)
        {
            fmrRecuperarClave formRecuperar = new fmrRecuperarClave(_connectionString, _docenteActual.CodigoAcceso);
            formRecuperar.ShowDialog();
        }

        private void fmrEditarDocente_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}