using System.Diagnostics;
using EnvioNotificacionesApplication.Services;
using Microsoft.Extensions.DependencyInjection;
using Seguridad;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace EnvioNotificacionesUIWinForms
{
    public partial class Form1 : Form
    {
        private const string NombreServicio = "NotificaciondeCitas"; // ¡Usa el nombre INTERNO exacto de tu servicio!

        //Cifrado _cifrado = new Cifrado();
        //private readonly  _serviceDescriptor;
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider; // Necesario para resolver MainForm

        public Form1(IAuthService authService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _authService = authService;
            _serviceProvider = serviceProvider;
            // Opcional: Configurar para que Enter en el textbox de contraseña active el botón de login
            //this.txtContraseña.KeyDown += new KeyEventHandler(txtContrasena_KeyDown);
            // Ocultar el formulario principal si solo quieres que esté en la bandeja al inicio
            this.WindowState = FormWindowState.Minimized; // Minimiza al inicio
            this.ShowInTaskbar = false;                   // No mostrar en la barra de tareas

        }


        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            //lblMensajeError.Text = string.Empty; // Limpiar mensajes de error anteriores

            //string username = txtNombreUsuario.Text.Trim(); // Obtener el usuario y quitar espacios
            //string password = txtContraseña.Text;           // Obtener la contraseña

            //if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            //{
            //    lblMensajeError.Text = "Por favor, ingrese el usuario y la contraseña.";
            //    return; // Salir si los campos están vacíos
            //}

            //var contra = _cifrado.Encrit(password);
            //var pass = _cifrado.Desencrit(usu); 

            //try
            //{

            //    // El método ahora devuelve un objeto LoginUsuario o null
            //    //var loggedInUser = await _authService.GetByUsernameAsync(username, password);

            //    //if (loggedInUser != null) // Si se obtuvo un usuario, el login fue exitoso
            //    //{
            //        // Opcional: Podrías guardar la información del usuario logeado en algún lugar
            //        // global o pasársela al formulario principal.
            //        // Por ejemplo: CurrentUser.Set(loggedInUser);

            //        this.DialogResult = DialogResult.OK; // Indica que el login fue exitoso
            //        this.Close(); // Cierra el formulario de login
            //    //}
            //    //else
            //    //{
            //    //    lblMensajeError.Text = "Usuario o contraseña incorrectos."; // Mensaje genérico por seguridad
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    // Aquí puedes registrar el error (ej. usando un logger) y mostrar un mensaje genérico.
            //    lblMensajeError.Text = "Ocurrió un error al intentar iniciar sesión. Por favor, intente de nuevo.";
            //    // log.Error("Error en login", ex); // Ejemplo de logging
            //}

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        // Método auxiliar para controlar el servicio (el mismo que ya tenías)
        private void ControlarServicio(string accion)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C net {accion} \"{NombreServicio}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();

                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show($"Servicio '{NombreServicio}' {accion}do correctamente.\n{output}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error al {accion} el servicio '{NombreServicio}'.\n{output}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void iniciarServicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlarServicio("start");
        }

        private void detenerServicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ControlarServicio("stop");
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Asegúrate de limpiar el NotifyIcon antes de salir para que no se quede el icono "fantasma"
            if (notifyIcon1 != null)
            {
                notifyIcon1.Visible = false;
                notifyIcon1.Dispose();
            }
            Application.Exit(); // Cierra la aplicación
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Cancela el cierre real del formulario
                this.Hide();     // Oculta el formulario
                notifyIcon1.ShowBalloonTip(1000, "Control de Servicio", "La aplicación se ha minimizado a la bandeja del sistema.", ToolTipIcon.Info);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
