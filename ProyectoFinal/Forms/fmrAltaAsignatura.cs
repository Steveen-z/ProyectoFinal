using ProyectoFinal.Repositorios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrRegistroAsignaturas : Form
    {
        private readonly CatalogosRepository _catalogosRepository;
        private readonly fmrGestionAsignaturas _formPadre;
        private readonly string _connectionString;

        
        public fmrRegistroAsignaturas(fmrGestionAsignaturas formPadre)
        {
            InitializeComponent();
            _connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;
            _catalogosRepository = new CatalogosRepository(_connectionString);
            _formPadre = formPadre;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Registrar Nueva Asignatura";

            CargarCatalogos();

            btnGuardarMateria.Click += btnGuardarMateria_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        // MMTODOS DE CARGA DE DATOS 

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

                DataRow rowNulo = dtEspecializacion.NewRow();
                rowNulo["IdEspecializacion"] = DBNull.Value;
                rowNulo["NombreEspecializacion"] = "--- Ninguna (Bachillerato General) ---";
                dtEspecializacion.Rows.InsertAt(rowNulo, 0);

                cmbEspecializacion.DataSource = dtEspecializacion;
                cmbEspecializacion.DisplayMember = "NombreEspecializacion";
                cmbEspecializacion.ValueMember = "IdEspecializacion";
                cmbEspecializacion.SelectedIndex = 0; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar especializaciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void fmrAltaAsignatura_Load(object sender, EventArgs e)
        {

        }

        private void CargarNiveles()
        {
            try
            {
                DataTable dtNivel = _catalogosRepository.ObtenerNivelesDataTable();

                DataTable dtNivelConTipo = dtNivel.Clone();
                dtNivelConTipo.Columns["IdNivel"].DataType = typeof(System.Int32);

                DataRow rowFiltro = dtNivelConTipo.NewRow();
                rowFiltro["IdNivel"] = DBNull.Value;
                rowFiltro["NombreCompleto"] = "--- Seleccionar Nivel ---";
                dtNivelConTipo.Rows.InsertAt(rowFiltro, 0);

                foreach (DataRow row in dtNivel.Rows)
                {
                    dtNivelConTipo.ImportRow(row);
                }

                cmbNivel.DataSource = dtNivelConTipo;
                cmbNivel.DisplayMember = "NombreCompleto";
                cmbNivel.ValueMember = "IdNivel";
                cmbNivel.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar niveles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // EVENTOS DE BOTONES

        private void btnGuardarMateria_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtNombreMateria.Text))
            {
                MessageBox.Show("Debe ingresar el nombre de la asignatura.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreMateria.Focus();
                return;
            }

            int? idNivel = null;
            if (cmbNivel.SelectedValue != null && cmbNivel.SelectedValue != DBNull.Value)
            {
                try { idNivel = Convert.ToInt32(cmbNivel.SelectedValue); }
                catch {  }
            }

            if (!idNivel.HasValue)
            {
                MessageBox.Show("Debe seleccionar un nivel educativo válido.", "Validación de Nivel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? idEspecializacion = null;
            if (cmbEspecializacion.SelectedValue != null && cmbEspecializacion.SelectedValue != DBNull.Value)
            {
                try { idEspecializacion = Convert.ToInt32(cmbEspecializacion.SelectedValue); }
                catch {  }
            }

            try
            {
                string nuevoNombre = txtNombreMateria.Text.Trim();
                int idNivelValue = idNivel.Value;

                var idsNivelTecnico = new List<int> { 3, 4, 5, 6 };
                var idsNivelGeneral = new List<int> { 1, 2 }; 

                bool esNivelTecnico = idsNivelTecnico.Contains(idNivelValue);
                bool esNivelGeneral = idsNivelGeneral.Contains(idNivelValue);

                if (esNivelTecnico && !idEspecializacion.HasValue)
                {
                    MessageBox.Show("El Nivel Técnico requiere que se asigne una Especialización.", "Validación de Nivel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (esNivelGeneral && idEspecializacion.HasValue)
                {
                    MessageBox.Show("El Nivel General no puede ser asignado a una Especialización. Seleccione 'Ninguna'.", "Validación de Nivel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_catalogosRepository.AsignaturaYaExiste(nuevoNombre, idNivelValue, idEspecializacion))
                {
                    MessageBox.Show(
                        "La asignatura que intenta registrar ya existe para el Nivel y la Especialización seleccionados.",
                        "Error de Duplicidad",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                bool exito = _catalogosRepository.InsertarAsignatura(nuevoNombre, idNivelValue, idEspecializacion);

                if (exito)
                {
                    MessageBox.Show("Asignatura registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _formPadre.CargarAsignaturas();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo registrar la asignatura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}