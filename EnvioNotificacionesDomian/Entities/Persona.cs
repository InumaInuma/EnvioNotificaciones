using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioNotificacionesDomian.Entities
{
    public class Persona
    {
        public int PacienteID { get; set; }
        public string DNI { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? CorreoElectronico { get; set; }
        public char? Genero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public bool Activo { get; set; }

        // Propiedad de navegación para las citas de la persona
        //public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
