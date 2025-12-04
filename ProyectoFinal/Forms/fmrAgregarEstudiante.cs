using ProyectoFinal.Clases;
using ProyectoFinal.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrAgregarEstudiante : Form
    {
        private readonly EstudiantesGestionRepository _estudiantesGestionRepository;
        private readonly string _connectionString;

        public fmrAgregarEstudiante(string connectionString)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _connectionString = connectionString;
            _estudiantesGestionRepository = new EstudiantesGestionRepository(connectionString);

            btnGuardar.Click += btnGuardar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            btnCancelar.Click += btnCancelar_Click;

            CargarCombobox();

            cmbNivel.SelectedIndexChanged += cmbNivel_SelectedIndexChanged;
        }


        private void cmbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNivel.SelectedItem != null)
            {
                string nivelSeleccionado = cmbNivel.Text;

                if (nivelSeleccionado.Contains("General") &&
                    (nivelSeleccionado.Contains("1°") || nivelSeleccionado.Contains("2°")))
                {
                    cmbEspecializacion.Enabled = false;

                    if (cmbEspecializacion.Items.Count > 0)
                    {
                        cmbEspecializacion.SelectedValue = -1;
                    }
                }
                else
                {
                    cmbEspecializacion.Enabled = true;
                    if ((int)cmbEspecializacion.SelectedValue == -1)
                    {
                        var source = cmbEspecializacion.DataSource as BindingSource;
                        var firstValidItem = source.Cast<KeyValuePair<int, string>>().FirstOrDefault(kvp => kvp.Key > 0);
                        if (firstValidItem.Key > 0)
                        {
                            cmbEspecializacion.SelectedValue = firstValidItem.Key;
                        }
                    }
                }
            }
        }

        private void CargarCombobox()
        {
            try
            {
                var niveles = _estudiantesGestionRepository.ObtenerNiveles();
                cmbNivel.DataSource = new BindingSource(niveles, null);
                cmbNivel.DisplayMember = "Value";
                cmbNivel.ValueMember = "Key";
                cmbNivel.SelectedIndex = -1;

                var especializaciones = _estudiantesGestionRepository.ObtenerEspecializaciones();

                var especializacionesConCentinela = new Dictionary<int, string> { { -1, "(Ninguna/Opcional)" } };

                foreach (var kvp in especializaciones)
                {
                    especializacionesConCentinela.Add(kvp.Key, kvp.Value);
                }

                cmbEspecializacion.DataSource = new BindingSource(especializacionesConCentinela, null);
                cmbEspecializacion.DisplayMember = "Value";
                cmbEspecializacion.ValueMember = "Key";

                cmbEspecializacion.SelectedValue = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos de niveles y especializaciones: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtClaveTemporal.Text) ||
                cmbNivel.SelectedValue == null || (cmbNivel.SelectedValue is int i && i <= 0))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos (Nombre, Apellido, Clave Temporal y Nivel).", "Datos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idNivel = (int)cmbNivel.SelectedValue;
                int idEspecializacionSeleccionada = (int)cmbEspecializacion.SelectedValue;

                int? idEspecializacion = null;

                if (idEspecializacionSeleccionada > 0)
                {
                    idEspecializacion = idEspecializacionSeleccionada;
                }

                string nivelSeleccionado = cmbNivel.Text;
                bool isSpecializationRequired = !(nivelSeleccionado.Contains("General") &&
                                                  (nivelSeleccionado.Contains("1°") || nivelSeleccionado.Contains("2°")));

                if (isSpecializationRequired && !idEspecializacion.HasValue)
                {
                    MessageBox.Show("El nivel seleccionado requiere una especialización específica. Por favor, elija una.", "Validación de Especialización", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Estudiantes nuevoEstudiante = new Estudiantes
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    IdNivel = idNivel,

                    IdEspecializacion = idEspecializacion
                };

                string claveTemporal = txtClaveTemporal.Text.Trim();

                string codigoAcceso = _estudiantesGestionRepository.AgregarEstudianteCompleto(nuevoEstudiante, claveTemporal);

                if (codigoAcceso != null)
                {
                    MessageBox.Show($"Estudiante '{nuevoEstudiante.Nombre} {nuevoEstudiante.Apellido}' agregado exitosamente.\n\nEl **Código de Acceso** es: **{codigoAcceso}**", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {

                    MessageBox.Show("Error desconocido al guardar el estudiante. Consulte los detalles en la consola.", "Error de Guardado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al intentar guardar el estudiante: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtClaveTemporal.Clear();
            cmbNivel.SelectedIndex = -1;

            cmbEspecializacion.SelectedValue = -1;
            cmbEspecializacion.Enabled = true;
            txtNombre.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fmrAgregarEstudiante_Load(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e) { }
    }
}