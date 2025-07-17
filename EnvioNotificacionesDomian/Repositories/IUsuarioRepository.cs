using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesDomian.Entities;

namespace EnvioNotificacionesDomian.Repositories
{
    public interface IUsuarioRepository
    {
        Task<LoginUsuario> GetByUsernameAsync(string username, string password); // Puede ser null si no lo encuentra
        // Task AddAsync(Usuario user); // Si necesitaras registrar usuarios
        // Task UpdateAsync(Usuario user); // Si necesitaras actualizar
        Task SaveChangesAsync(); // Para guardar cambios en la base de datos
    }
}
