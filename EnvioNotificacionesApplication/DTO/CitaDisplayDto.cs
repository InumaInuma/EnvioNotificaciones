using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioNotificacionesApplication.DTO
{
    public class CitaDisplayDto
    {
        public int CitaID { get; set; } // Mapea a CodCit
        public DateTime? FechaHoraProgramada { get; set; } // Mapea a FecCit
        public string? Observaciones { get; set; } // Mapea a Observ
        public string? Empresa { get; set; } // Mapea a CodEmp (tendrás que cargar el nombre de la empresa si es un FK)
        public string? Interconsultas { get; set; } // Mapea a Interconsultas

        // Campos del paciente ahora directos de Cita
        public string PacienteNombreCompleto { get; set; } = string.Empty; // Combina Nombre, ApePat, ApeMat
        public string PacienteTelefono { get; set; } = string.Empty; // Mapea a NroCel
        public string PacienteCorreoElectronico { get; set; } = string.Empty; // Mapea a CorElec

        public string TipoCitaDescripcion { get; set; } = string.Empty; // Mapea a CodTDo (tendrás que cargar la descripción de Constante)

        // Estados derivados de las nuevas columnas BIT
        public string EstadoBienvenidaNombre { get; set; } = string.Empty;
        public string EstadoRecordatorioNombre { get; set; } = string.Empty;
        public string EstadoFinalizadoNombre { get; set; } = string.Empty;

        // Nuevos campos de mensaje de error
        public string? MensajeErrorBienvenida { get; set; } // Mapea a MsjErrW
        public string? MensajeErrorRecordatorio { get; set; } // Mapea a MsjERW
        public string? MensajeErrorFinalizado { get; set; } // Mapea a MsjIRW

        // Fechas de último intento (si quieres mostrarlas en UI)
        public DateTime? UltimoIntentoBienvenida { get; set; } // Mapea a FecEWa
        public DateTime? UltimoIntentoRecordatorio { get; set; } // Mapea a FecRWa
        public DateTime? UltimoIntentoFinalizado { get; set; } // Mapea a FecIWa

        // El EstadoCita general (si lo sigues usando, necesitará ser derivado de otros campos si no hay uno en la DB)
        public string EstadoCitaGeneral { get; set; } = string.Empty;
    }
}
