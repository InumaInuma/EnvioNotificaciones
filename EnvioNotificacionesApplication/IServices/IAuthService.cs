using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesDomian.Entities;

namespace EnvioNotificacionesApplication.Services
{
    public interface IAuthService
    {
        // Retorna LoginUsuario si el login es exitoso, null si falla.
        Task<LoginUsuario> GetByUsernameAsync(string username, string password);
    }
}
