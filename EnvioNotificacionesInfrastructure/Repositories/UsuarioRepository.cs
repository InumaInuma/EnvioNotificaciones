using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesDomian.Entities;
using EnvioNotificacionesDomian.Repositories;
using EnvioNotificacionesInfrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Seguridad;

namespace EnvioNotificacionesInfrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        Cifrado _cifrado = new Cifrado();
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Cambiado el tipo de retorno a Task<LoginResult> o Task<Usuario>
        public async Task<LoginUsuario> GetByUsernameAsync(string username, string password) // 'contraseña' cambiado a 'password' por convención
        {
            try
            {
                var contra = _cifrado.Encrit(password);
                // No es estrictamente necesario verificar el estado de la conexión aquí
                // DbContext generalmente maneja esto automáticamente al ejecutar un comando.
                // Sin embargo, si lo quieres mantener, está bien.
                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    await _context.Database.OpenConnectionAsync();
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    // 1. Nombre del procedimiento almacenado
                    command.CommandText = "usp_ValidarLogin";
                    command.CommandType = CommandType.StoredProcedure;

                    // 2. Nombres y tipos de parámetros correctos
                    // @NomUsu VARCHAR(50)
                    command.Parameters.Add(new SqlParameter("@NomUsu", SqlDbType.VarChar, 50) { Value = (object)username ?? DBNull.Value });
                    // @ClauSr VARCHAR(100)
                    command.Parameters.Add(new SqlParameter("@ClauSr", SqlDbType.VarChar, 100) { Value = (object)contra ?? DBNull.Value });

                    // 3. Usar ExecuteReaderAsync para leer los resultados
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read()) // Si hay un registro (login exitoso)
                        {
                            return new LoginUsuario
                            {
                                CodUsr = reader.GetInt32(reader.GetOrdinal("CodUsr")), // Obtener por nombre de columna
                                NOMCOM = reader.GetString(reader.GetOrdinal("NOMCOM"))
                            };
                        }
                        else
                        {
                            // Si no se encuentra el usuario/contraseña, retornar null
                            return null;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Es buena práctica capturar excepciones SQL específicas para logging o manejo detallado
                throw new Exception("Error de base de datos en ValidarLogin: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción general
                throw new Exception("Error inesperado en ValidarLogin: " + ex.Message, ex);
            }
            finally
            {
                // Asegurarse de cerrar la conexión si la abrimos manualmente
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
