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


        public fmrLogin()
        {
            InitializeComponent();

            _usuariosRepository = new UsuariosRepository(_connectionString);
            _catalogosRepository = new CatalogosRepository(_connectionString);

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

                // 2Inicialización de Usuarios Fijos

                string codAdmin = "ADMIN01";
                if (_usuariosRepository.ObtenerUsuarioPorCodigo(codAdmin) == null)
                {
                    _usuariosRepository.AgregarUsuarioFijo(codAdmin, "adminpass", "Admin");
                }

                string codDocente = "D10100000";
                if (_usuariosRepository.ObtenerUsuarioPorCodigo(codDocente) == null)
                {
                    int idUserDocente = _usuariosRepository.AgregarUsuarioFijo(codDocente, "docentepass", "Docente");
                    _usuariosRepository.AgregarDocente(idUserDocente, "Carla", "Reyes");
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
                    // new frmRegistroNotas(user).Show();
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