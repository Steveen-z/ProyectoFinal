using ProyectoFinal.Clases;
using ProyectoFinal.Forms;
using ProyectoFinal.Repositorios;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class frmRegistroNotas : Form
    {
        private readonly CatalogosRepository _catalogosRepository;
        private readonly string _connectionString;


        public frmRegistroNotas()
        {
            InitializeComponent();
            _connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;
            _catalogosRepository = new CatalogosRepository(_connectionString);

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Gestión de Asignaturas";

            CargarEspecializaciones();
            ConfigurarDataGridView();
            CargarAsignaturas();
        }

        private void CargarEspecializaciones()
        {
            try
            {
                DataTable dtEspecializacion = _catalogosRepository.ObtenerEspecializacionesDataTable();

                DataRow rowFiltro = dtEspecializacion.NewRow();
                rowFiltro["IdEspecializacion"] = DBNull.Value;
                rowFiltro["NombreEspecializacion"] = "--- Seleccionar Especialización ---";
                dtEspecializacion.Rows.InsertAt(rowFiltro, 0);

                cmbEspecializacion.DataSource = dtEspecializacion;
                cmbEspecializacion.DisplayMember = "NombreEspecializacion";
                cmbEspecializacion.ValueMember = "IdEspecializacion";

                cmbEspecializacion.SelectedIndex = 0;

                cmbEspecializacion_SelectedIndexChanged(cmbEspecializacion, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar especializaciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarNiveles(int? idEspecializacion)
        {
            try
            {
                DataTable dtNivel = _catalogosRepository.ObtenerNivelesPorEspecializacion(idEspecializacion);

                if (!dtNivel.Columns.Contains("IdNivel"))
                {
                    return;
                }

                // MANIPULACION DEL FILTRO DE COMBOBOX 

                DataTable dtNivelConFiltro = dtNivel.Clone();

                dtNivelConFiltro.Columns["IdNivel"].DataType = typeof(System.Int32);

                DataRow rowFiltro = dtNivelConFiltro.NewRow();

                rowFiltro["IdNivel"] = DBNull.Value;
                rowFiltro["NombreCompleto"] = "--- Seleccionar Nivel ---";
                dtNivelConFiltro.Rows.InsertAt(rowFiltro, 0);

                foreach (DataRow row in dtNivel.Rows)
                {
                    dtNivelConFiltro.ImportRow(row);
                }

                cmbNivel.DataSource = dtNivelConFiltro;
                cmbNivel.DisplayMember = "NombreCompleto";
                cmbNivel.ValueMember = "IdNivel";
                cmbNivel.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar niveles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CargarAsignaturas(int? idNivel = null, int? idEspecializacion = null)
        {
            try
            {
                dgvAsignaturas.DataSource = _catalogosRepository.ObtenerAsignaturas(idNivel, idEspecializacion);

                txtNombreAsignatura_TextChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaturas: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarDataGridView()
        {
            dgvAsignaturas.ReadOnly = true;
            dgvAsignaturas.AllowUserToAddRows = false;
            dgvAsignaturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAsignaturas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvAsignaturas.CellDoubleClick += dgvAsignaturas_CellDoubleClick;


        }

        private void cmbEspecializacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEspecializacion.SelectedIndex >= 0)
            {
                int? idEspecializacion = cmbEspecializacion.SelectedValue as int?;

                CargarNiveles(idEspecializacion);

                CargarAsignaturas(null, idEspecializacion);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            int? idNivel = cmbNivel.SelectedValue as int?;
            int? idEspecializacion = cmbEspecializacion.SelectedValue as int?;

            CargarAsignaturas(idNivel, idEspecializacion);
        }

        private void txtNombreAsignatura_TextChanged(object sender, EventArgs e)
        {
            if (dgvAsignaturas.DataSource is DataTable dt)
            {
                string texto = txtNombreAsignatura.Text.Trim().Replace("'", "''");

                string filtro = $"NombreAsignatura LIKE '%{texto}%'";

                try
                {
                    dt.DefaultView.RowFilter = filtro;
                }
                catch (Exception)
                {
                    dt.DefaultView.RowFilter = "";
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombreAsignatura.Clear();

            cmbEspecializacion.SelectedIndex = 0;
            cmbNivel.SelectedIndex = 0;

            CargarAsignaturas();
        }


        private void dgvAsignaturas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvAsignaturas.Rows[e.RowIndex];

            if (row.Cells["IdAsignatura"].Value == null)
            {
                MessageBox.Show("No se pudo obtener el ID de la asignatura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int idAsignaturaSeleccionada = Convert.ToInt32(row.Cells["IdAsignatura"].Value);
                string nombreAsignatura = row.Cells["NombreAsignatura"].Value.ToString();

                fmrCapturaNotas formNotas = new fmrCapturaNotas(idAsignaturaSeleccionada, nombreAsignatura);
                formNotas.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la captura de notas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            fmrLogin loginForm = new fmrLogin();
            loginForm.Show();
            this.Close();
        }
        private void frmRegistroNotas_Load(object sender, EventArgs e)
        {

        }
    }
}
