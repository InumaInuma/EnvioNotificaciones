using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvioNotificacionesApplication.DTO;
using EnvioNotificacionesApplication.Service;
using EnvioNotificacionesApplication.Services;
using EnvioNotificacionesDomian.Entities;
using EnvioNotificacionesDomian.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace EnvioNotificacionesUIWinForms
{
    // Define una pequeña clase interna o un record para los elementos del ComboBox
    public record ComboBoxItem(string DisplayText, int Value);

    public partial class FormInicio : Form
    {
        private const string NombreServicio = "NotificaciondeCitas"; // ¡Usa el nombre INTERNO exacto de tu servicio!
        //private readonly ICitaManagementService _citaManagementService;
        private readonly IServiceScopeFactory _scopeFactory; // Necesario si usas scopes por carga
        // La lista ahora contendrá CitaDisplayDto
        private List<CitaDisplayDto> _currentCitas = new List<CitaDisplayDto>();
        private List<OrdenDisplayDto> _currentOrdenesObservadas = new List<OrdenDisplayDto>();
        //private List<OrdenDisplayDto> _currenOrden = new List<OrdenDisplayDto>(); // Para almacenar las citas y poder manipularlas

        // Variable para controlar la vista actual
        private bool _isObservadosView = false;

        // El constructor recibe ICitaManagementService gracias a la inyección de dependencias
        public FormInicio(ICitaManagementService citaManagementService, IServiceScopeFactory scopeFactory)
        {
            InitializeComponent();

            //_citaManagementService = citaManagementService;
            _scopeFactory = scopeFactory; // Asigna si lo inyectas
            // Configura aquí tu DataGridView o control de lista si no lo haces en el diseñador
            SetupDataGridView();
            SetupDataGridViewObservados(); // ¡NUEVO MÉTODO! Configura dataGridViewObservados
            SetupEstadoNotificacionComboBox(); // ¡NUEVO MÉTODO!
            // *** Reemplazamos SetupMonthCalendar por SetupDatePickers ***
            SetupDatePickers();

            // Configuración del Timer si lo haces por código
            //timerActualizacionCitas = new System.Windows.Forms.Timer();
            //timerActualizacionCitas.Interval = 120000; // 2 minutos en milisegundos
            //timerActualizacionCitas.Tick += timerActualizacionCitas_Tick; // Suscribe el evento // No lo inicies aquí, lo iniciaremos en el Load del formulario.
            // Suscribe el evento FormClosing para detener el Timer
            this.FormClosing += FormInicio_FormClosing;
            // Inicializa la vista por defecto (Citas)
            ShowCitasView();
        }

        private async void FormInicio_Load(object sender, EventArgs e)
        {
            // No cargamos las citas aquí todavía, el ComboBox las disparará al seleccionar "Todos"
            // o al establecer un valor por defecto.
            //await LoadCitasAsync();
            // Carga inicial de citas al abrir el formulario
            await LoadCitasAsync(dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date, (cmbEstadoNotificacion.SelectedItem as ComboBoxItem)?.Value);

            if (timerActualizacionCitas != null)
            {
                timerActualizacionCitas.Start();
            }
        }



        private void ControlarServicio(string accion)
        {
            try
            {
                // Crea un nuevo proceso para ejecutar el comando cmd
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe", // Ejecuta el intérprete de comandos
                    Arguments = $"/C net {accion} \"{NombreServicio}\"", // /C ejecuta el comando y luego cierra cmd
                    RedirectStandardOutput = true, // Redirige la salida estándar para capturarla
                    UseShellExecute = false,       // No usar el shell para ejecutar (necesario para redirección)
                    CreateNoWindow = true          // No crear una ventana de consola visible
                };

                using (Process process = Process.Start(psi))
                {
                    // Espera a que el proceso termine y captura su salida
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();

                    // Verifica si el comando fue exitoso
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

        // --- ¡NUEVO MÉTODO PARA CONFIGURAR LOS DATETIMEPICKERS! ---
        private void SetupDatePickers()
        {
            // Establece la fecha de hoy por defecto en ambos DateTimePickers
            dtpFechaInicio.Value = DateTime.Today;
            dtpFechaFin.Value = DateTime.Today; // Por defecto el rango es solo hoy

            //No es necesario suscribirse al evento ValueChanged si tienes un botón de "Filtrar"
            // dtpFechaInicio.ValueChanged += DtpFecha_ValueChanged;
            //dtpFechaFin.ValueChanged += DtpFecha_ValueChanged;
        }

        // --- MÉTODOS PARA CAMBIAR LA VISTA ---
        private void ShowCitasView()
        {
            panelObservados.Visible = false;
            panelCitas.Visible = true;

            lblFiltroestado.Visible = true;
            cmbEstadoNotificacion.Visible = true;
            // *** Hacer visibles los nuevos DateTimePickers y el botón de filtro ***
            dtpFechaInicio.Visible = true;
            dtpFechaFin.Visible = true;
            lblFechaInicio.Visible = true; // Si tienes labels para las fechas
            lblFechaFin.Visible = true;   // Si tienes labels para las fechas
            btnAplicarFiltros.Visible = true; // El nuevo botón de filtro

            btnObservados.Visible = true;
        }

        private async void ShowObservadosView()
        {
            panelCitas.Visible = false;
            panelObservados.Visible = true;

            lblFiltroestado.Visible = false;
            cmbEstadoNotificacion.Visible = false;
            // *** Ocultar los DateTimePickers y el botón de filtro ***
            dtpFechaInicio.Visible = false;
            dtpFechaFin.Visible = false;
            lblFechaInicio.Visible = false;
            lblFechaFin.Visible = false;
            btnAplicarFiltros.Visible = false;

            btnObservados.Visible = false;
            await LoadOrdenesObservadasAsync();// Carga los datos de observados al cambiar a esta vista
        }




        private void SetupEstadoNotificacionComboBox()
        {
            // Opcional: Si quieres un estado "Todos" que no filtre
            var items = new List<ComboBoxItem>
        {
            new ComboBoxItem("Todos los Estados", 0), // Un valor 0 o -1 para indicar 'sin filtro' que me traiga pendientes , enviado , fallido , el tipo de dato es bit en sql 
            new ComboBoxItem("Pendiente", (int)EstadoNotificacionEnum.PENDIENTE), // cuando sea iguala null ,el tipo de dato es bit en sql 
            new ComboBoxItem("Enviado", (int)EstadoNotificacionEnum.ENVIADO), // cuando sea igual a 1 ,el tipo de dato es bit en sql 
            new ComboBoxItem("Fallido", (int)EstadoNotificacionEnum.FALLIDO) // cuando sea igual a 0 , el tipo de dato es bit en sql 
            // Asegúrate de que EstadoNotificacionEnum está accesible aquí,
            // si no, podrías usar los números directamente o referenciar el namespace.
            // using EnvioNotificacionesDomian.Enums;
        };

            cmbEstadoNotificacion.DataSource = items;
            cmbEstadoNotificacion.DisplayMember = "DisplayText"; // La propiedad a mostrar
            cmbEstadoNotificacion.ValueMember = "Value";         // La propiedad que es el valor real

            // Selecciona el primer elemento por defecto ("Todos los Estados")
            cmbEstadoNotificacion.SelectedIndex = 0;

            // Suscribe al evento SelectedIndexChanged para filtrar cuando el usuario seleccione una opción
            cmbEstadoNotificacion.SelectedIndexChanged += cmbEstadoNotificacion_SelectedIndexChanged;
        }

        private async Task LoadCitasAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null, int? estadoNotificacionId = null)
        {
            try
            {
                // Muestra un mensaje de carga o un spinner
                Cursor = Cursors.WaitCursor;

                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedCitaManagementService = scope.ServiceProvider.GetRequiredService<ICitaManagementService>();

                    _currentCitas = (await scopedCitaManagementService.GetCitasFilteredAsync(fechaInicio, fechaFin, estadoNotificacionId)).ToList();


                }
                dataGridViewCitas.DataSource = _currentCitas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las citas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default; // Restaura el cursor
            }
        }

        // --- NUEVO MÉTODO para cargar Órdenes Observadas (movido desde FormObservadosPacientes) ---
        private async Task LoadOrdenesObservadasAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedCitaManagementService = scope.ServiceProvider.GetRequiredService<ICitaManagementService>();
                    _currentOrdenesObservadas = (await scopedCitaManagementService.GetObservadorOrdenAsync()).ToList();
                }
                dataGridViewObservados.DataSource = _currentOrdenesObservadas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Órdenes Observadas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // _logger.LogError(ex, "Error al cargar órdenes observadas en FormInicio: {Message}", ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        private void SetupDataGridView()
        {
            dataGridViewCitas.AutoGenerateColumns = false;
            dataGridViewCitas.Columns.Clear();

            // *** Definición de columnas usando las propiedades DIRECTAS del DTO ***

            //dataGridViewCitas.Columns.Add(new DataGridViewTextBoxColumn { Name = "CitaID", HeaderText = "ID Cita", DataPropertyName = "CitaID" });

            var fechaCol = new DataGridViewTextBoxColumn { Name = "FechaHoraProgramada", HeaderText = "Fecha de Cita Generada", DataPropertyName = "FechaHoraProgramada" };
            fechaCol.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dataGridViewCitas.Columns.Add(fechaCol);

            // Ahora el DataPropertyName es simple, porque el DTO ya tiene la propiedad aplanada
            dataGridViewCitas.Columns.Add(new DataGridViewTextBoxColumn { Name = "PacienteNombreCompleto", HeaderText = "Paciente", DataPropertyName = "PacienteNombreCompleto" });

            dataGridViewCitas.Columns.Add(new DataGridViewTextBoxColumn { Name = "Empresa", HeaderText = "Empresa", DataPropertyName = "Empresa" });

            dataGridViewCitas.Columns.Add(new DataGridViewTextBoxColumn { Name = "TipoCitaDescripcion", HeaderText = "Tipo de Cita", DataPropertyName = "TipoCitaDescripcion" });

            var fechaUlt = new DataGridViewTextBoxColumn { Name = "UltimoIntentoBienvenida", HeaderText = "Fecha/Hora Envio de Notificacion", DataPropertyName = "UltimoIntentoBienvenida" };
            fechaUlt.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dataGridViewCitas.Columns.Add(fechaUlt);

            dataGridViewCitas.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstadoBienvenidaNombre", HeaderText = "Estado Bienvenida", DataPropertyName = "EstadoBienvenidaNombre" });

            dataGridViewCitas.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstadoRecordatorioNombre", HeaderText = "Estado Recordatorio", DataPropertyName = "EstadoRecordatorioNombre" });

            //dataGridViewCitas.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstadoFinalizadoNombre", HeaderText = "Estado Finalizado", DataPropertyName = "EstadoFinalizadoNombre" });

            //dataGridViewCitas.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstadoCita", HeaderText = "Estado General Cita", DataPropertyName = "EstadoCita" });


            // Configuraciones visuales
            dataGridViewCitas.ReadOnly = true;
            dataGridViewCitas.AllowUserToAddRows = false;
            dataGridViewCitas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCitas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // --- NUEVO MÉTODO: Configuración del DataGridView de Observados (movido desde FormObservadosPacientes) ---
        private void SetupDataGridViewObservados()
        {
            dataGridViewObservados.AutoGenerateColumns = false;
            dataGridViewObservados.Columns.Clear();

            var fechaCol = new DataGridViewTextBoxColumn { Name = "FecAte", HeaderText = "Fecha de atención", DataPropertyName = "FecAte" };
            fechaCol.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dataGridViewObservados.Columns.Add(fechaCol);

            dataGridViewObservados.Columns.Add(new DataGridViewTextBoxColumn { Name = "NumOrd", HeaderText = "Número de Orden", DataPropertyName = "NumOrd" });
            dataGridViewObservados.Columns.Add(new DataGridViewTextBoxColumn { Name = "NomEmp", HeaderText = "Empresa", DataPropertyName = "NomEmp" });
            dataGridViewObservados.Columns.Add(new DataGridViewTextBoxColumn { Name = "NomPac", HeaderText = "Nombre Completo", DataPropertyName = "NomPac" });
            dataGridViewObservados.Columns.Add(new DataGridViewTextBoxColumn { Name = "NroDId", HeaderText = "Número de DNI", DataPropertyName = "NroDId" });
            dataGridViewObservados.Columns.Add(new DataGridViewTextBoxColumn { Name = "DesTCh", HeaderText = "Tipo de chequeo", DataPropertyName = "DesTCh" });

            dataGridViewObservados.ReadOnly = true;
            dataGridViewObservados.AllowUserToAddRows = false;
            dataGridViewObservados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewObservados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private async void cmbEstadoNotificacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = cmbEstadoNotificacion.SelectedItem as ComboBoxItem;
            // *** Usar los valores de los DateTimePickers ***
            DateTime fechaInicio = dtpFechaInicio.Value.Date;
            DateTime fechaFin = dtpFechaFin.Value.Date;
            await LoadCitasAsync(fechaInicio, fechaFin, selectedItem?.Value);
        }



        private async void timerActualizacionCitas_Tick(object sender, EventArgs e)
        {
            if (!_isObservadosView)
            {
                timerActualizacionCitas.Stop();
                // *** Usar los valores de los DateTimePickers ***
                DateTime fechaInicio = dtpFechaInicio.Value.Date;
                DateTime fechaFin = dtpFechaFin.Value.Date;
                var selectedEstadoId = (cmbEstadoNotificacion.SelectedItem as ComboBoxItem)?.Value;
                await LoadCitasAsync(fechaInicio, fechaFin, selectedEstadoId);
                timerActualizacionCitas.Start();
            }
        }
        // --- Evento de cierre del formulario para detener el temporizador ---
        private void FormInicio_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timerActualizacionCitas != null)
            {
                timerActualizacionCitas.Stop();
                timerActualizacionCitas.Dispose(); // Libera los recursos
            }
        }


        private void Observados_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAlternarVista_Click(object sender, EventArgs e)
        {
            ShowCitasView();
        }

        private void btnObservados_Click(object sender, EventArgs e)
        {
            // Este botón ahora será el "Ver Observados"
            // Su click simplemente llamará al método para cambiar la vista.
            // Hemos renombrado btnActualizar a btnAlternarVista y lo usamos para alternar.
            // Si quieres un botón separado para esto (que es el Observados_Click),
            // puedes tenerlo y que llame a ShowObservadosView directamente.
            // Pero si es el mismo botón que antes era "Actualizar" y ahora alterna,
            // entonces el btnAlternarVista_Click es el que hace el trabajo.

            // Si btnObservados es un botón aparte, simplemente llama a ShowObservadosView()
            ShowObservadosView();
        }

        private async void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio = dtpFechaInicio.Value.Date;
            DateTime fechaFin = dtpFechaFin.Value.Date;
            var selectedEstadoId = (cmbEstadoNotificacion.SelectedItem as ComboBoxItem)?.Value;

            // Validación básica: la fecha de fin no puede ser anterior a la de inicio
            if (fechaFin < fechaInicio)
            {
                MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Error de Fecha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (fechaInicio < DateTime.Today)
            {
                MessageBox.Show("La fecha de Inicio no puede ser anterior a la fecha de Hoy.", "Error de Fecha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await LoadCitasAsync(fechaInicio, fechaFin, selectedEstadoId);
        }

        private void btnIniciarServicio_Click(object sender, EventArgs e)
        {
            ControlarServicio("start");
        }

        private void btnDetenerServicio_Click(object sender, EventArgs e)
        {
            ControlarServicio("stop");
        }
    }
}
