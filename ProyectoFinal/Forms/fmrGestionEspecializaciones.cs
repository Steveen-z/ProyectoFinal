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
    public partial class fmrGestionEspecializaciones : Form
    {
        private readonly CatalogosRepository _catalogosRepository;
        private List<Especializaciones> _listaEspecializaciones;
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;

        public fmrGestionEspecializaciones()
        {
            InitializeComponent();
            _catalogosRepository = new CatalogosRepository(_connectionString);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Gestión de Especializaciones";

            
            txtBusqueda.TextChanged += new EventHandler(txtBusqueda_TextChanged);
            dgvEspecializaciones.CellDoubleClick += new DataGridViewCellEventHandler(dgvEspecializaciones_CellDoubleClick);

            CargarEspecializaciones();
        }

        private void CargarEspecializaciones()
        {
            try
            {
                _listaEspecializaciones = _catalogosRepository.ObtenerEspecializaciones();

                var dataSource = _listaEspecializaciones
                    .Select(esp => new { esp.IdEspecializacion, esp.NombreEspecializacion })
                    .ToList();

                dgvEspecializaciones.DataSource = dataSource;

                // Configuracin visual de la DataGridView
                if (dgvEspecializaciones.Columns.Contains("IdEspecializacion"))
                {
                    dgvEspecializaciones.Columns["IdEspecializacion"].Visible = false; // Ocultar ID
                }
                if (dgvEspecializaciones.Columns.Contains("NombreEspecializacion"))
                {
                    dgvEspecializaciones.Columns["NombreEspecializacion"].HeaderText = "Especialización (Carrera Técnica)";
                }
                dgvEspecializaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar especializaciones: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // LÓGICA DE BÚSQUEDA EN TIEMPO REAL
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBusqueda.Text.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(filtro))
            {
                var dataSource = _listaEspecializaciones
                    .Select(esp => new { esp.IdEspecializacion, esp.NombreEspecializacion })
                    .ToList();
                dgvEspecializaciones.DataSource = dataSource;
            }
            else
            {
                var resultados = _listaEspecializaciones
                    .Where(esp => esp.NombreEspecializacion.ToLower().Contains(filtro))
                    .Select(esp => new { esp.IdEspecializacion, esp.NombreEspecializacion })
                    .ToList();

                dgvEspecializaciones.DataSource = resultados;
            }

            if (dgvEspecializaciones.Columns.Contains("IdEspecializacion"))
            {
                dgvEspecializaciones.Columns["IdEspecializacion"].Visible = false;
            }
            if (dgvEspecializaciones.Columns.Contains("NombreEspecializacion"))
            {
                dgvEspecializaciones.Columns["NombreEspecializacion"].HeaderText = "Especialización (Carrera Técnica)";
            }
        }




        private void btnAgregarEspecializacion_Click(object sender, EventArgs e)
        {
            fmrAgregarEspecializacion formAgregar = new fmrAgregarEspecializacion();
            formAgregar.DatosActualizados += CargarEspecializaciones;
            formAgregar.ShowDialog();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            // Volver al menú de Catálogos
            fmrMenuGestionCatalogos menuCatalogos = new fmrMenuGestionCatalogos();
            menuCatalogos.Show();
            this.Close();
        }

        private void dgvEspecializaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //Obtener los datos de la fila seleccionada
                if (dgvEspecializaciones.Rows[e.RowIndex].Cells["IdEspecializacion"].Value == null)
                {
                    
                    return;
                }

                int idSeleccionado = Convert.ToInt32(dgvEspecializaciones.Rows[e.RowIndex].Cells["IdEspecializacion"].Value);
                string nombreSeleccionado = dgvEspecializaciones.Rows[e.RowIndex].Cells["NombreEspecializacion"].Value.ToString();

                //Crear y mostrar el formulario de edición
                fmrEdicionEspecializacion formEdicion = new fmrEdicionEspecializacion(idSeleccionado, nombreSeleccionado);

                //Suscribirse al evento para recargar la lista si hay cambios 
                formEdicion.DatosActualizados += CargarEspecializaciones;

                formEdicion.ShowDialog();
            }
        }

        private void fmrGestionEspecializaciones_Load(object sender, EventArgs e)
        {

        }
    }
}