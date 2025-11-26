using ProyectoFinal.Clases;
using ProyectoFinal.Repositorios;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace ProyectoFinal.Forms
{
    public partial class fmrConsultaExpediente : Form
    {
        private readonly Usuarios _usuarioActual;
        private readonly string _codigoAcceso; // Almacena el código que se pasó desde el Login
        private readonly ExpedienteRepository _expedienteRepository;
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;

        // El constructor ahora recibe el objeto Usuario y el Código de Acceso como string.
        public fmrConsultaExpediente(Usuarios user, string codigoAcceso)
        {
            InitializeComponent();
            _usuarioActual = user;
            _codigoAcceso = codigoAcceso; // Se guarda el código de acceso ingresado.
            _expedienteRepository = new ExpedienteRepository(_connectionString);

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Expediente Académico del Estudiante";

            CargarDatosEstudiante();
            CargarNotas();

            // Asignar el evento para pintar las filas
            dgvNotas.CellFormatting += new DataGridViewCellFormattingEventHandler(dgvNotas_CellFormatting);
        }

        private void CargarDatosEstudiante()
        {
            try
            {
                // Obtener datos personales/académicos del estudiante 
                var datosEstudiante = _expedienteRepository.ObtenerDetalleEstudiante(_usuarioActual.IdUsuario);

                if (datosEstudiante != null)
                {
                    txtNombreEstudiante.Text = $"{datosEstudiante.Nombre} {datosEstudiante.Apellido}";

                    txtCodigoAcceso.Text = _codigoAcceso;

                    string especializacion = string.IsNullOrWhiteSpace(datosEstudiante.Especializacion)
                        ? "Bachillerato General"
                        : datosEstudiante.Especializacion;

                    txtNivelEspecializacion.Text = $" {datosEstudiante.TipoBachillerato} {datosEstudiante.Anio}° - {especializacion}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos del estudiante: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarNotas()
        {
            try
            {
                // Obtener la lista de asignaturas y notas (ahora incluye P1, P2, P3, P4 y NotaFinal)
                DataTable dtNotas = _expedienteRepository.ObtenerNotasPorEstudiante(_usuarioActual.IdUsuario);

                dgvNotas.DataSource = dtNotas;

                //  Configuración visual y de formato de la DataGridView

                if (dgvNotas.Columns.Contains("IdAsignatura"))
                {
                    dgvNotas.Columns["IdAsignatura"].Visible = false;
                }

                if (dgvNotas.Columns.Contains("NombreAsignatura"))
                {
                    dgvNotas.Columns["NombreAsignatura"].HeaderText = "Materia";
                    dgvNotas.Columns["NombreAsignatura"].Width = 250;
                }

                if (dgvNotas.Columns.Contains("P1"))
                {
                    // Títulos de Encabezado
                    dgvNotas.Columns["P1"].HeaderText = "Per. 1";
                    dgvNotas.Columns["P2"].HeaderText = "Per. 2";
                    dgvNotas.Columns["P3"].HeaderText = "Per. 3";
                    dgvNotas.Columns["P4"].HeaderText = "Per. 4";

                    // Ancho para todas las columnas de período
                    int periodoWidth = 80;
                    dgvNotas.Columns["P1"].Width = periodoWidth;
                    dgvNotas.Columns["P2"].Width = periodoWidth;
                    dgvNotas.Columns["P3"].Width = periodoWidth;
                    dgvNotas.Columns["P4"].Width = periodoWidth;

                    dgvNotas.Columns["P1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvNotas.Columns["P2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvNotas.Columns["P3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvNotas.Columns["P4"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    // Formato de dos decimales para las notas de período
                    string format = "N2";
                    dgvNotas.Columns["P1"].DefaultCellStyle.Format = format;
                    dgvNotas.Columns["P2"].DefaultCellStyle.Format = format;
                    dgvNotas.Columns["P3"].DefaultCellStyle.Format = format;
                    dgvNotas.Columns["P4"].DefaultCellStyle.Format = format;
                }

                if (dgvNotas.Columns.Contains("NotaFinal"))
                {
                    dgvNotas.Columns["NotaFinal"].HeaderText = "PROM. FINAL";
                    dgvNotas.Columns["NotaFinal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvNotas.Columns["NotaFinal"].Width = 120;

                    // Resaltar el promedio final con negritas
                    dgvNotas.Columns["NotaFinal"].DefaultCellStyle.Font = new System.Drawing.Font(dgvNotas.Font, System.Drawing.FontStyle.Bold);

                    dgvNotas.Columns["NotaFinal"].DefaultCellStyle.Format = "N2";
                }

                dgvNotas.ReadOnly = true;
                dgvNotas.AllowUserToAddRows = false;
                dgvNotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvNotas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar notas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // PINTAR DE ROJO LAS NOTAS INFERIORES A 6.0
        private void dgvNotas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Aplicar formato solo a la columna 'NotaFinal'
            if ((dgvNotas.Columns[e.ColumnIndex].Name == "NotaFinal" && e.Value != null))
            {
                if (double.TryParse(e.Value.ToString(), out double nota))
                {
                    // Notas inferiores a 6.0
                    if (nota < 6.0)
                    {
                        e.CellStyle.BackColor = System.Drawing.Color.LightCoral;
                        e.CellStyle.ForeColor = System.Drawing.Color.DarkRed;
                    }
                    else
                    {
                        e.CellStyle.BackColor = System.Drawing.Color.LightGreen;
                        e.CellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            fmrLogin loginForm = new fmrLogin();
            loginForm.Show();
            this.Close();
        }
    }
}