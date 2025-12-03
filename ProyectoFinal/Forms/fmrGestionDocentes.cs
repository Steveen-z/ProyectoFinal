using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using ProyectoFinal.Repositorios;

namespace ProyectoFinal.Forms
{
    public partial class fmrGestionDocentes : Form
    {
        private readonly DocentesRepository _docentesRepository;
        private readonly string _connectionString;
        private DataTable _dtDocentesOriginal;

        public fmrGestionDocentes()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            dgvDocentes.CellDoubleClick += dgvDocentes_CellDoubleClick;

            _connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;
            _docentesRepository = new DocentesRepository(_connectionString);


            ConfigurarDataGridView();
            CargarDocentes();
        }

        private void ConfigurarDataGridView()
        {
            dgvDocentes.ReadOnly = true;
            dgvDocentes.AllowUserToAddRows = false;
            dgvDocentes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void CargarDocentes(string filtro = "")
        {
            try
            {
                _dtDocentesOriginal = _docentesRepository.ObtenerTodosLosDocentes("");

                dgvDocentes.DataSource = _dtDocentesOriginal;

                if (dgvDocentes.Columns.Contains("IdDocente")) dgvDocentes.Columns["IdDocente"].Visible = false;
                if (dgvDocentes.Columns.Contains("IdUsuario")) dgvDocentes.Columns["IdUsuario"].Visible = false;
                if (dgvDocentes.Columns.Contains("NombreCompleto")) dgvDocentes.Columns["NombreCompleto"].Visible = false;

                dgvDocentes.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgvDocentes.Columns["DUI"].Width = 100;
                dgvDocentes.Columns["Telefono"].Width = 100;

                if (dgvDocentes.Columns.Contains("Nombre")) dgvDocentes.Columns["Nombre"].HeaderText = "Nombre";
                if (dgvDocentes.Columns.Contains("Apellido")) dgvDocentes.Columns["Apellido"].HeaderText = "Apellido";
                if (dgvDocentes.Columns.Contains("DUI")) dgvDocentes.Columns["DUI"].HeaderText = "DUI";
                if (dgvDocentes.Columns.Contains("Email")) dgvDocentes.Columns["Email"].HeaderText = "Email";
                if (dgvDocentes.Columns.Contains("Telefono")) dgvDocentes.Columns["Telefono"].HeaderText = "Teléfono";
                if (dgvDocentes.Columns.Contains("CodigoAcceso")) dgvDocentes.Columns["CodigoAcceso"].HeaderText = "Usuario";

                dgvDocentes.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la lista de docentes: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string filtro = txtBusqueda.Text.Trim();
            CargarDocentes(filtro);
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
           fmrAgregarDocente formAgregar = new fmrAgregarDocente(_connectionString);

            if (formAgregar.ShowDialog() == DialogResult.OK)
            {
                CargarDocentes();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDocentes.SelectedRows.Count > 0)
            {
                int idDocente = Convert.ToInt32(dgvDocentes.SelectedRows[0].Cells["IdDocente"].Value);

                fmrEditarDocente formEditar = new fmrEditarDocente(_connectionString, idDocente);

                if (formEditar.ShowDialog() == DialogResult.OK)
                {
                    CargarDocentes();
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un docente para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDocentes.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("¿Está seguro de que desea eliminar al docente seleccionado? Esto eliminará también su cuenta de usuario.", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        int idDocente = Convert.ToInt32(dgvDocentes.SelectedRows[0].Cells["IdDocente"].Value);

                        if (_docentesRepository.EliminarDocente(idDocente))
                        {
                            MessageBox.Show("Docente y cuenta de usuario eliminados exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarDocentes();
                        }
                        else
                        {
                            MessageBox.Show("Error al eliminar el docente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocurrió un error inesperado al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un docente para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            if (_dtDocentesOriginal == null) return;

            string filtro = txtBusqueda.Text.Trim();

            DataView dv = new DataView(_dtDocentesOriginal);

            if (string.IsNullOrWhiteSpace(filtro))
            {
                dv.RowFilter = string.Empty;
            }
            else
            {
                string filtroExpresion = $"Nombre LIKE '%{filtro}%' OR Apellido LIKE '%{filtro}%' OR CodigoAcceso LIKE '%{filtro}%'";
                dv.RowFilter = filtroExpresion;
            }

            dgvDocentes.DataSource = dv;
        }

        private void fmrGestionDocentes_Load(object sender, EventArgs e)
        {
        }

        private void dgvDocentes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int idDocente = Convert.ToInt32(dgvDocentes.Rows[e.RowIndex].Cells["IdDocente"].Value);

                    fmrEditarDocente formEditar = new fmrEditarDocente(_connectionString, idDocente);

                    if (formEditar.ShowDialog() == DialogResult.OK)
                    {
                        CargarDocentes();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al intentar abrir la edición: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}