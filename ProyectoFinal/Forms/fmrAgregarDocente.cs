using ProyectoFinal.Clases;
using ProyectoFinal.Repositorios;
using System;
using System.Data.SqlClient; 
using System.Windows.Forms;
namespace ProyectoFinal.Forms
{
    public partial class fmrAgregarDocente : Form
    {
        private readonly DocentesRepository _docentesRepository;

        public fmrAgregarDocente(string connectionString)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            _docentesRepository = new DocentesRepository(connectionString);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtClave.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string codigoAccesoGenerado = _docentesRepository.GenerarCodigoDocenteUnico();

            Docentes nuevoDocente = new Docentes
            {
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                DUI = txtDUI.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),

                CodigoAcceso = codigoAccesoGenerado
            };

            string clave = txtClave.Text;
            string rol = "Docente";

            try
            {
                if (_docentesRepository.AgregarDocente(nuevoDocente, clave, rol))
                {
                    MessageBox.Show($"Docente registrado exitosamente.\nCódigo de Acceso: {codigoAccesoGenerado}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hubo un error al intentar guardar el docente.", "Error de Guardado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    MessageBox.Show("El Correo Electrónico ya existe en el sistema. Por favor, ingrese un dato único.", "Dato Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Error de base de datos: " + ex.Message, "Error de SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error General", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void fmrAgregarDocente_Load(object sender, EventArgs e)
        {
        }
    }
}