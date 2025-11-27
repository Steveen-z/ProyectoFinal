using ProyectoFinal.Clases;
using ProyectoFinal.Repositorios;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrEdicionAsignatura : Form
    {
        private readonly CatalogosRepository _catalogosRepository;
        private readonly int _idAsignatura;
        private readonly fmrGestionAsignaturas _formPadre;
        private readonly string _connectionString;

        public fmrEdicionAsignatura(int idAsignatura, fmrGestionAsignaturas formPadre)
        {
            InitializeComponent();
            _connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;
            _catalogosRepository = new CatalogosRepository(_connectionString);
            _idAsignatura = idAsignatura;
            _formPadre = formPadre;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Editar Asignatura";

            btnGuardarMateria.Click += btnGuardarMateria_Click;
            btnCancelar.Click += btnCancelar_Click;
            if (this.Controls.ContainsKey("btnEliminar"))
            {
                ((Button)this.Controls["btnEliminar"]).Click += btnEliminar_Click;
            }

            CargarCatalogos();
            CargarDatosAsignatura();
        }

        // cargar datos

        private void CargarCatalogos()
        {
            CargarEspecializaciones();
            CargarNiveles();
        }

        private void CargarEspecializaciones()
        {
            try
            {
                DataTable dtEspecializacion = _catalogosRepository.ObtenerEspecializacionesDataTable();

                // 1. Añadir opción 'Ninguna' para Asignaturas Generales (IdEspecializacion = NULL)
                DataRow rowNulo = dtEspecializacion.NewRow();
                rowNulo["IdEspecializacion"] = DBNull.Value;
                rowNulo["NombreEspecializacion"] = "--- Ninguna (Bachillerato General) ---";
                dtEspecializacion.Rows.InsertAt(rowNulo, 0);

                cmbEspecializacion.DataSource = dtEspecializacion;
                cmbEspecializacion.DisplayMember = "NombreEspecializacion";
                cmbEspecializacion.ValueMember = "IdEspecializacion";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar especializaciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarNiveles()
        {
            try
            {
                DataTable dtNivel = _catalogosRepository.ObtenerNivelesDataTable();

                DataTable dtNivelConTipo = dtNivel.Clone();
                dtNivelConTipo.Columns["IdNivel"].DataType = typeof(System.Int32);

                foreach (DataRow row in dtNivel.Rows)
                {
                    dtNivelConTipo.ImportRow(row);
                }

                cmbNivel.DataSource = dtNivelConTipo;
                cmbNivel.DisplayMember = "NombreCompleto";
                cmbNivel.ValueMember = "IdNivel";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar niveles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatosAsignatura()
        {
            try
            {
                Asignaturas asignatura = _catalogosRepository.ObtenerAsignaturaPorId(_idAsignatura);

                if (asignatura != null)
                {
                    txtNombreMateria.Text = asignatura.NombreAsignatura;

                    bool estaEnUso = _catalogosRepository.AsignaturaEstaEnUso(_idAsignatura);

                    if (estaEnUso)
                    {
                        cmbNivel.Enabled = false;
                        cmbEspecializacion.Enabled = false;

                        MessageBox.Show("Esta asignatura ya está asociada a registros de notas. Solo puede modificar su nombre.",
                                        "Restricción de Edición", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        cmbNivel.Enabled = true;
                        cmbEspecializacion.Enabled = true;
                    }

                    if (asignatura.IdNivel.HasValue)
                    {
                        cmbNivel.SelectedValue = (int)asignatura.IdNivel.Value;
                    }

                    if (asignatura.IdEspecializacion.HasValue)
                    {
                        cmbEspecializacion.SelectedValue = (int)asignatura.IdEspecializacion.Value;
                    }
                    else
                    {
                        cmbEspecializacion.SelectedValue = DBNull.Value;
                    }
                }
                else
                {
                    MessageBox.Show("La asignatura no fue encontrada.", "Error de carga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos de la asignatura. Verifique la coherencia de tipos. Detalle: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- botones

        private void btnGuardarMateria_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreMateria.Text))
            {
                MessageBox.Show("Debe ingresar el nombre de la asignatura.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreMateria.Focus();
                return;
            }

            if (cmbNivel.SelectedValue == null || cmbNivel.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("Debe seleccionar un nivel educativo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string nuevoNombre = txtNombreMateria.Text.Trim();
                int idNivel = Convert.ToInt32(cmbNivel.SelectedValue);

                int? idEspecializacion = cmbEspecializacion.SelectedValue as int?;

                bool exito = _catalogosRepository.ModificarAsignatura(_idAsignatura, nuevoNombre, idNivel, idEspecializacion);

                if (exito)
                {
                    MessageBox.Show("Asignatura modificada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _formPadre.CargarAsignaturas();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo modificar la asignatura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "¿Está seguro que desea eliminar esta asignatura? Esta acción es irreversible.",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool exito = _catalogosRepository.EliminarAsignatura(_idAsignatura);

                    if (exito)
                    {
                        MessageBox.Show("Asignatura eliminada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        _formPadre.CargarAsignaturas();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar la asignatura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fmrEdicionAsignatura_Load(object sender, EventArgs e)
        {

        }
    }
}