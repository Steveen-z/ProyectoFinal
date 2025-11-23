using ProyectoFinal.Repositorios;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrAgregarEspecializacion : Form
    {
        private readonly CatalogosRepository _catalogosRepository;

        // Evento para notificar al formulario padre que recargue la lista
        public event Action DatosActualizados;

        public fmrAgregarEspecializacion()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;
            _catalogosRepository = new CatalogosRepository(connectionString);

            this.Text = "Agregar Nueva Especialización";
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nuevoNombre = txtNombreEspecializacion.Text.Trim();

            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show("Ingrese el nombre de la especialización.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                bool resultado = _catalogosRepository.AgregarEspecializacion(nuevoNombre);

                if (resultado)
                {
                    MessageBox.Show("Especializacion agregada exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DatosActualizados?.Invoke(); // Notificar al padre para recargar
                    this.Close();
                }
                else
                {
                    // Esto sucede si ya existe una especializacion con ese nombre
                    MessageBox.Show("La especializacion ya existe o hubo un error al guardar", "Error de Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}