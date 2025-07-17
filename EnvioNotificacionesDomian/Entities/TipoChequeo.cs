using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EnvioNotificacionesDomian.Entities;

namespace EnvioNotificacionesInfrastructure.EnvioNotificacionesDomian.Entities;

[Table("TipoChequeo")]
public partial class TipoChequeo
{

    
    public int CodTCh { get; set; }
    public string DesTCh { get; set; } = null!; // O string? si es nullable en DB

    //public virtual ICollection<Cita> Cita { get; set; } // Colección de citas relacionadas
}
