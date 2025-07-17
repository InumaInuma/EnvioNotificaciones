using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesDomian.Enums;

namespace EnvioNotificacionesApplication.DTO
{
    public class OrdenDisplayDto
    {
        public DateTime FecAte { get; set; }
        public int CodEmp { get; set; }
        public int CodSed { get; set; }
        public int CodTcl { get; set; }
        public int NumOrd { get; set; }
        public string NomCom { get; set; }
        public string NomPac { get; set; }
        public int NroDId { get; set; }
        public string DesTCh { get; set; }
        public string EspMed { get; set; }
        public string NroTlf { get; set; }
        public string CorEle { get; set; }
        public bool? IndIWa { get; set; } // Indicador de envío de Finalizado
        public DateTime? FecIWa { get; set; } // Fecha/Hora del último intento de Finalizado
        public string? MsjIRW { get; set; } // Mensaje de error de Finalizado
        public int NroInt { get; set; }

        [NotMapped]
        public EstadoNotificacionEnum EstadoNotificacionFinalizado
        {
            get => IndIWa switch
            {
                true => EstadoNotificacionEnum.ENVIADO,
                false => EstadoNotificacionEnum.FALLIDO,
                null => EstadoNotificacionEnum.PENDIENTE
            };
            set // Añade un setter si quieres poder actualizarlo desde C#
            {
                if (value == EstadoNotificacionEnum.ENVIADO)
                {
                    IndIWa = true; FecIWa = DateTime.Now; MsjIRW = null;
                }
                else if (value == EstadoNotificacionEnum.FALLIDO)
                {
                    IndIWa = false; FecIWa = DateTime.Now; // MsjErrW se establecerá al llamar a MarcarFallida
                }
                else // PENDIENTE o cualquier otro
                {
                    IndIWa = null; FecIWa = null; MsjIRW = null;
                }
            }
        }

        // Y para Finalizado:
        public void MarcarFinalizadoComoEnviado()
        {
            IndIWa = true;
            FecIWa = DateTime.Now;
            MsjIRW = null;
        }

        public void MarcarFinalizadoComoFallida(string? errorMessage)
        {
            IndIWa = false;
            FecIWa = DateTime.Now;
            MsjIRW = errorMessage;
        }


    }
}
