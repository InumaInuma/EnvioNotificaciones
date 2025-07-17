using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesApplication.DTO;
using EnvioNotificacionesDomian.Entities;
using EnvioNotificacionesInfrastructure.EnvioNotificacionesDomian.Entities;

namespace EnvioNotificacionesDomian.Repositories
{
    public interface ICitaRepository
    {
        Task<Cita?> GetByIdAsync(int id);
        Task<IEnumerable<Cita>> GetAllAsync();
        Task AddAsync(Cita cita);
        Task UpdateAsync(Cita cita);
        Task DeleteAsync(int id);
        Task<int> SaveChangesAsync();
        // --- ¡NUEVO MÉTODO PARA FILTRAR POR ESTADO DE NOTIFICACIÓN! ---
        //Task<IEnumerable<Cita>> GetCitasByEstadoNotificacionAsync(int estadoNotificacionId);

        // --- ¡NUEVO MÉTODO PARA FILTRADO COMBINADO! ---
        // *** MODIFICADO: Ahora acepta fechaInicio y fechaFin ***
        Task<IEnumerable<Cita>> GetCitasFilteredAsync(DateTime? fechaInicio, DateTime? fechaFin, int? estadoNotificacionId);

        // Métodos específicos para las notificaciones del Worker Service
        Task<IEnumerable<Cita>> GetCitasPendientesBienvenidaAsync();
        Task<IEnumerable<Cita>> GetCitasPendientesRecordatorioHoyAsync();
        //Task<IEnumerable<Cita>> GetCitasPendientesFinalizadoAsync();
        Task<IEnumerable<OrdenDisplayDto>> GetObservadorOrdenAsync();
        Task<IEnumerable<OrdenDisplayDto>> GetObservadorOrdenAgrupadosAsync();

        Task UpdateBienvenidaEnviadoStatusAsync(Cita cita, bool isSuccess, string errorMessage, int nroInt);
        Task UpdateRecordatorioEnviadoStatusAsync(Cita cita, bool isSuccess, string errorMessage, int nroInt);
        Task UpdateOrdenFinalizadoEnviadoStatusAsync(OrdenDisplayDto orden, bool isSuccess, string errorMessage,int nroInt); 

     

    }
}
