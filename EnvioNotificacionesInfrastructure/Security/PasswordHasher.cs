using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesDomian.Security;

namespace EnvioNotificacionesInfrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        // Los métodos ya no son static, son métodos de instancia
        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Ahora llamas a HashPassword en la misma instancia de la clase
            string hashOfInput = HashPassword(password);
            return StringComparer.OrdinalIgnoreCase.Equals(hashOfInput, hashedPassword);
        }
    }
}
