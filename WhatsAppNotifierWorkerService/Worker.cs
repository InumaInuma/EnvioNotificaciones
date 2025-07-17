using EnvioNotificacionesApplication.Services;
using EnvioNotificacionesDomian.Repositories;
using EnvioNotificacionesInfrastructure.EnvioNotificacionesDomian.Entities;

namespace WhatsAppNotifierWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory; // Para crear scopes y DbContexts por ciclo

        // Intervalo de chequeo ( configurable )
        private readonly TimeSpan _checkInterval;
        //private const int MAX_RETRIES = 3; // Número máximo de reintentos para cada notificación
        

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            // Leer el intervalo de chequeo desde appsettings.json
            var intervalInSeconds = configuration.GetValue<int>("HostConfiguration:IntervaloChequeoSegundos");
            _checkInterval = TimeSpan.FromSeconds(intervalInSeconds > 0 ? intervalInSeconds : 20); // Por defecto 30 segundos
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de Notificaciones de WhatsApp iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker ejecutándose en: {time}", DateTimeOffset.Now);

                // Cada ciclo del worker debe crear su propio scope para DbContext y servicios "scoped"
                using (var scope = _scopeFactory.CreateScope())
                {
                    // Obtén tu servicio de la capa de Aplicación
                    var citaManagementService = scope.ServiceProvider.GetRequiredService<ICitaManagementService>();
                    var whatsAppService = scope.ServiceProvider.GetRequiredService<IWhatsAppService>();
                    // --- LLAMADAS ACTUALIZADAS CON ICitaManagementService ---
                    // 1. Mensajes de Bienvenida
                    await CheckAndSendWelcomeMessages(citaManagementService, whatsAppService);
                    // 2. Mensajes de Recordatorio
                    await CheckAndSendReminderMessages(citaManagementService, whatsAppService);
                    // 3. Mensajes Finalizados
                    await CheckAndSendFinalizedMessages(citaManagementService, whatsAppService);

                    // --- ¡AQUÍ ES DONDE LLAMAS SAVECHANGESASYNC DEL SERVICIO DE APLICACIÓN! ---
                    // Después de procesar todas las notificaciones en un ciclo, guardas los cambios.
                    await citaManagementService.SaveChangesAsync();

                }

                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("Servicio de Notificaciones de WhatsApp detenido.");
        }
        // --- MÉTODOS DE MANEJO DE NOTIFICACIONES (Modificados para usar ICitaManagementService) ---
        private async Task CheckAndSendWelcomeMessages(ICitaManagementService citaManagementService, IWhatsAppService whatsAppService)
        {
            // Podrías ajustar el rango de tiempo si la creación de citas no es "instantánea"
           var citas = await citaManagementService.GetCitasPendientesBienvenidaAsync();
           if (citas?.Any() == true)
            {
                try
                {
                    foreach (var cita in citas) // Creadas en los últimos 5 min
                    {
                        // Incrementa el número de intentos ANTES de pasarlo al método de actualización
                        cita.NroInt++; // ¡Aquí se suma 1!
                        var NroCel = "51" + cita.NroTlf;
                        _logger.LogInformation("Procesando bienvenida para CitaID: {CitaID}, Paciente: {PacienteNombre}", cita.CodCit, cita.Nombre);
                        try
                        {
                            var success = await whatsAppService.EnviarBienvenidaAsync(
                                NroCel,
                                cita.Nombre,
                                cita.FecCit,
                                cita.DesTCh
                            );

                            if (success)
                            {
                                //cita.MarcarBienvenidaComoEnviada();
                                await citaManagementService.UpdateBienvenidaEnviadoStatusAsync(cita, true, null, 0);
                                _logger.LogInformation("Bienvenida enviada y marcada para CitaID: {CitaID}", cita.CodCit);
                            }
                            else
                            {
                                // Si whatsAppService.EnviarBienvenidaAsync devuelve 'false'
                                // significa que el envío no fue exitoso, pero no necesariamente hubo una excepción.
                                // Podrías tener un método en IWhatsAppService que devuelva el mensaje de error o detalles.
                                // Por ahora, pasaremos un mensaje genérico o un mensaje más detallado si tu servicio lo proporciona.
                                string errorMessage = "El servicio de WhatsApp indicó un fallo en el envío."; // Mensaje por defecto
                                                                                                              // Si tu whatsAppService.EnviarBienvenidaAsync podría devolver un mensaje de error específico,
                                                                                                              // lo recibirías aquí y lo pasarías.
                                //cita.MarcarBienvenidaComoFallida(errorMessage);
                                await citaManagementService.UpdateBienvenidaEnviadoStatusAsync(cita, false, errorMessage, cita.NroInt);
                                _logger.LogWarning("Fallo al enviar bienvenida para CitaID: {CitaID}. Intentos: {Intentos}", cita.CodCit, errorMessage);
                            }
                        }
                        catch (Exception ex)
                        {
                            //cita.MarcarBienvenidaComoFallida(ex.Message);
                            await citaManagementService.UpdateBienvenidaEnviadoStatusAsync(cita, false, ex.Message, cita.NroInt);
                            _logger.LogError(ex, "Excepción al enviar bienvenida para CitaID: {CitaID}. Intentos: {Intentos}", cita.CodCit, ex.Message);
                        }
                        await citaManagementService.SaveChangesAsync(); // Guardar cambios de estado
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al chequear y enviar mensajes de bienvenida.");

                }
                _logger.LogInformation("Notificacion de la cita enviada correctamente");
            }
            else
            {
                _logger.LogInformation("No se encontraron Citas Pendientes");
            }
           
        }

        private async Task CheckAndSendReminderMessages(ICitaManagementService citaManagementService, IWhatsAppService whatsAppService)
        {
            var citas = await citaManagementService.GetCitasPendientesRecordatorioHoyAsync();
            if (citas?.Any() == true)
            {
                try
                {

                    foreach (var cita in citas)
                    {
                        // Incrementa el número de intentos ANTES de pasarlo al método de actualización
                        cita.NroInt++; // ¡Aquí se suma 1!
                        var NroCel = "51" + cita.NroTlf;
                        _logger.LogInformation("Procesando recordatorio para CitaID: {CitaID}, Paciente: {PacienteNombre}", cita.CodCit, cita.Nombre);
                        try
                        {
                            var success = await whatsAppService.EnviarRecordatorioAsync(
                                NroCel,
                                cita.Nombre,
                                cita.FecCit,
                                cita.DesTCh
                            );

                            if (success)
                            {
                                //cita.MarcarRecordatorioComoEnviado();
                                await citaManagementService.UpdateRecordatorioEnviadoStatusAsync(cita, true, null, 0);
                                _logger.LogInformation("Recordatorio enviado y marcado para CitaID: {CitaID}", cita.CodCit);
                            }
                            else
                            {
                                string errorMessage = "El servicio de WhatsApp indicó un fallo en el envío.";
                                //cita.MarcarRecordatorioComoFallido(errorMessage);
                                await citaManagementService.UpdateRecordatorioEnviadoStatusAsync(cita, false, errorMessage, cita.NroInt);
                                _logger.LogWarning("Fallo al enviar recordatorio para CitaID: {CitaID}. Intentos: {Intentos}", cita.CodCit, errorMessage);
                            }
                        }
                        catch (Exception ex)
                        {
                            //cita.MarcarRecordatorioComoFallido(ex.Message);
                            await citaManagementService.UpdateRecordatorioEnviadoStatusAsync(cita, false, ex.Message, cita.NroInt);
                            _logger.LogError(ex, "Excepción al enviar recordatorio para CitaID: {CitaID}. Intentos: {Intentos}", cita.CodCit, ex.Message);
                        }
                        await citaManagementService.SaveChangesAsync(); // Guardar cambios de estado
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al chequear y enviar mensajes de recordatorio.");
                }
                _logger.LogInformation("Recordatorio de la cita enviada correctamente");
            }
            else 
            {
                _logger.LogInformation("No es la hora del recordatorio 6:00 AM");
            }
        
        }

        private async Task CheckAndSendFinalizedMessages(ICitaManagementService citaManagementService, IWhatsAppService whatsAppService)
        {
            try
            {
                var ordens = await citaManagementService.GetObservadorOrdenAgrupadosAsync();
                foreach (var orden in ordens)
                {
                    // Incrementa el número de intentos ANTES de pasarlo al método de actualización
                    orden.NroInt++; // ¡Aquí se suma 1!
                    var NroCel = "51" + orden.NroTlf;
                    _logger.LogInformation("Procesando finalizado para CitaID: {CitaID}, Paciente: {PacienteNombre}", orden.NumOrd, orden.NomPac);
                    // Asegúrate de que los campos Empresa, Interconsultas, CorreoElectronico no sean nulos
                    if (string.IsNullOrEmpty(orden.NomCom.ToString()) || string.IsNullOrEmpty(orden.EspMed) || string.IsNullOrEmpty(orden.CorEle))
                    {
                        _logger.LogWarning("CitaID: {CitaID} no tiene todos los datos necesarios para mensaje de finalizado (Empresa, Interconsultas, Correo).", orden.NumOrd);
                        
                        continue; // Saltar esta orden y pasar a la siguiente
                    }

                    try
                    {
                        var success = await whatsAppService.EnviarFinalizadoAsync(
                            NroCel,
                            orden.NomPac,
                            orden.FecAte, // Puedes usar la fecha de la cita o la fecha actual
                            orden.NomCom,
                            orden.EspMed,
                            orden.CorEle
                        );

                        if (success)
                        {
                            //orden.MarcarFinalizadoComoEnviado();
                            await citaManagementService.UpdateOrdenFinalizadoEnviadoStatusAsync(orden,true, null, 0 );   //
                            _logger.LogInformation("Finalizado enviado y marcado para CitaID: {CitaID}", orden.NumOrd);
                        }
                        else
                        {
                            string errorMessage = "El servicio de WhatsApp indicó un fallo en el envío.";
                            //orden.MarcarFinalizadoComoFallida(errorMessage);
                            await citaManagementService.UpdateOrdenFinalizadoEnviadoStatusAsync(orden, false, errorMessage, orden.NroInt);
                            _logger.LogWarning("Fallo al enviar finalizado para CitaID: {CitaID}. Intentos: {Intentos}", orden.NumOrd, errorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        //orden.MarcarFinalizadoComoFallida(ex.Message);
                        await citaManagementService.UpdateOrdenFinalizadoEnviadoStatusAsync(orden, false, ex.Message, orden.NroInt);
                        _logger.LogError(ex, "Excepción al enviar finalizado para CitaID: {CitaID}. Intentos: {Intentos}", orden.NumOrd, ex.Message);
                    }
                    //await citaManagementService.SaveChangesAsync(); // Guardar cambios de estado
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al chequear y enviar mensajes de finalizado.");
                
            }
            _logger.LogInformation("No se encontraron pendientes para enviar notificacion de Finalizados");
        }


    }
}
