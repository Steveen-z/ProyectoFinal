using ProyectoFinal.Repositorios;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrEdicionEspecializacion : Form
    {
        private readonly CatalogosRepository _catalogosRepository;
        private readonly int _idEspecializacion;
        private readonly string _nombreOriginal;

        // Delegado para notificar al formulario padre que se recargue.
        public event Action DatosActualizados;

        public fmrEdicionEspecializacion(int id, string nombre)
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;
            _catalogosRepository = new CatalogosRepository(connectionString);

            _idEspecializacion = id;
            _nombreOriginal = nombre;

            this.Text = $"Editar: {nombre}";
            txtNombreEspecializacion.Text = nombre;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nuevoNombre = txtNombreEspecializacion.Text.Trim();

            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show("El nombre de la especialización no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (nuevoNombre == _nombreOriginal)
            {
                MessageBox.Show("No se detectaron cambios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (_catalogosRepository.ModificarEspecializacion(_idEspecializacion, nuevoNombre))
                {
                    MessageBox.Show("Especialización modificada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DatosActualizados?.Invoke(); // Notificar al padre para que recargue la lista
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al modificar la especialización. Intente de nuevo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            var confirmacion = MessageBox.Show(
                $"¿Está seguro de que desea ELIMINAR la especialización '{_nombreOriginal}'?\nEsta acción no se puede deshacer.",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    // No se implementara por falta de tiempo ya que se deberian anadir mas filtros
                    if (_catalogosRepository.EliminarEspecializacion(_idEspecializacion))
                    {
                        MessageBox.Show("Especialización eliminada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DatosActualizados?.Invoke(); // Notificar al padre para que recargue la lista
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar la especialización. Puede que existan registros asociados (estudiantes/asignaturas).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al eliminar: {ex.Message}", "Error de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
