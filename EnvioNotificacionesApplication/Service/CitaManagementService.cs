using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesApplication.DTO;
using EnvioNotificacionesApplication.Services;
using EnvioNotificacionesDomian.Entities;
using EnvioNotificacionesDomian.Enums;
using EnvioNotificacionesDomian.Repositories;
using EnvioNotificacionesInfrastructure.EnvioNotificacionesDomian.Entities;

namespace EnvioNotificacionesApplication.Service
{
    public class CitaManagementService : ICitaManagementService
    {
        private readonly ICitaRepository _citaRepository;

        public CitaManagementService(ICitaRepository citaRepository)
        {
            _citaRepository = citaRepository;
        }

        public async Task<IEnumerable<Cita>> GetCitasPendientesBienvenidaAsync()
        {
            // La lógica del filtro de tiempo (si lo necesitas) debería estar aquí,
            // o pasarse al repositorio como un parámetro si el repositorio lo va a ejecutar en la DB.
            // Por ahora, usamos el método existente que ya filtra estados.
            var citas = await _citaRepository.GetCitasPendientesBienvenidaAsync();

            // Si quieres mantener el filtro de 5 minutos, aplícalo aquí en la capa de aplicación
            // (aunque lo ideal sería que el repositorio lo hiciera si es un filtro de DB).
            //return citas.Where(c => c.FechaHoraGeneracionCita >= DateTime.Now.AddMinutes(-5)).ToList();
            // O si quieres todas las programadas:
            return citas.ToList();
        }

        public async Task<IEnumerable<Cita>> GetCitasPendientesRecordatorioHoyAsync()
        {
            var citas = await _citaRepository.GetCitasPendientesRecordatorioHoyAsync();
            //return citas.Select(c => MapCitaToDisplayDto(c)).ToList();
            return citas.ToList();
        }

        //public async Task<IEnumerable<Cita>> GetCitasPendientesFinalizadoAsync()
        //{
        //    var citas = await _citaRepository.GetCitasPendientesFinalizadoAsync();
        //    //return citas.Select(c => MapCitaToDisplayDto(c)).ToList();
        //    return citas.ToList();
        //}

        // Implementa otros métodos de la interfaz aquí
        public async Task<IEnumerable<CitaDisplayDto>> GetAllCitasAsync()
        {
            // Podrías añadir lógica de negocio aquí antes de llamar al repositorio
            // Por ejemplo, filtrado, paginación, etc.
            var citas = await _citaRepository.GetAllAsync(); // Esto trae las Citas con Includes

            // Mapea las entidades Cita a CitaDisplayDto
            return citas.Select(c => new CitaDisplayDto
            {
                CitaID = c.CodCit, // Mapea CodCit a CitaID
                FechaHoraProgramada = c.FecCit , // Mapea FecCit a FechaHoraProgramada
                Observaciones = c.Observ, // Mapea Observ a Observaciones
                Empresa = c.NomCom ?? string.Empty, // Si CodEmp es un ID, necesitarías cargar el nombre de la empresa
                //Interconsultas = c.Interconsultas,

                // Mapeo de campos de paciente directamente de Cita
                PacienteNombreCompleto = $"{c.Nombre} {c.ApePat} {c.ApeMat}".Trim(),
                PacienteTelefono = c.NroCel ?? string.Empty,
                PacienteCorreoElectronico = c.CorElec ?? string.Empty,

                // Mapeo de tipo de cita (si CodTDo es FK a Constante, necesitarías cargar la descripción de Constante)
                TipoCitaDescripcion = c.DesTCh ?? string.Empty, // Placeholder, si es FK a Constante, necesitarías cargar Constante.Descripcion

                // Uso de las propiedades derivadas de la entidad Cita
                EstadoBienvenidaNombre = c.EstadoNotificacionBienvenida.ToString(),
                EstadoRecordatorioNombre = c.EstadoNotificacionRecordatorio.ToString(),
                //EstadoFinalizadoNombre = c.EstadoNotificacionFinalizado.ToString(),

                // Nuevos campos de error y último intento
                MensajeErrorBienvenida = c.MsjErrW,
                MensajeErrorRecordatorio = c.MsjERW,
                //MensajeErrorFinalizado = c.MsjIRW,
                UltimoIntentoBienvenida = c.FecEWa,
                UltimoIntentoRecordatorio = c.FecRWa,
                //UltimoIntentoFinalizado = c.FecIWa

                //EstadoCitaGeneral = c.EstadoCita.ToString() // Si mantienes este Enum en tu Cita.cs // Convierte el Enum a string para mostrar
            }).ToList();
        }

