using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvioNotificacionesApplication.DTO;
using EnvioNotificacionesApplication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EnvioNotificacionesUIWinForms
{
    public partial class FormObservadosPacientes : Form
    {
        //private readonly ICitaManagementService _citaManagementService;
        private readonly IServiceScopeFactory _scopeFactory; // Necesario si usas scopes por carga
        private List<OrdenDisplayDto> _currenOrden = new List<OrdenDisplayDto>();

        // Necesario si usas scopes por carga
        public FormObservadosPacientes(ICitaManagementService citaManagementService, IServiceScopeFactory scopeFactory)
        {
            InitializeComponent();
            //_citaManagementService = citaManagementService;
            _scopeFactory = scopeFactory; // Asigna si lo inyectas
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewOrdenObser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void FormObservadosPacientes_Load(object sender, EventArgs e)
        {
            await LoadCitasAsync();
        }

        private async Task LoadCitasAsync()
        {
            try
            {
                // Muestra un mensaje de carga o un spinner
                Cursor = Cursors.WaitCursor;

                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedCitaManagementService = scope.ServiceProvider.GetRequiredService<ICitaManagementService>();
                    // Si hay un filtro de fecha y/o estado, usa el nuevo método
                    //if (fechaCita.HasValue || (estadoNotificacionId.HasValue && estadoNotificacionId.Value > 0))
                    //{
                        // Llama al nuevo método para filtrar por estado
                        //_currentCitas = (await scopedCitaManagementService.GetCitasByEstadoNotificacionAsync(estadoNotificacionId.Value)).ToList();
                        _currenOrden = (await scopedCitaManagementService.GetObservadorOrdenAsync()).ToList();
                    //_currentCitas = (await scopedCitaManagementService.GetCitasFilteredAsync(fechaCita, estadoNotificacionId)).ToList();

                   
                    //}
                    //else
                    //{
                    //    // Si no hay filtro (Todos los Estados) o el valor es 0, trae todas las citas
                    //    _currentCitas = (await scopedCitaManagementService.GetAllCitasAsync()).ToList();

                    //}
                }
                dataGridViewOrdenObser.DataSource = _currenOrden;
                dataGridViewOrdenObser.AutoGenerateColumns = false;
                dataGridViewOrdenObser.Columns.Clear();

                // *** Definición de columnas usando las propiedades DIRECTAS del DTO ***

                //dataGridViewCitas.Columns.Add(new DataGridViewTextBoxColumn { Name = "CitaID", HeaderText = "ID Cita", DataPropertyName = "CitaID" });

                var fechaCol = new DataGridViewTextBoxColumn { Name = "FecAte", HeaderText = "Fecha de atencion", DataPropertyName = "FecAte" };
                fechaCol.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dataGridViewOrdenObser.Columns.Add(fechaCol);

                // Ahora el DataPropertyName es simple, porque el DTO ya tiene la propiedad aplanada
                dataGridViewOrdenObser.Columns.Add(new DataGridViewTextBoxColumn { Name = "NumOrd", HeaderText = "Numero de Orden", DataPropertyName = "NumOrd" });

                dataGridViewOrdenObser.Columns.Add(new DataGridViewTextBoxColumn { Name = "NomEmp", HeaderText = "Empresa", DataPropertyName = "NomEmp" });

                dataGridViewOrdenObser.Columns.Add(new DataGridViewTextBoxColumn { Name = "NomPac", HeaderText = "Nombre Completo", DataPropertyName = "NomPac" });

                //var fechaUlt = new DataGridViewTextBoxColumn { Name = "UltimoIntentoBienvenida", HeaderText = "Fecha/Hora Envio de Notificacion", DataPropertyName = "UltimoIntentoBienvenida" };
                //fechaUlt.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                //dataGridViewOrdenObser.Columns.Add(fechaUlt);

                dataGridViewOrdenObser.Columns.Add(new DataGridViewTextBoxColumn { Name = "NroDId", HeaderText = "Numero de DNI", DataPropertyName = "NroDId" });

                dataGridViewOrdenObser.Columns.Add(new DataGridViewTextBoxColumn { Name = "DesTCh", HeaderText = "Tipo de chekeo", DataPropertyName = "DesTCh" });

                //dataGridViewOrdenObser.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstadoFinalizadoNombre", HeaderText = "Estado Finalizado", DataPropertyName = "EstadoFinalizadoNombre" });

                //dataGridViewCitas.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstadoCita", HeaderText = "Estado General Cita", DataPropertyName = "EstadoCita" });


                // Configuraciones visuales
                dataGridViewOrdenObser.ReadOnly = true;
                dataGridViewOrdenObser.AllowUserToAddRows = false;
                dataGridViewOrdenObser.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewOrdenObser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Observados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default; // Restaura el cursor
            }
        }

    }
}
