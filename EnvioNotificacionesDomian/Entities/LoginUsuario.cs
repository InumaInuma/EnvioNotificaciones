using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioNotificacionesDomian.Entities
{
    public class LoginUsuario
    {
        public int CodUsr { get; set; }
        public string NOMUSR { get; set; } = string.Empty;
        public string ClaUsr { get; set; } = string.Empty; // Aquí guardaremos el hash
        public string NOMCOM { get; set; }
        public string Rol { get; set; } = "Usuario"; // Ejemplo: Administrador, Usuario, etc.
        public bool Activo { get; set; } = true;
    }
}
