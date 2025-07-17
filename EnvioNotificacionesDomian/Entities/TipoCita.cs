using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioNotificacionesDomian.Entities
{
    public class TipoCita
    {
        public int TipoCitaID { get; set; }
        public string Descripcion { get; set; } = string.Empty;

        // Propiedad de navegación para las citas de este tipo
        //public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
