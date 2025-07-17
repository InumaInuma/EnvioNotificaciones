using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioNotificacionesDomian.Entities
{
    public class EstadoNotificacion
    {
        public int EstadoNotificacionID { get; set; }
        public string NombreEstado { get; set; } = string.Empty;

        // Propiedades de navegación para citas (opcional, pero útil para EF Core)
        //public ICollection<Cita> CitasBienvenida { get; set; } = new List<Cita>();
        //public ICollection<Cita> CitasRecordatorio { get; set; } = new List<Cita>();
        //public ICollection<Cita> CitasFinalizado { get; set; } = new List<Cita>();
    }
}