        // --- ¡IMPLEMENTACIÓN DEL NUEVO MÉTODO SAVECHANGESASYNC! ---
        public async Task<int> SaveChangesAsync()
        {
            return await _citaRepository.SaveChangesAsync();
        }

        //// --- ¡IMPLEMENTACIÓN DEL NUEVO MÉTODO! ---
        //public async Task<IEnumerable<CitaDisplayDto>> GetCitasByEstadoNotificacionAsync(int estadoNotificacionId)
        //{
        //    var citas = await _citaRepository.GetCitasByEstadoNotificacionAsync(estadoNotificacionId);
        //    // Mapea las entidades Cita a CitaDisplayDto
        //    return citas.Select(c => new CitaDisplayDto
        //    {
        //        CitaID = c.CitaID,
        //        FechaHoraProgramada = c.FechaHoraProgramada,
        //        Observaciones = c.Observaciones,
        //        Empresa = c.Empresa,
        //        UltimoIntentoBienvenida = c.UltimoIntentoBienvenida,
        //        Interconsultas = c.Interconsultas,
        //        // Mapeo de las propiedades relacionadas
        //        PacienteNombreCompleto = $"{c.Paciente.Nombre} {c.Paciente.ApellidoPaterno} {c.Paciente.ApellidoMaterno}".Trim(),
        //        TipoCitaDescripcion = c.TipoCita.Descripcion,
        //        EstadoBienvenidaNombre = c.EstadoBienvenida.NombreEstado,
        //        EstadoRecordatorioNombre = c.EstadoRecordatorio.NombreEstado,
        //        EstadoFinalizadoNombre = c.EstadoFinalizado.NombreEstado,
        //        EstadoCita = c.EstadoCita.ToString() // Convierte el Enum a string para mostrar
        //    }).ToList();

        //}

        // --- ¡IMPLEMENTACIÓN DEL NUEVO MÉTODO DE FILTRADO COMBINADO! ---
        public async Task<IEnumerable<CitaDisplayDto>> GetCitasFilteredAsync(DateTime? fechaInicio, DateTime? fechaFin, int? estadoNotificacionId)
        {
            var citas = await _citaRepository.GetCitasFilteredAsync(fechaInicio, fechaFin, estadoNotificacionId);
            return citas.Select(c => new CitaDisplayDto
            {
                CitaID = c.CodCit, // Mapea CodCit a CitaID
                FechaHoraProgramada = c.FecCit, //?? DateTime.MinValue, // Mapea FecCit a FechaHoraProgramada
                Observaciones = c.Observ, // Mapea Observ a Observaciones
                Empresa = c.NomCom ?? string.Empty, // Si CodEmp es un ID, necesitarías cargar el nombre de la empresa
                //Interconsultas = c.Interconsultas,

                // Mapeo de campos de paciente directamente de Cita
                PacienteNombreCompleto = $"{c.Nombre} {c.ApePat} {c.ApeMat}".Trim(),
                PacienteTelefono = c.NroCel ?? string.Empty,
                PacienteCorreoElectronico = c.CorElec ?? string.Empty,

                // Mapeo de tipo de cita (si CodTDo es FK a Constante, necesitarías cargar la descripción de Constante)
                TipoCitaDescripcion = c.DesTCh ?? string.Empty, // Placeholder, si es FK a Constante, necesitarías cargar Constante.Descripcion

                // Uso de las propiedades derivadas de la entidad Cita
                EstadoBienvenidaNombre = c.EstadoNotificacionBienvenida.ToString(),
                EstadoRecordatorioNombre = c.EstadoNotificacionRecordatorio.ToString(),
                //EstadoFinalizadoNombre = c.EstadoNotificacionFinalizado.ToString(),

                // Nuevos campos de error y último intento
                MensajeErrorBienvenida = c.MsjErrW,
                MensajeErrorRecordatorio = c.MsjERW,
                //MensajeErrorFinalizado = c.MsjIRW,
                UltimoIntentoBienvenida = c.FecEWa,
                UltimoIntentoRecordatorio = c.FecRWa,
                //UltimoIntentoFinalizado = c.FecIWa
            }).ToList();
            //    CitaID = c.CodCit,
            //    FechaHoraProgramada = c.FecCit,
            //    Observaciones = c.Observ,
            //    Empresa = c.NomEmp ?? string.Empty,
            //    PacienteNombreCompleto = $"{c.Nombre} {c.ApePat} {c.ApeMat}".Trim(),
            //    PacienteTelefono = c.NroCel ?? string.Empty,
            //    PacienteCorreoElectronico = c.CorElec ?? string.Empty,
            //    TipoCitaDescripcion = c.DesTCh ?? string.Empty,
            //    EstadoBienvenidaNombre = c.EstadoNotificacionBienvenida.ToString(),
            //    EstadoRecordatorioNombre = c.EstadoNotificacionRecordatorio.ToString(),
            //    MensajeErrorBienvenida = c.MsjErrW,
            //    MensajeErrorRecordatorio = c.MsjERW,
            //    UltimoIntentoBienvenida = c.FecEWa,
            //    UltimoIntentoRecordatorio = c.FecRWa,
            //}).ToList();

        }

