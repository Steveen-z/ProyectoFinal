using ProyectoFinal.Clases;
using ProyectoFinal.Forms;
using ProyectoFinal.Repositorios;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrGestionAsignaturas : Form
    {
        private readonly CatalogosRepository _catalogosRepository;
        private readonly string _connectionString;

        public fmrGestionAsignaturas()
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

        //CARGA Y CONFIGURACN INICIAL


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
                // Llama al método que trae todos los niveles
                DataTable dtNivel = _catalogosRepository.ObtenerNivelesPorEspecializacion(idEspecializacion);

                if (!dtNivel.Columns.Contains("IdNivel"))
                {
                    return;
                }

                // MANIPULACION DEL FILTRO DE COMBOBOX 

                // Creamos una nueva tabla para manipular los tipos y agregar el filtro
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

                // Asignar la fuente de datos
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

                // Aplica la busqueda por texto
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

            // Enlazar evento Doble Clic para Edicin
            dgvAsignaturas.CellDoubleClick += dgvAsignaturas_CellDoubleClick;

            
        }


        // LOGICA DE FILTROS Y BÚSQUEDA

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

        //  Limpiar Limpia todos los filtros y recarga la DGV completa
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombreAsignatura.Clear();

            cmbEspecializacion.SelectedIndex = 0;
            cmbNivel.SelectedIndex = 0;

            CargarAsignaturas();
        }

        

        private void btnAgregarAsignatura_Click(object sender, EventArgs e)
        {
            var altaForm = new fmrRegistroAsignaturas(this);
            altaForm.ShowDialog();
        }

        private void dgvAsignaturas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAsignaturas.Rows[e.RowIndex];

                if (row.Cells["IdAsignatura"].Value != null &&
                    int.TryParse(row.Cells["IdAsignatura"].Value.ToString(), out int idAsignatura))
                {
                    var edicionForm = new fmrEdicionAsignatura(idAsignatura, this);
                    edicionForm.ShowDialog();
                }
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {

            fmrMenuGestionCatalogos menuCatalogos = new fmrMenuGestionCatalogos();
            menuCatalogos.Show();
            this.Close();
        }

        private void fmrGestionAsignaturas_Load(object sender, EventArgs e)
        {

        }
    }
}