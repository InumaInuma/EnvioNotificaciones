using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioNotificacionesApplication.Services
{
    public interface IWhatsAppService
    {
        // Contrato para enviar el mensaje de bienvenida
        Task<bool> EnviarBienvenidaAsync(string numeroDestino, string nombrePaciente, DateTime fechaCita, string tipoExamen);

        // Contrato para enviar el mensaje de recordatorio
        Task<bool> EnviarRecordatorioAsync(string numeroDestino, string nombrePaciente, DateTime fechaCita, string tipoExamen);

        // Contrato para enviar el mensaje de finalizado/interconsulta
        Task<bool> EnviarFinalizadoAsync(string numeroDestino, string nombrePaciente, DateTime fechaCita, string empresa, string interconsultas, string correo);
    }
}
