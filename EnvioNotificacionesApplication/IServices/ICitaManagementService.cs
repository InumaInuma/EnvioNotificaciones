using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesApplication.DTO;
using EnvioNotificacionesDomian.Entities;
using EnvioNotificacionesDomian.Enums;
using EnvioNotificacionesInfrastructure.EnvioNotificacionesDomian.Entities;

namespace EnvioNotificacionesApplication.Services
{
    public interface ICitaManagementService
    {
        Task<IEnumerable<Cita>> GetCitasPendientesBienvenidaAsync();

        Task<IEnumerable<Cita>> GetCitasPendientesRecordatorioHoyAsync();
        //Task<IEnumerable<Cita>> GetCitasPendientesFinalizadoAsync();
        // Puedes añadir más métodos para otras listas o gestión de citas
        //Task<IEnumerable<Cita>> GetAllCitasAsync();

        // Task<Cita?> GetCitaByIdAsync(int citaId);

        // Cambia el tipo de retorno a IEnumerable<CitaDisplayDto>
        Task<IEnumerable<CitaDisplayDto>> GetAllCitasAsync();
        // --- ¡NUEVO MÉTODO PARA FILTRAR POR ESTADO DE NOTIFICACIÓN! ---
        //Task<IEnumerable<CitaDisplayDto>> GetCitasByEstadoNotificacionAsync(int estadoNotificacionId);

        // --- ¡NUEVO MÉTODO PARA FILTRADO COMBINADO! ---
        // *** MODIFICADO: Ahora acepta fechaInicio y fechaFin ***
        Task<IEnumerable<CitaDisplayDto>> GetCitasFilteredAsync(DateTime? fechaInicio, DateTime? fechaFin, int? estadoNotificacionId);

        // ... otros métodos
        // Métodos específicos para las notificaciones del Worker Service
        Task<int> SaveChangesAsync();

        // Puedes agregar métodos para actualizar estados aquí si el UI o otros servicios los necesitan.
        Task UpdateCitaEstadoBienvenidaAsync(int citaId, EstadoNotificacionEnum nuevoEstado);
        //Task UpdateCitaEstadoRecordatorioAsync(int citaId, EstadoNotificacionEnum nuevoEstado);
        //Task UpdateCitaEstadoFinalizadoAsync(int citaId, EstadoNotificacionEnum nuevoEstado);
        //Task UpdateCitaEstadoGeneralAsync(int citaId, EstadoCitaEnum nuevoEstado);   
        Task<IEnumerable<OrdenDisplayDto>> GetObservadorOrdenAsync();
        Task<IEnumerable<OrdenDisplayDto>> GetObservadorOrdenAgrupadosAsync();
        Task UpdateBienvenidaEnviadoStatusAsync(Cita cita, bool isSuccess, string errorMessage, int nroInt);
        Task UpdateRecordatorioEnviadoStatusAsync(Cita cita, bool isSuccess, string errorMessage, int nroInt);
        Task UpdateOrdenFinalizadoEnviadoStatusAsync(OrdenDisplayDto orden, bool isSuccess, string errorMessage,int nroInt);
       










    }
}