        // Método existente para todas las citas (para UI)
        //public async Task<IEnumerable<CitaDisplayDto>> GetAllCitasAsync()
        //{
        //    var citas = await _citaRepository.GetAllAsync(); // Trae Citas con Includes

        //    return citas.Select(c => MapCitaToDisplayDto(c)).ToList();
        //}

        // *** NUEVOS MÉTODOS PARA EL WORKER (O CUALQUIER OTRA CAPA QUE NECESITE ESTOS DATOS) ***

        //public async Task<IEnumerable<CitaDisplayDto>> GetCitasProgramadasPendientesBienvenidaAsync()
        //{
        //    var citas = await _citaRepository.GetCitasPendientesBienvenidaAsync();
        //    //return citas.Select(c => MapCitaToDisplayDto(c)).ToList();
        //}



        // *** NUEVOS MÉTODOS PARA ACTUALIZAR ESTADOS ***

        //public async Task UpdateCitaEstadoBienvenidaAsync(int citaId, EstadoNotificacionEnum nuevoEstado)
        //{
        //    var cita = await _citaRepository.GetByIdAsync(citaId);
        //    if (cita == null)
        //    {
        //        throw new InvalidOperationException($"Cita con ID {citaId} no encontrada.");
        //    }
        //    // Aquí puedes aplicar lógica de negocio antes de actualizar
        //    // Por ejemplo: if (nuevoEstado == EstadoNotificacionEnum.ENVIADO) cita.MarcarBienvenidaComoEnviada();
        //    // Esto asume que MarcarBienvenidaComoEnviada ya actualiza el ID del estado.

        //    // Si tu método de entidad solo actualiza el ID, hazlo directamente:
        //    cita.EstadoNotificacionBienvenidaID = (int)nuevoEstado;
        //    // O si tienes el método en la entidad para esto:
        //    // if (nuevoEstado == EstadoNotificacionEnum.ENVIADO) cita.MarcarBienvenidaComoEnviada();
        //    // else if (nuevoEstado == EstadoNotificacionEnum.FALLIDO) cita.MarcarBienvenidaComoFallida();

        //    await _citaRepository.UpdateAsync(cita);
        //    await _citaRepository.SaveChangesAsync();
        //}

        // ... (tus métodos UpdateCitaEstado...Async) ...
        // Estos métodos también necesitarán ser adaptados para usar los nuevos nombres de campos (IndEWa, FecEWa, MsjErrW)
        // y tus métodos de entidad como MarcarBienvenidaComoEnviada(string errorMessage)
        public async Task UpdateCitaEstadoBienvenidaAsync(int citaId, EstadoNotificacionEnum nuevoEstado)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId); // Este GetByIdAsync debe traer la Cita generada
            if (cita == null) throw new InvalidOperationException($"Cita con ID {citaId} no encontrada.");

            if (nuevoEstado == EstadoNotificacionEnum.ENVIADO)
                cita.MarcarBienvenidaComoEnviada();
            else if (nuevoEstado == EstadoNotificacionEnum.FALLIDO)
                cita.MarcarBienvenidaComoFallida("Error desconocido al enviar."); // Necesitarás pasar el mensaje de error real
            else // PENDIENTE
            { cita.IndEWa = null; cita.FecEWa = null; cita.MsjErrW = null; } // Marcar como pendiente

