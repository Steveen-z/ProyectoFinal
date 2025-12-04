using ProyectoFinal.Clases;
using ProyectoFinal.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProyectoFinal.Forms
{
    public partial class fmrEditarEstudiante : Form
    {
        private readonly EstudiantesGestionRepository _estudiantesGestionRepository;
        private readonly string _connectionString;
        private Estudiantes _estudianteAEditar;

        private bool _isHandlingSpecializationChange = false;
        private int _lastValidEspecializacionId = -1;

        public fmrEditarEstudiante(Estudiantes estudianteAEditar, string connectionString)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _connectionString = connectionString;
            _estudiantesGestionRepository = new EstudiantesGestionRepository(connectionString);
            _estudianteAEditar = estudianteAEditar;

            CargarComboboxes();
            CargarDatosEstudiante(_estudianteAEditar);

            btnGuardar.Click += btnGuardar_Click;
            btnCancelar.Click += btnCancelar_Click;

            cmbNivel.SelectedIndexChanged += cmbNivel_SelectedIndexChanged;
            cmbEspecializacion.SelectedIndexChanged += cmbEspecializacion_SelectedIndexChanged;
        }

        private void CargarComboboxes()
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
                foreach (var kvp in especializaciones) especializacionesConCentinela.Add(kvp.Key, kvp.Value);

                cmbEspecializacion.DataSource = new BindingSource(especializacionesConCentinela, null);
                cmbEspecializacion.DisplayMember = "Value";
                cmbEspecializacion.ValueMember = "Key";
                cmbEspecializacion.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos de niveles y especializaciones: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCambiarClave_Click(object sender, EventArgs e)
        {
            if (_estudianteAEditar == null || _estudianteAEditar.IdUsuario <= 0)
            {
                MessageBox.Show("No se puede cambiar la contraseña. El ID del usuario no es válido.", "Error de Identificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int idUsuarioEstudiante = _estudianteAEditar.IdUsuario;

            try
            {
                ProyectoFinal.Forms.RecuperarContrasena formRecuperar = new ProyectoFinal.Forms.RecuperarContrasena(idUsuarioEstudiante);

                formRecuperar.ShowDialog();

                if (formRecuperar.DialogResult == DialogResult.OK)
                {
                    MessageBox.Show("La contraseña se ha actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al intentar abrir el formulario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarDatosEstudiante(Estudiantes estudiante)
        {
            txtNombre.Text = estudiante.Nombre;
            txtApellido.Text = estudiante.Apellido;

            cmbNivel.SelectedValue = estudiante.IdNivel;

            cmbEspecializacion.SelectedValue = estudiante.IdEspecializacion.HasValue ?
                                               estudiante.IdEspecializacion.Value :
                                               -1;

            AplicarRestriccionEspecializacion();

            _lastValidEspecializacionId = (int)cmbEspecializacion.SelectedValue;
        }

        private void AplicarRestriccionEspecializacion()
        {
            if (cmbNivel.SelectedValue == null || cmbNivel.SelectedIndex == -1)
                return;

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
            }
        }

        private void cmbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNivel.SelectedValue != null)
            {
                AplicarRestriccionEspecializacion();
            }
        }

        private void cmbEspecializacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isHandlingSpecializationChange)
                return;

            if (cmbEspecializacion.SelectedValue == null)
                return;

            _isHandlingSpecializationChange = true;

            try
            {
                int idEspecializacionActual = (int)cmbEspecializacion.SelectedValue;

                if (cmbNivel.SelectedValue == null)
                    return;

                string nivelSeleccionado = cmbNivel.Text;
                bool isGeneral1Or2 = nivelSeleccionado.Contains("General") &&
                                    (nivelSeleccionado.Contains("1°") || nivelSeleccionado.Contains("2°"));

                bool isSpecializationRequired = !isGeneral1Or2;

                if (isSpecializationRequired && idEspecializacionActual == -1)
                {
                    MessageBox.Show("Para el nivel seleccionado, se requiere elegir una especialización específica. No se puede seleccionar la opción '(Ninguna/Opcional)'.", "Restricción de Nivel", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (_lastValidEspecializacionId > 0)
                    {
                        cmbEspecializacion.SelectedValue = _lastValidEspecializacionId;
                    }
                    else
                    {
                        var items = (BindingSource)cmbEspecializacion.DataSource;
                        var firstSpecialization = items.Cast<KeyValuePair<int, string>>()
                                                        .FirstOrDefault(kvp => kvp.Key > 0);

                        if (firstSpecialization.Key > 0)
                        {
                            cmbEspecializacion.SelectedValue = firstSpecialization.Key;
                            _lastValidEspecializacionId = firstSpecialization.Key;
                        }
                    }
                }
                else
                {
                    _lastValidEspecializacionId = idEspecializacionActual;
                }
            }
            finally
            {
                _isHandlingSpecializationChange = false;
            }
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                cmbNivel.SelectedValue == null || (cmbNivel.SelectedValue is int i && i <= 0))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos (Nombre, Apellido y Nivel).", "Datos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string claveActual = txtClaveActual.Text.Trim();
            string claveNueva = txtNuevaClave.Text.Trim();
            string confirmarClave = txtConfirmarClave.Text.Trim();

            bool intentarCambioClave = !string.IsNullOrEmpty(claveActual) || !string.IsNullOrEmpty(claveNueva) || !string.IsNullOrEmpty(confirmarClave);

            string nivelSeleccionado = cmbNivel.Text;
            bool isSpecializationRequired = !(nivelSeleccionado.Contains("General") && (nivelSeleccionado.Contains("1°") || nivelSeleccionado.Contains("2°")));

            int idEspecializacionSeleccionado = (int)cmbEspecializacion.SelectedValue;
            int? nuevoIdEspecializacion = (idEspecializacionSeleccionado > 0) ? idEspecializacionSeleccionado : (int?)null;

            if (isSpecializationRequired && !nuevoIdEspecializacion.HasValue)
            {
                MessageBox.Show("El nivel seleccionado requiere una especialización específica.", "Validación de Especialización", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int nuevoIdNivel = (int)cmbNivel.SelectedValue;
                bool datosActualizados = false;

                if (_estudianteAEditar.Nombre != txtNombre.Text.Trim() ||
                    _estudianteAEditar.Apellido != txtApellido.Text.Trim() ||
                    _estudianteAEditar.IdNivel != nuevoIdNivel ||
                    _estudianteAEditar.IdEspecializacion != nuevoIdEspecializacion)
                {
                    if (_estudiantesGestionRepository.ActualizarEstudiante(
                        _estudianteAEditar.IdEstudiante,
                        txtNombre.Text.Trim(),
                        txtApellido.Text.Trim(),
                        nuevoIdNivel,
                        nuevoIdEspecializacion))
                    {
                        _estudianteAEditar.Nombre = txtNombre.Text.Trim();
                        _estudianteAEditar.Apellido = txtApellido.Text.Trim();
                        _estudianteAEditar.IdNivel = nuevoIdNivel;
                        _estudianteAEditar.IdEspecializacion = nuevoIdEspecializacion;
                        datosActualizados = true;
                    }
                }

                if (intentarCambioClave)
                {
                    if (string.IsNullOrEmpty(claveActual))
                    {
                        MessageBox.Show("Debe ingresar la Clave Actual para poder cambiar la contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClaveActual.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(claveNueva) || string.IsNullOrEmpty(confirmarClave))
                    {
                        MessageBox.Show("Debe ingresar y confirmar la Clave Nueva.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtNuevaClave.Focus();
                        return;
                    }

                    if (claveNueva != confirmarClave)
                    {
                        MessageBox.Show("La Clave Nueva y la Confirmación no coinciden.", "Validación de Clave", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtConfirmarClave.Focus();
                        return;
                    }

                    string hashActualAlmacenado = _estudiantesGestionRepository.ObtenerPasswordHash(_estudianteAEditar.IdUsuario);

                    if (string.IsNullOrEmpty(hashActualAlmacenado))
                    {
                        MessageBox.Show("No se encontró registro de clave para este estudiante. Contacte al administrador.", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string hashActualIngresado = Encriptador.EncriptarClave(claveActual);

                    if (hashActualIngresado != hashActualAlmacenado)
                    {
                        MessageBox.Show("La Clave Actual ingresada es incorrecta. No se pudo cambiar la contraseña.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtClaveActual.Focus();
                        return;
                    }

                    string nuevoPasswordHash = Encriptador.EncriptarClave(claveNueva);

                    if (_estudiantesGestionRepository.ActualizarClave(_estudianteAEditar.IdUsuario, nuevoPasswordHash))
                    {
                        datosActualizados = true;
                    }

                    txtClaveActual.Clear();
                    txtNuevaClave.Clear();
                    txtConfirmarClave.Clear();
                }

                if (datosActualizados)
                {
                    MessageBox.Show("Los datos del estudiante fueron actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Conflicto de datos. Revise los campos.", "Dato Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fmrEditarEstudiante_Load(object sender, EventArgs e)
        {

        }
    }
}