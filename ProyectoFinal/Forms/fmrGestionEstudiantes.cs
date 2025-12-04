using ProyectoFinal.Clases;
using ProyectoFinal.Repositorios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrGestionEstudiantes : Form
    {
        private readonly EstudiantesGestionRepository _estudiantesGestionRepository;
        private readonly string _connectionString;

        private Dictionary<int, string> _niveles;
        private Dictionary<int, string> _especializaciones;

        public fmrGestionEstudiantes()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            _connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;
            _estudiantesGestionRepository = new EstudiantesGestionRepository(_connectionString);

            ConfigurarDataGridView();
            CargarFiltros();
            CargarEstudiantes();

            btnBuscar.Click += btnBuscar_Click;
            btnLimpiar.Click += btnLimpiar_Click;
            //btnAgregarEstudiante.Click += btnAgregarEstudiante_Click;
            dgvEstudiantes.CellDoubleClick += dgvEstudiantes_CellDoubleClick;
            btnRegresar.Click += btnRegresar_Click;

            txtNombreBusqueda.TextChanged += txtNombreBusqueda_TextChanged;
        }


        private void ConfigurarDataGridView()
        {
            dgvEstudiantes.ReadOnly = true;
            dgvEstudiantes.AllowUserToAddRows = false;
            dgvEstudiantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstudiantes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvEstudiantes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }

        private void CargarFiltros()
        {
            try
            {
                _niveles = _estudiantesGestionRepository.ObtenerNiveles();
                _especializaciones = _estudiantesGestionRepository.ObtenerEspecializaciones();

                var nivelesConTodos = new Dictionary<int, string> { { 0, "Todos" } };
                foreach (var kvp in _niveles) nivelesConTodos.Add(kvp.Key, kvp.Value);

                var especializacionesConTodas = new Dictionary<int, string> { { 0, "Todas" } };
                foreach (var kvp in _especializaciones) especializacionesConTodas.Add(kvp.Key, kvp.Value);

                cmbNivel.DataSource = new BindingSource(nivelesConTodos, null);
                cmbNivel.DisplayMember = "Value";
                cmbNivel.ValueMember = "Key";
                cmbNivel.SelectedIndex = 0;

                cmbEspecializacion.DataSource = new BindingSource(especializacionesConTodas, null);
                cmbEspecializacion.DisplayMember = "Value";
                cmbEspecializacion.ValueMember = "Key";
                cmbEspecializacion.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los filtros de Nivel/Especialización: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEstudiantes()
        {
            int? idNivel = null;
            int? idEspecializacion = null;

            string nombreFiltro = txtNombreBusqueda.Text.Trim();
            if (string.IsNullOrEmpty(nombreFiltro))
            {
                nombreFiltro = null;
            }

            if (cmbNivel.SelectedValue != null && (int)cmbNivel.SelectedValue != 0)
            {
                idNivel = (int)cmbNivel.SelectedValue;
            }
            if (cmbEspecializacion.SelectedValue != null && (int)cmbEspecializacion.SelectedValue != 0)
            {
                idEspecializacion = (int)cmbEspecializacion.SelectedValue;
            }

            try
            {
                DataTable dtEstudiantes = _estudiantesGestionRepository.ObtenerEstudiantesFiltrados(idNivel, idEspecializacion, nombreFiltro);

                dgvEstudiantes.DataSource = dtEstudiantes;

                if (dgvEstudiantes.Columns.Contains("IdEstudiante")) dgvEstudiantes.Columns["IdEstudiante"].Visible = false;
                if (dgvEstudiantes.Columns.Contains("IdNivel")) dgvEstudiantes.Columns["IdNivel"].Visible = false;
                if (dgvEstudiantes.Columns.Contains("IdEspecializacion")) dgvEstudiantes.Columns["IdEspecializacion"].Visible = false;

                if (dgvEstudiantes.Columns.Contains("Nombre")) dgvEstudiantes.Columns["Nombre"].HeaderText = "Nombre";
                if (dgvEstudiantes.Columns.Contains("Apellido")) dgvEstudiantes.Columns["Apellido"].HeaderText = "Apellido";
                if (dgvEstudiantes.Columns.Contains("Nivel")) dgvEstudiantes.Columns["Nivel"].HeaderText = "Nivel";
                if (dgvEstudiantes.Columns.Contains("Especializacion")) dgvEstudiantes.Columns["Especializacion"].HeaderText = "Especialización";
                if (dgvEstudiantes.Columns.Contains("CodigoAcceso")) dgvEstudiantes.Columns["CodigoAcceso"].HeaderText = "Usuario";

                //dgvEstudiantes.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la lista de estudiantes: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarEstudiantes();
        }

        private void txtNombreBusqueda_TextChanged(object sender, EventArgs e)
        {
            CargarEstudiantes();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cmbNivel.SelectedIndex = 0;
            cmbEspecializacion.SelectedIndex = 0;
            txtNombreBusqueda.Clear(); 
            CargarEstudiantes();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregarEstudiante_Click(object sender, EventArgs e)
        {
           fmrAgregarEstudiante formAgregar = new fmrAgregarEstudiante(_connectionString);
            if (formAgregar.ShowDialog() == DialogResult.OK)
            {
                CargarEstudiantes();
            }
        }

        private void dgvEstudiantes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int idEstudiante = Convert.ToInt32(dgvEstudiantes.Rows[e.RowIndex].Cells["IdEstudiante"].Value);

                    Estudiantes estudianteAEditar = _estudiantesGestionRepository.ObtenerEstudiantePorId(idEstudiante);

                    if (estudianteAEditar != null)
                    {
                        fmrEditarEstudiante formEditar = new fmrEditarEstudiante(estudianteAEditar, _connectionString);

                        if (formEditar.ShowDialog() == DialogResult.OK)
                        {
                            CargarEstudiantes();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el estudiante para editar.", "Error de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al intentar abrir la edición: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void fmrGestionEstudiantes_Load(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fmrGestionEstudiantes_Load_1(object sender, EventArgs e)
        {

        }
    }
}