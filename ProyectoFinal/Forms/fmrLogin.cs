using ProyectoFinal.Clases;
using ProyectoFinal.Repositorios;
using System;
using System.Configuration;
using System.Windows.Forms;


namespace ProyectoFinal.Forms
{
    public partial class fmrLogin : Form
    {

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ProyectoDB"].ConnectionString;
        private readonly UsuariosRepository _usuariosRepository;
        private readonly CatalogosRepository _catalogosRepository;
        private readonly DocentesRepository _docentesRepository;

        public fmrLogin()
        {
            InitializeComponent();

            _usuariosRepository = new UsuariosRepository(_connectionString);
            _catalogosRepository = new CatalogosRepository(_connectionString);
            _docentesRepository = new DocentesRepository(_connectionString);

            InicializarDatosSistema();
        }

        private void InicializarDatosSistema()
        {
            try
            {
                // Inicialización de Catálogos
                int idNivelBase = _catalogosRepository.InicializarNiveles();
                _catalogosRepository.InicializarEspecializaciones();
                _catalogosRepository.InicializarAsignaturas();

                int? idEspecializacionNull = null;

                // 2Inicializacion de Usuarios Fijos

                string codAdmin = "ADMIN01";
                if (_usuariosRepository.ObtenerUsuarioPorCodigo(codAdmin) == null)
                {
                    _usuariosRepository.AgregarUsuarioFijo(codAdmin, "adminpass", "Admin");
                }

                string codigoDocenteBase = "D10100000";
                string claveDocenteBase = "docentepass";
                string rolDocente = "Docente";

                Docentes docenteBase = new Docentes
                {
                    Nombre = "Carla",
                    Apellido = "Reyes",
                    DUI = "00000000-0",
                    Email = "carla.reyes@escuela.edu",
                    Telefono = "7000-0000",
                    CodigoAcceso = codigoDocenteBase
                };

                if (_docentesRepository.ObtenerDocentePorCodigoAcceso(codigoDocenteBase) == null)
                {
                    _docentesRepository.AgregarDocente(docenteBase, claveDocenteBase, rolDocente);
                }
                else
                {
                    
                    _docentesRepository.ActualizarDocenteBase(docenteBase);
                }

                string codEstudiante = "E20250000";
                if (_usuariosRepository.ObtenerUsuarioPorCodigo(codEstudiante) == null)
                {
                    int idUserEstudiante = _usuariosRepository.AgregarUsuarioFijo(codEstudiante, "estupass", "Estudiante");

                    _usuariosRepository.AgregarEstudiante(
                        idUserEstudiante,
                        "Manuel", "Perez",
                        idNivelBase,
                        idEspecializacionNull
                    );
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al inicializar datos del sistema: " + ex.Message);
            }
        }


        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {

            string codigo = txtCodigo.Text.Trim();
            string contrasena = txtContrasena.Text.Trim();

            if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(contrasena))
            {
                MessageBox.Show("Ingrese código de acceso y contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {

                Usuarios user = _usuariosRepository.ObtenerUsuarioPorCodigo(codigo);

                if (user == null)
                {
                    MessageBox.Show("Código de acceso no registrado o incorrecto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Limpia los campos si el código no existe
                    txtCodigo.Clear();
                    txtContrasena.Clear();
                    txtCodigo.Focus();

                    return;
                }


                string hashIngresado = Encriptador.EncriptarClave(contrasena);

                if (hashIngresado != user.PasswordHash)
                {
                    MessageBox.Show("Contraseña incorrecta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Limpia solo la contraseña si es incorrecta
                    txtContrasena.Clear();
                    txtContrasena.Focus();

                    return;
                }


                Form menuForm = null;

                if (user.Rol == "Admin")
                {
                    new fmrMenuAdmin().Show(); 
                }
                else if (user.Rol == "Docente")
                {
                    new frmRegistroNotas().Show();
                }
                else if (user.Rol == "Estudiante")
                {
                    menuForm = new fmrConsultaExpediente(user, codigo);
                }

                this.Hide();

                if (menuForm != null)
                {
                    menuForm.Show();
                }

            }
            catch (Exception ex)
            {
                // Limpia ambos campos en caso de un error
                txtCodigo.Clear();
                txtContrasena.Clear();
                txtCodigo.Focus();

                MessageBox.Show("Error de Sistema al iniciar sesión: " + ex.Message, "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCrearUsuarioAuxiliar_Click(object sender, EventArgs e)
        {

        }
    }
}