            await _citaRepository.UpdateAsync(cita);
            await _citaRepository.SaveChangesAsync();
        }
        // Repite para Recordatorio y Finalizado

        //public async Task UpdateCitaEstadoRecordatorioAsync(int citaId, EstadoNotificacionEnum nuevoEstado)
        //{
        //    var cita = await _citaRepository.GetByIdAsync(citaId);
        //    if (cita == null)
        //    {
        //        throw new InvalidOperationException($"Cita con ID {citaId} no encontrada.");
        //    }
        //    cita.EstadoNotificacionRecordatorioID = (int)nuevoEstado;
        //    await _citaRepository.UpdateAsync(cita);
        //    await _citaRepository.SaveChangesAsync();
        //}

        //public async Task UpdateCitaEstadoFinalizadoAsync(int citaId, EstadoNotificacionEnum nuevoEstado)
        //{
        //    var cita = await _citaRepository.GetByIdAsync(citaId);
        //    if (cita == null)
        //    {
        //        throw new InvalidOperationException($"Cita con ID {citaId} no encontrada.");
        //    }
        //    cita.EstadoNotificacionFinalizadoID = (int)nuevoEstado;
        //    await _citaRepository.UpdateAsync(cita);
        //    await _citaRepository.SaveChangesAsync();
        //}

        //public async Task UpdateCitaEstadoGeneralAsync(int citaId, EstadoCitaEnum nuevoEstado)
        //{
        //    var cita = await _citaRepository.GetByIdAsync(citaId);
        //    if (cita == null)
        //    {
        //        throw new InvalidOperationException($"Cita con ID {citaId} no encontrada.");
        //    }
        //    cita.ActualizarEstadoCita(nuevoEstado); // Usa el método de tu entidad Cita
        //    await _citaRepository.UpdateAsync(cita);
        //    await _citaRepository.SaveChangesAsync();
        //}

        // *** Método Privado de Mapeo (para reutilizar el código de mapeo) ***
        //private CitaDisplayDto MapCitaToDisplayDto(Cita c)
        //{
        //    return new CitaDisplayDto
        //    {
        //        CitaID = c.CitaID,
        //        FechaHoraProgramada = c.FechaHoraProgramada,
        //        Observaciones = c.Observaciones,
        //        Empresa = c.Empresa,
        //        Interconsultas = c.Interconsultas,
        //        PacienteNombreCompleto = $"{c.Paciente.Nombre} {c.Paciente.ApellidoPaterno} {c.Paciente.ApellidoMaterno}".Trim(),
        //        TipoCitaDescripcion = c.TipoCita.Descripcion,
        //        EstadoBienvenidaNombre = c.EstadoBienvenida.NombreEstado,
        //        EstadoRecordatorioNombre = c.EstadoRecordatorio.NombreEstado,
        //        EstadoFinalizadoNombre = c.EstadoFinalizado.NombreEstado,
        //        EstadoCita = c.EstadoCita.ToString() // Convierte el Enum a string
        //    };
        //}

        public async Task<IEnumerable<OrdenDisplayDto>> GetObservadorOrdenAsync()
        {
            var citas = await _citaRepository.GetObservadorOrdenAsync();
            //return citas.Select(c => MapCitaToDisplayDto(c)).ToList();
            return citas.ToList();
        }

        public async Task<IEnumerable<OrdenDisplayDto>> GetObservadorOrdenAgrupadosAsync()
        {
            var citas = await _citaRepository.GetObservadorOrdenAgrupadosAsync();
            //return citas.Select(c => MapCitaToDisplayDto(c)).ToList();
            return citas.ToList();
        }
        public async Task UpdateBienvenidaEnviadoStatusAsync(Cita cita, bool isSuccess, string errorMessage, int nroInt)
        {
            await _citaRepository.UpdateBienvenidaEnviadoStatusAsync(cita, isSuccess, errorMessage, nroInt);
        }
        public async Task UpdateRecordatorioEnviadoStatusAsync(Cita cita, bool isSuccess, string errorMessage, int nroInt)
        {
            await _citaRepository.UpdateRecordatorioEnviadoStatusAsync(cita, isSuccess, errorMessage, nroInt);
        }
        public async Task UpdateOrdenFinalizadoEnviadoStatusAsync(OrdenDisplayDto orden, bool isSuccess, string errorMessage,int nroInt)
        {
            await _citaRepository.UpdateOrdenFinalizadoEnviadoStatusAsync(orden, isSuccess,errorMessage, nroInt);
        }
       
    }
}
