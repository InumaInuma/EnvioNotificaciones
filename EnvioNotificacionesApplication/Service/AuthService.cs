using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesApplication.Services;
using EnvioNotificacionesDomian.Entities;
using EnvioNotificacionesDomian.Repositories;
using EnvioNotificacionesDomian.Security;


namespace EnvioNotificacionesApplication.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher; // <-- ¡Inyectamos la interfaz!

        // El constructor ahora recibe IPasswordHasher
        public AuthService(IUsuarioRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher; // <-- Asignamos la instancia inyectada
        }

        public async Task<LoginUsuario> GetByUsernameAsync(string username, string password)
        {
           

            // Usamos la instancia inyectada de IPasswordHasher
            var user = await _userRepository.GetByUsernameAsync(username , password); // <-- ¡Cambio clave aquí!
            return user; 
        } 
    }
}
