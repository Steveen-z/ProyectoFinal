using ProyectoFinal.Repositorios;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using System.Drawing; 

namespace ProyectoFinal.Forms
{
    public partial class fmrCapturaNotas : Form
    {
        private readonly int _idAsignatura;
        private readonly NotasRepository _notasRepository;
        private DataTable _dtAlumnosNotas;

        public fmrCapturaNotas(int idAsignatura, string nombreAsignatura)
        {
            InitializeComponent();

            _idAsignatura = idAsignatura;
            this.Text = $"Captura de Notas - {nombreAsignatura}";
            this.StartPosition = FormStartPosition.CenterScreen;

            string connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;
            _notasRepository = new NotasRepository(connectionString);

            CargarAlumnosYNotas();

            dgvAlumnos.EditMode = DataGridViewEditMode.EditOnEnter;

            btnRegresar.Click += btnRegresar_Click;
            dgvAlumnos.CellEndEdit += dgvAlumnos_CellEndEdit;
            txtNombreAlumno.TextChanged += txtNombreAlumno_TextChanged; 
        }



        private void CargarAlumnosYNotas()
        {
            try
            {
                _dtAlumnosNotas = _notasRepository.ObtenerAlumnosYNotasPorAsignatura(_idAsignatura); 

                dgvAlumnos.DataSource = _dtAlumnosNotas;
                dgvAlumnos.ReadOnly = false;

                if (_dtAlumnosNotas != null && _dtAlumnosNotas.Rows.Count > 0)
                {
                    // Ocultar columnas de IDs y de Nivel/Especialización
                    if (dgvAlumnos.Columns.Contains("IdAlumno"))
                        dgvAlumnos.Columns["IdAlumno"].Visible = false;
                    if (dgvAlumnos.Columns.Contains("IdNivel"))
                        dgvAlumnos.Columns["IdNivel"].Visible = false;
                    if (dgvAlumnos.Columns.Contains("IdEspecializacion"))
                        dgvAlumnos.Columns["IdEspecializacion"].Visible = false;

                    // Hacer la columna de nombre de solo lectura
                    if (dgvAlumnos.Columns.Contains("NombreCompleto"))
                    {
                        dgvAlumnos.Columns["NombreCompleto"].ReadOnly = true;
                        dgvAlumnos.Columns["NombreCompleto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
                else if (_dtAlumnosNotas != null && _dtAlumnosNotas.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron estudiantes para esta asignatura. Verifique que la asignatura y los estudiantes coincidan en Nivel/Especialización.", "Sin Estudiantes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar alumnos y notas: {ex.Message}", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void fmrCapturaNotas_Load(object sender, EventArgs e)
        {

        }

        private void txtNombreAlumno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string filtro = txtNombreAlumno.Text.Trim();

                if (_dtAlumnosNotas != null)
                {
                    // Usa DataView para aplicar el filtro de búsqueda
                    DataView dv = new DataView(_dtAlumnosNotas);

                    dv.RowFilter = $"NombreCompleto LIKE '%{filtro}%'";

                    dgvAlumnos.DataSource = dv;
                }
            }
            catch (Exception)
            {
            }
        }

 

        private void dgvAlumnos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dgvAlumnos.Columns[e.ColumnIndex].Name;

            if (columnName.StartsWith("NotaP"))
            {
                int idEstudiante = Convert.ToInt32(dgvAlumnos.Rows[e.RowIndex].Cells["IdAlumno"].Value);
                decimal nota;

                if (decimal.TryParse(dgvAlumnos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString(), out nota))
                {
                    // Validación de Rango: 0.1 a 10.0
                    if (nota < 0.1M || nota > 10.0M)
                    {
                        MessageBox.Show("La nota debe estar en el rango de 0.1 a 10.0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dgvAlumnos.CancelEdit();
                        return;
                    }

                    string periodoNombre = "";
                    if (columnName == "NotaP1") periodoNombre = "Periodo 1";
                    else if (columnName == "NotaP2") periodoNombre = "Periodo 2";
                    else if (columnName == "NotaP3") periodoNombre = "Periodo 3"; 
                    else if (columnName == "NotaP4") periodoNombre = "Periodo 4";

                    if (string.IsNullOrEmpty(periodoNombre))
                    {
                        MessageBox.Show($"Error: Columna {columnName} no mapeada a un Periodo de BD.", "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try
                    {
                        bool exito = _notasRepository.GuardarNota(idEstudiante, _idAsignatura, periodoNombre, nota);

                        if (exito)
                        {
                            dgvAlumnos.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            MessageBox.Show("Error al guardar la nota.", "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dgvAlumnos.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightPink;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dgvAlumnos.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightPink;
                    }
                }
                else
                {
                    MessageBox.Show("El valor ingresado no es un número válido.", "Error de Entrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgvAlumnos.CancelEdit();
                }
            }
        }



        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}