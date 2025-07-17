using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesApplication.DTO;
using EnvioNotificacionesDomian.Entities;
using EnvioNotificacionesDomian.Enums;
using EnvioNotificacionesDomian.Repositories;
using EnvioNotificacionesInfrastructure.Data;
using EnvioNotificacionesInfrastructure.EnvioNotificacionesDomian.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // ¡Añadimos esto para el logging!

namespace EnvioNotificacionesInfrastructure.Repositories
{
    public class CitaRepository : ICitaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CitaRepository> _logger; // ¡Añadimos un logger para depuración y monitoreo!

        public CitaRepository(ApplicationDbContext context, ILogger<CitaRepository> logger)
        {
            _context = context;
            _logger = logger; // Asignamos el logger
        }

        public async Task AddAsync(Cita cita)
        {
            await _context.Cita.AddAsync(cita);
        }

        public async Task DeleteAsync(int id)
        {
            var cita = await _context.Cita.FindAsync(id);
            if (cita != null)
            {
                _context.Cita.Remove(cita);
            }
        }

        public async Task<IEnumerable<Cita>> GetAllAsync()
        {
            // *** ¡LA CLAVE ESTÁ AQUÍ: USAR .Include()! ***
            //// Esto le dice a Entity Framework Core que cargue la entidad relacionada
            //// TipoCita y Paciente junto con cada Cita.
            //return await _context.Citas
            //                     .Include(c => c.Paciente) // Para cargar el objeto Paciente relacionado
            //                     .Include(c => c.TipoCita) // Para cargar el objeto TipoCita relacionado
            //                     .Include(c => c.EstadoBienvenida) // Para los nombres de estado
            //                     .Include(c => c.EstadoRecordatorio)
            //                     .Include(c => c.EstadoFinalizado)
            //                     .AsNoTracking() // Con AsNoTracking hago que consulte cada ves ala DB y actualize el DataGrid (Solo usar para lectura No para consultad que lleguen a modificar insertar datos)
            //                     .ToListAsync();
            return await _context.Cita
                                .Include(c => c.TipoChequeo) // ¡Ajusta el nombre de la navegación generada!
                                .AsNoTracking()
                                .ToListAsync();
        }

        //// --- ¡IMPLEMENTACIÓN DEL NUEVO MÉTODO! ---
        //public async Task<IEnumerable<Cita>> GetCitasByEstadoNotificacionAsync(int estadoNotificacionId)
        //{
        //    return await _context.Citas
        //                         .Include(c => c.Paciente)
        //                         .Include(c => c.TipoCita)
        //                         .Include(c => c.EstadoBienvenida)
        //                         .Include(c => c.EstadoRecordatorio)
        //                         .Include(c => c.EstadoFinalizado)
        //                         // Aquí la lógica de tu filtro
        //                         // Necesitas decidir qué "estado de notificación" filtrar.
        //                         // Por ejemplo, si quieres filtrar por EstadoBienvenidaID:
        //                         //.Where(c => c.EstadoNotificacionBienvenidaID == estadoNotificacionId ||
        //                         //            c.EstadoNotificacionRecordatorioID == estadoNotificacionId ||
        //                         //            c.EstadoNotificacionFinalizadoID == estadoNotificacionId)
        //                          // O, si prefieres solo filtrar por EstadoBienvenidaID por ahora
        //                          .Where(c => c.EstadoNotificacionBienvenidaID == estadoNotificacionId)
        //                         .AsNoTracking() // Aquí sí queremos que sea lectura fresca para la UI
        //                         .ToListAsync();
        //}

        // --- ¡IMPLEMENTACIÓN DEL NUEVO MÉTODO DE FILTRADO COMBINADO! ---
        public async Task<IEnumerable<Cita>> GetCitasFilteredAsync(DateTime? fechaInicio, DateTime? fechaFin, int? estadoNotificacionId)
        {

            var results = new List<Cita>();

            try
            {
                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    await _context.Database.OpenConnectionAsync();
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SP_GetCitasFiltered"; // ¡Nombre del SP!
                    command.CommandType = CommandType.StoredProcedure;
                    // *** Añade los parámetros de fecha de inicio y fin ***
                    command.Parameters.Add(new SqlParameter("@FechaInicio", SqlDbType.Date)
                    {
                        Value = fechaInicio.HasValue ? fechaInicio.Value.Date : DBNull.Value
                    });

                    command.Parameters.Add(new SqlParameter("@FechaFin", SqlDbType.Date)
                    {
                        Value = fechaFin.HasValue ? fechaFin.Value.Date : DBNull.Value
                    });

                    command.Parameters.Add(new SqlParameter("@EstadoNotificacionId", SqlDbType.Int)
                    {
                        Value = estadoNotificacionId.HasValue ? (object)estadoNotificacionId.Value : DBNull.Value
                    });
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                           

                            var cita = new Cita
                            {
                                CodCit = reader.GetInt32(reader.GetOrdinal("CodCit")),
                                FecCit = reader.GetDateTime(reader.GetOrdinal("FecCit")),
                                Observ = reader.IsDBNull(reader.GetOrdinal("Observ")) ? null : reader.GetString(reader.GetOrdinal("Observ")),
                                NroCel = reader.IsDBNull(reader.GetOrdinal("NroCel")) ? null : reader.GetString(reader.GetOrdinal("NroCel")),
                                CorElec = reader.IsDBNull(reader.GetOrdinal("CorElec")) ? null : reader.GetString(reader.GetOrdinal("CorElec")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                                ApePat = reader.IsDBNull(reader.GetOrdinal("ApePat")) ? null : reader.GetString(reader.GetOrdinal("ApePat")),
                                ApeMat = reader.IsDBNull(reader.GetOrdinal("ApeMat")) ? null : reader.GetString(reader.GetOrdinal("ApeMat")),

                                IndEWa = reader.IsDBNull(reader.GetOrdinal("IndEWa")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IndEWa")),
                                FecEWa = reader.IsDBNull(reader.GetOrdinal("FecEWa")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FecEWa")),
                                MsjErrW = reader.IsDBNull(reader.GetOrdinal("MsjErrW")) ? null : reader.GetString(reader.GetOrdinal("MsjErrW")),
                                IndRWa = reader.IsDBNull(reader.GetOrdinal("IndRWa")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IndRWa")),
                                // Confirmado que IndRWa es Int32 en producción, así que lo leemos como INT y convertimos
                                //IndRWa = reader.IsDBNull(reader.GetOrdinal("IndRWa")) ? (bool?)null : (reader.GetInt32(reader.GetOrdinal("IndRWa")) == 1),
                                FecRWa = reader.IsDBNull(reader.GetOrdinal("FecRWa")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FecRWa")),
                                MsjERW = reader.IsDBNull(reader.GetOrdinal("MsjERW")) ? null : reader.GetString(reader.GetOrdinal("MsjERW")),

                                DesTCh = reader.IsDBNull(reader.GetOrdinal("DesTCh")) ? null : reader.GetString(reader.GetOrdinal("DesTCh")),
                                NomCom = reader.IsDBNull(reader.GetOrdinal("NomCom")) ? null : reader.GetString(reader.GetOrdinal("NomCom")),
                               


                               
                            };


                            results.Add(cita);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Error de SQL al ejecutar SP_GetCitasFiltered. Mensaje: {Message}", sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al ejecutar SP_GetCitasFiltered. Mensaje: {Message}", ex.Message);
                throw;
            }
            finally
            {
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }
            return results;
        
        
        }

        public async Task<Cita?> GetByIdAsync(int id)
        {
            //return await _context.Cita
            //                     .Include(c => c.Paciente)
            //                     .Include(c => c.TipoCita)
            //                     .FirstOrDefaultAsync(c => c.CitaID == id);
            return await _context.Cita
                                .Include(c => c.TipoChequeo) // ¡Ajusta el nombre de la navegación generada!
                                                                  // ... otros Includes si Scaffold generó más navegaciones (Cliente, Constante)
                                .FirstOrDefaultAsync(c => c.CodCit == id); // Usa CodCit como PK
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cita cita)
        {
            _context.Cita.Update(cita);
        }

        // --- Métodos específicos para el Worker Service ---
        // Adapta GetCitasPendientesBienvenidaAsync, GetCitasPendientesRecordatorioHoyAsync, GetCitasPendientesFinalizadoAsync
        // para usar los nuevos campos (IndEWa, FecEWa) y los Includes correctos.
        public async Task<IEnumerable<Cita>> GetCitasPendientesBienvenidaAsync()
        {
            //var today = DateTime.Today; // Solo la fecha, sin la hora
            //return await _context.Cita
            //                     .Include(c => c.TipoChequeo)
            //                     .Where(c => (c.IndEWa == null || c.IndEWa == false) &&
            //                     (c.NroInt <= 3) &&
            //                     //c.MsjErrW == null &&
            //                     (c.FecCit.Date == today.Date || c.FecCit.Date > today.Date) ) // Citas programadas para HOY o mayor a hoy
            //                     .ToListAsync(); // Sin AsNoTracking si se van a modificar
            var results = new List<Cita>();

            try // ¡Iniciamos el bloque try para capturar excepciones!
            {
                // Asegúrate de que la conexión esté abierta.
                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    await _context.Database.OpenConnectionAsync();
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SP_CheckAndSendWelcomeMessages";
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new Cita
                            {
                                // --- ¡Así es como controlas los NULLs, mi rey! ---
                                
                                CodCit = reader.IsDBNull(reader.GetOrdinal("CodCit")) ? 1 : reader.GetInt32(reader.GetOrdinal("CodCit")),
                                NroTlf = reader.IsDBNull(reader.GetOrdinal("NroTlf")) ? null : reader.GetString(reader.GetOrdinal("NroTlf")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("NombreCompleto")) ? null : reader.GetString(reader.GetOrdinal("NombreCompleto")),
                                FecCit = reader.IsDBNull(reader.GetOrdinal("FecCit")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FecCit")),
                                DesTCh = reader.IsDBNull(reader.GetOrdinal("DesTCh")) ? null : reader.GetString(reader.GetOrdinal("DesTCh")),
                                NroInt = reader.IsDBNull(reader.GetOrdinal("NroInt")) ? 1 : reader.GetInt32(reader.GetOrdinal("NroInt")),
                                // 1. Para string (VARCHAR, NVARCHAR, TEXT, etc.):
                                // Un string en C# ya es anulable, así que si es DBNull, puedes asignarle 'null'.
                                //NroDId = reader.IsDBNull(reader.GetOrdinal("NroDId")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroDId")),

                                //NombreEmpresa = reader.IsDBNull(reader.GetOrdinal("NombreEmpresa")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEmpresa")),
                                //NomEmp = reader.IsDBNull(reader.GetOrdinal("NombreCompleto")) ? null : reader.GetString(reader.GetOrdinal("NomEmp")),
                                //NomPac = reader.IsDBNull(reader.GetOrdinal("NomPac")) ? null : reader.GetString(reader.GetOrdinal("NomPac")),
                                //NroDId = reader.IsDBNull(reader.GetOrdinal("NroDId")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroDId")),
                                //DesTCh = reader.IsDBNull(reader.GetOrdinal("DesTCh")) ? null : reader.GetString(reader.GetOrdinal("DesTCh")),
                                // 2. Para int? (INT, SMALLINT, BIGINT, etc., que puedan ser NULL en DB):
                                // Usamos 'int?' (tipo de valor anulable) en C#.
                                //CantidadChequeos = reader.IsDBNull(reader.GetOrdinal("CantidadChequeos")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CantidadChequeos")),

                                //// 3. Para DateTime? (DATETIME, DATE, DATETIME2, etc., que puedan ser NULL en DB):
                                //// Usamos 'DateTime?' (tipo de valor anulable) en C#.
                                ////FecAte = reader.IsDBNull(reader.GetOrdinal("FecAte")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FecAte")),

                                //// 4. Para bool? (BIT que pueda ser NULL en DB):
                                //// Usamos 'bool?' (tipo de valor anulable) en C#.
                                //Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Activo")),

                                //// 5. Para decimal? (DECIMAL, NUMERIC, MONEY, etc., que puedan ser NULL en DB):
                                //// Usamos 'decimal?' (tipo de valor anulable) en C#.
                                //MontoTotal = reader.IsDBNull(reader.GetOrdinal("MontoTotal")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("MontoTotal"));


                                //FecAte = reader.GetDateTime(reader.GetOrdinal("FecAte")),
                                //NumOrd = reader.GetInt32(reader.GetOrdinal("NumOrd")),
                                //NombreEmpresa = reader.GetString(reader.GetOrdinal("NombreEmpresa")),
                                //NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto")),
                                //NroDId = reader.GetInt32(reader.GetOrdinal("NroDId")),
                                //DesTCh = reader.GetString(reader.GetOrdinal("DesTCh"))
                            });
                        }
                    }
                }
            }
            catch (SqlException sqlEx) // Captura excepciones específicas de SQL Server
            {
                _logger.LogError(sqlEx, "Error de SQL al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", sqlEx.Message);
                // Aquí puedes decidir qué hacer:
                // 1. Relanzar una excepción personalizada: throw new RepositorioException("No se pudo obtener la orden.", sqlEx);
                // 2. Devolver una lista vacía: return new List<ObservadorOrdenDto>();
                // 3. Manejar el error de alguna otra manera específica.
                throw; // Por ahora, relanzamos la excepción para que sea manejada por la capa superior
            }
            catch (Exception ex) // Captura cualquier otra excepción genérica
            {
                _logger.LogError(ex, "Error inesperado al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", ex.Message);
                throw; // Relanzamos para que se maneje en la capa superior
            }
            finally
            {
                // Opcional: Si quieres asegurar el cierre de la conexión, aunque EF Core lo gestiona
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

            return results;

        }

        public async Task<IEnumerable<Cita>> GetCitasPendientesRecordatorioHoyAsync()
        {
            //var today = DateTime.Today; // Solo la fecha, sin la hora

            //return await _context.Cita
            //    .Include(c => c.TipoChequeo)
            //    .Where(c => (c.IndEWa == true) &&
            //                (c.IndRWa == null) &&

            ////c.FecCit > DateTime.Now.AddHours(-2) && // Si el recordatorio es hasta 2 horas después de la cita
            ////c.FecCit < DateTime.Now.AddHours(2)) // Y 2 horas antes
            //                (c.FecCit.Date == today.Date)) // Citas programadas para HOY

            //    //.AsNoTracking()
            //    .ToListAsync();
            var results = new List<Cita>();

            try // ¡Iniciamos el bloque try para capturar excepciones!
            {
                // Asegúrate de que la conexión esté abierta.
                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    await _context.Database.OpenConnectionAsync();
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SP_CheckAndSendReminderMessages";
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new Cita
                            {
                                // --- ¡Así es como controlas los NULLs, mi rey! ---

                                CodCit = reader.IsDBNull(reader.GetOrdinal("CodCit")) ? 1 : reader.GetInt32(reader.GetOrdinal("CodCit")),
                                NroTlf = reader.IsDBNull(reader.GetOrdinal("NroTlf")) ? null : reader.GetString(reader.GetOrdinal("NroTlf")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("NombreCompleto")) ? null : reader.GetString(reader.GetOrdinal("NombreCompleto")),
                                FecCit = reader.IsDBNull(reader.GetOrdinal("FecCit")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FecCit")),
                                DesTCh = reader.IsDBNull(reader.GetOrdinal("DesTCh")) ? null : reader.GetString(reader.GetOrdinal("DesTCh")),
                                NroInt = reader.IsDBNull(reader.GetOrdinal("NroInt")) ? 1 : reader.GetInt32(reader.GetOrdinal("NroInt")),
                                // 1. Para string (VARCHAR, NVARCHAR, TEXT, etc.):
                                // Un string en C# ya es anulable, así que si es DBNull, puedes asignarle 'null'.
                                //NroDId = reader.IsDBNull(reader.GetOrdinal("NroDId")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroDId")),

                                //NombreEmpresa = reader.IsDBNull(reader.GetOrdinal("NombreEmpresa")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEmpresa")),
                                //NomEmp = reader.IsDBNull(reader.GetOrdinal("NombreCompleto")) ? null : reader.GetString(reader.GetOrdinal("NomEmp")),
                                //NomPac = reader.IsDBNull(reader.GetOrdinal("NomPac")) ? null : reader.GetString(reader.GetOrdinal("NomPac")),
                                //NroDId = reader.IsDBNull(reader.GetOrdinal("NroDId")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroDId")),
                                //DesTCh = reader.IsDBNull(reader.GetOrdinal("DesTCh")) ? null : reader.GetString(reader.GetOrdinal("DesTCh")),
                                // 2. Para int? (INT, SMALLINT, BIGINT, etc., que puedan ser NULL en DB):
                                // Usamos 'int?' (tipo de valor anulable) en C#.
                                //CantidadChequeos = reader.IsDBNull(reader.GetOrdinal("CantidadChequeos")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CantidadChequeos")),

                                //// 3. Para DateTime? (DATETIME, DATE, DATETIME2, etc., que puedan ser NULL en DB):
                                //// Usamos 'DateTime?' (tipo de valor anulable) en C#.
                                ////FecAte = reader.IsDBNull(reader.GetOrdinal("FecAte")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FecAte")),

                                //// 4. Para bool? (BIT que pueda ser NULL en DB):
                                //// Usamos 'bool?' (tipo de valor anulable) en C#.
                                //Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Activo")),

                                //// 5. Para decimal? (DECIMAL, NUMERIC, MONEY, etc., que puedan ser NULL en DB):
                                //// Usamos 'decimal?' (tipo de valor anulable) en C#.
                                //MontoTotal = reader.IsDBNull(reader.GetOrdinal("MontoTotal")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("MontoTotal"));


                                //FecAte = reader.GetDateTime(reader.GetOrdinal("FecAte")),
                                //NumOrd = reader.GetInt32(reader.GetOrdinal("NumOrd")),
                                //NombreEmpresa = reader.GetString(reader.GetOrdinal("NombreEmpresa")),
                                //NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto")),
                                //NroDId = reader.GetInt32(reader.GetOrdinal("NroDId")),
                                //DesTCh = reader.GetString(reader.GetOrdinal("DesTCh"))
                            });
                        }
                    }
                }
            }
            catch (SqlException sqlEx) // Captura excepciones específicas de SQL Server
            {
                _logger.LogError(sqlEx, "Error de SQL al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", sqlEx.Message);
                // Aquí puedes decidir qué hacer:
                // 1. Relanzar una excepción personalizada: throw new RepositorioException("No se pudo obtener la orden.", sqlEx);
                // 2. Devolver una lista vacía: return new List<ObservadorOrdenDto>();
                // 3. Manejar el error de alguna otra manera específica.
                throw; // Por ahora, relanzamos la excepción para que sea manejada por la capa superior
            }
            catch (Exception ex) // Captura cualquier otra excepción genérica
            {
                _logger.LogError(ex, "Error inesperado al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", ex.Message);
                throw; // Relanzamos para que se maneje en la capa superior
            }
            finally
            {
                // Opcional: Si quieres asegurar el cierre de la conexión, aunque EF Core lo gestiona
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

            return results;
        }

        public async Task<IEnumerable<OrdenDisplayDto>> GetObservadorOrdenAgrupadosAsync()
        {
            var results = new List<OrdenDisplayDto>();

            try // ¡Iniciamos el bloque try para capturar excepciones!
            {
                // Asegúrate de que la conexión esté abierta.
                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    await _context.Database.OpenConnectionAsync();
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SP_ObtenerOrdenesFichasAgrupadas";
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new OrdenDisplayDto
                            {
                                EspMed = reader.IsDBNull(reader.GetOrdinal("EspMed")) ? null : reader.GetString(reader.GetOrdinal("EspMed")),
                                // --- ¡Así es como controlas los NULLs, mi rey! ---
                                CodEmp = reader.IsDBNull(reader.GetOrdinal("CodEmp")) ? 0 : reader.GetInt32(reader.GetOrdinal("CodEmp")),
                                CodSed = reader.IsDBNull(reader.GetOrdinal("CodSed")) ? 0 : reader.GetInt32(reader.GetOrdinal("CodSed")),
                                CodTcl = reader.IsDBNull(reader.GetOrdinal("CodTcl")) ? 0 : reader.GetInt32(reader.GetOrdinal("CodTcl")),
                                NumOrd = reader.IsDBNull(reader.GetOrdinal("NumOrd")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumOrd")),
                                FecAte = reader.IsDBNull(reader.GetOrdinal("FecAte")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FecAte")),
                                NroTlf = reader.IsDBNull(reader.GetOrdinal("NroTlf")) ? null : reader.GetString(reader.GetOrdinal("NroTlf")),
                                CorEle = reader.IsDBNull(reader.GetOrdinal("CorEle")) ? null : reader.GetString(reader.GetOrdinal("CorEle")),
                                // 1. Para string (VARCHAR, NVARCHAR, TEXT, etc.):
                                // Un string en C# ya es anulable, así que si es DBNull, puedes asignarle 'null'.
                                //NroDId = reader.IsDBNull(reader.GetOrdinal("NroDId")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroDId")),

                                //NombreEmpresa = reader.IsDBNull(reader.GetOrdinal("NombreEmpresa")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEmpresa")),
                                NomCom = reader.IsDBNull(reader.GetOrdinal("NomCom")) ? null : reader.GetString(reader.GetOrdinal("NomCom")),
                                NomPac = reader.IsDBNull(reader.GetOrdinal("NomPac")) ? null : reader.GetString(reader.GetOrdinal("NomPac")),
                                DesTCh = reader.IsDBNull(reader.GetOrdinal("DesTCh")) ? null : reader.GetString(reader.GetOrdinal("DesTCh")),
                                IndIWa = reader.IsDBNull(reader.GetOrdinal("IndIWa")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("IndIWa")),
                                FecIWa = reader.IsDBNull(reader.GetOrdinal("FecIWa")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FecIWa")),
                                MsjIRW = reader.IsDBNull(reader.GetOrdinal("MsjIRW")) ? null : reader.GetString(reader.GetOrdinal("MsjIRW")),
                                // 2. Para int? (INT, SMALLINT, BIGINT, etc., que puedan ser NULL en DB):
                                // Usamos 'int?' (tipo de valor anulable) en C#.
                                //CantidadChequeos = reader.IsDBNull(reader.GetOrdinal("CantidadChequeos")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CantidadChequeos")),

                                //// 3. Para DateTime? (DATETIME, DATE, DATETIME2, etc., que puedan ser NULL en DB):
                                //// Usamos 'DateTime?' (tipo de valor anulable) en C#.
                                ////FecAte = reader.IsDBNull(reader.GetOrdinal("FecAte")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FecAte")),

                                //// 4. Para bool? (BIT que pueda ser NULL en DB):
                                //// Usamos 'bool?' (tipo de valor anulable) en C#.
                                //Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Activo")),

                                //// 5. Para decimal? (DECIMAL, NUMERIC, MONEY, etc., que puedan ser NULL en DB):
                                //// Usamos 'decimal?' (tipo de valor anulable) en C#.
                                //MontoTotal = reader.IsDBNull(reader.GetOrdinal("MontoTotal")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("MontoTotal"));

                            });
                        }
                    }
                }
            }
            catch (SqlException sqlEx) // Captura excepciones específicas de SQL Server
            {
                _logger.LogError(sqlEx, "Error de SQL al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", sqlEx.Message);
                // Aquí puedes decidir qué hacer:
                // 1. Relanzar una excepción personalizada: throw new RepositorioException("No se pudo obtener la orden.", sqlEx);
                // 2. Devolver una lista vacía: return new List<ObservadorOrdenDto>();
                // 3. Manejar el error de alguna otra manera específica.
                throw; // Por ahora, relanzamos la excepción para que sea manejada por la capa superior
            }
            catch (Exception ex) // Captura cualquier otra excepción genérica
            {
                _logger.LogError(ex, "Error inesperado al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", ex.Message);
                throw; // Relanzamos para que se maneje en la capa superior
            }
            finally
            {
                // Opcional: Si quieres asegurar el cierre de la conexión, aunque EF Core lo gestiona
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

            return results;
        }

        //public async Task<IEnumerable<Cita>> GetCitasPendientesFinalizadoAsync()
        //{
        //    var today = DateTime.Today; // Solo la fecha, sin la hora
        //    return await _context.Cita
        //        //.Include(c => c.Paciente)
        //        //.Include(c => c.TipoCita)
        //        .Include(c => c.TipoChequeo)
        //        .Where(c => (c.IndEWa == true) &&
        //                    (c.IndRWa == true) &&
        //                    //c.IndIWa == null )&& 
        //                    (c.FecCit.Date == today.Date))
        //        //c.EstadoNotificacionFinalizadoID == (int)EstadoNotificacionEnum.PENDIENTE &&
        //        //c.EstadoCita == EstadoCitaEnum.ATENDIDA && // Solo citas que ya han sido ATENDIDAS
        //        //c.NumeroIntentosFinalizado < 3)
        //        //c.FecCit.Date == DateTime.Today.AddDays(-1).Date) // Para citas atendidas ayer
        //        //.AsNoTracking()
        //        .ToListAsync();
        //}

        // --- Nuevo método para llamar a SP_OBSERVADOR_ORDEN ---
        public async Task<IEnumerable<OrdenDisplayDto>> GetObservadorOrdenAsync()
        {
            var results = new List<OrdenDisplayDto>();

            try // ¡Iniciamos el bloque try para capturar excepciones!
            {
                // Asegúrate de que la conexión esté abierta.
                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    await _context.Database.OpenConnectionAsync();
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SP_OBSERVADOR_ORDEN";
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            results.Add(new OrdenDisplayDto
                            {
                                // --- ¡Así es como controlas los NULLs, mi rey! ---
                                FecAte = reader.IsDBNull(reader.GetOrdinal("FecAte")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FecAte")),
                                NumOrd = reader.IsDBNull(reader.GetOrdinal("NumOrd")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumOrd")),

                                // 1. Para string (VARCHAR, NVARCHAR, TEXT, etc.):
                                // Un string en C# ya es anulable, así que si es DBNull, puedes asignarle 'null'.
                                //NroDId = reader.IsDBNull(reader.GetOrdinal("NroDId")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroDId")),

                                //NombreEmpresa = reader.IsDBNull(reader.GetOrdinal("NombreEmpresa")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEmpresa")),
                                NomCom = reader.IsDBNull(reader.GetOrdinal("NomCom")) ? null : reader.GetString(reader.GetOrdinal("NomCom")),
                                NomPac = reader.IsDBNull(reader.GetOrdinal("NomPac")) ? null : reader.GetString(reader.GetOrdinal("NomPac")),
                                NroDId = reader.IsDBNull(reader.GetOrdinal("NroDId")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroDId")),
                                DesTCh = reader.IsDBNull(reader.GetOrdinal("DesTCh")) ? null : reader.GetString(reader.GetOrdinal("DesTCh")),
                                // 2. Para int? (INT, SMALLINT, BIGINT, etc., que puedan ser NULL en DB):
                                // Usamos 'int?' (tipo de valor anulable) en C#.
                                //CantidadChequeos = reader.IsDBNull(reader.GetOrdinal("CantidadChequeos")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CantidadChequeos")),

                                //// 3. Para DateTime? (DATETIME, DATE, DATETIME2, etc., que puedan ser NULL en DB):
                                //// Usamos 'DateTime?' (tipo de valor anulable) en C#.
                                ////FecAte = reader.IsDBNull(reader.GetOrdinal("FecAte")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FecAte")),

                                //// 4. Para bool? (BIT que pueda ser NULL en DB):
                                //// Usamos 'bool?' (tipo de valor anulable) en C#.
                                //Activo = reader.IsDBNull(reader.GetOrdinal("Activo")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Activo")),

                                //// 5. Para decimal? (DECIMAL, NUMERIC, MONEY, etc., que puedan ser NULL en DB):
                                //// Usamos 'decimal?' (tipo de valor anulable) en C#.
                                //MontoTotal = reader.IsDBNull(reader.GetOrdinal("MontoTotal")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("MontoTotal"));


                            });
                        }
                    }
                }
            }
            catch (SqlException sqlEx) // Captura excepciones específicas de SQL Server
            {
                _logger.LogError(sqlEx, "Error de SQL al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", sqlEx.Message);
                // Aquí puedes decidir qué hacer:
                // 1. Relanzar una excepción personalizada: throw new RepositorioException("No se pudo obtener la orden.", sqlEx);
                // 2. Devolver una lista vacía: return new List<ObservadorOrdenDto>();
                // 3. Manejar el error de alguna otra manera específica.
                throw; // Por ahora, relanzamos la excepción para que sea manejada por la capa superior
            }
            catch (Exception ex) // Captura cualquier otra excepción genérica
            {
                _logger.LogError(ex, "Error inesperado al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", ex.Message);
                throw; // Relanzamos para que se maneje en la capa superior
            }
            finally
            {
                // Opcional: Si quieres asegurar el cierre de la conexión, aunque EF Core lo gestiona
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

            return results;
        }

        // --- Nuevo método para llamar a SP_OBSERVADOR_ORDEN ---
       

        public async Task UpdateBienvenidaEnviadoStatusAsync(Cita cita, bool isSuccess, string errorMessage, int nroInt)
        {
            try
            {
                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    await _context.Database.OpenConnectionAsync();
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SP_MarcarEnviado_Bienvenida";
                    command.CommandType = CommandType.StoredProcedure;

                   
                    command.Parameters.Add(new SqlParameter("@CodCit", SqlDbType.Int) { Value = cita.CodCit });
                    command.Parameters.Add(new SqlParameter("@IndEWa", SqlDbType.Bit) { Value = isSuccess });
                    // ¡Aquí es donde capturamos la fecha y hora actual del sistema!
                    command.Parameters.Add(new SqlParameter("@FecEWa", SqlDbType.DateTime) { Value = DateTime.Now });
                    command.Parameters.Add(new SqlParameter("@MsjErrW", SqlDbType.NVarChar, -1) { Value = (object)errorMessage ?? DBNull.Value }); // Para cadenas que pueden ser NULL
                    command.Parameters.Add(new SqlParameter("@NroInt", SqlDbType.Int) { Value = nroInt });
                    await command.ExecuteNonQueryAsync(); // No se esperan resultados, solo ejecución
                }
            }
            catch (SqlException sqlEx)
            {

                _logger.LogError(sqlEx, "Error de SQL al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", sqlEx.Message);
                // Aquí puedes decidir qué hacer:
                // 1. Relanzar una excepción personalizada: throw new RepositorioException("No se pudo obtener la orden.", sqlEx);
                // 2. Devolver una lista vacía: return new List<ObservadorOrdenDto>();
                // 3. Manejar el error de alguna otra manera específica.
                throw; // Por ahora, relanzamos la excepción para que sea manejada por la capa superior
            }
            catch (Exception ex) // Captura cualquier otra excepción genérica
            {
                _logger.LogError(ex, "Error inesperado al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", ex.Message);
                throw; // Relanzamos para que se maneje en la capa superior
            }
            finally
            {
                // Opcional: Si quieres asegurar el cierre de la conexión, aunque EF Core lo gestiona
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

        }


        public async Task UpdateRecordatorioEnviadoStatusAsync(Cita cita, bool isSuccess, string errorMessage, int nroInt)
        {
            try
            {
                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    await _context.Database.OpenConnectionAsync();
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SP_MarcarEnviado_Recordatorio";
                    command.CommandType = CommandType.StoredProcedure;


                    command.Parameters.Add(new SqlParameter("@CodCit", SqlDbType.Int) { Value = cita.CodCit });
                    command.Parameters.Add(new SqlParameter("@IndRWa", SqlDbType.Bit) { Value = isSuccess });
                    // ¡Aquí es donde capturamos la fecha y hora actual del sistema!
                    command.Parameters.Add(new SqlParameter("@FecRWa", SqlDbType.DateTime) { Value = DateTime.Now });
                    command.Parameters.Add(new SqlParameter("@MsjERW", SqlDbType.NVarChar, -1) { Value = (object)errorMessage ?? DBNull.Value }); // Para cadenas que pueden ser NULL
                    command.Parameters.Add(new SqlParameter("@NroInt", SqlDbType.Int) { Value = nroInt });
                    await command.ExecuteNonQueryAsync(); // No se esperan resultados, solo ejecución
                }
            }
            catch (SqlException sqlEx)
            {

                _logger.LogError(sqlEx, "Error de SQL al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", sqlEx.Message);
                // Aquí puedes decidir qué hacer:
                // 1. Relanzar una excepción personalizada: throw new RepositorioException("No se pudo obtener la orden.", sqlEx);
                // 2. Devolver una lista vacía: return new List<ObservadorOrdenDto>();
                // 3. Manejar el error de alguna otra manera específica.
                throw; // Por ahora, relanzamos la excepción para que sea manejada por la capa superior
            }
            catch (Exception ex) // Captura cualquier otra excepción genérica
            {
                _logger.LogError(ex, "Error inesperado al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", ex.Message);
                throw; // Relanzamos para que se maneje en la capa superior
            }
            finally
            {
                // Opcional: Si quieres asegurar el cierre de la conexión, aunque EF Core lo gestiona
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

        }

        public async Task UpdateOrdenFinalizadoEnviadoStatusAsync(OrdenDisplayDto orden, bool isSuccess, string errorMessage, int nroInt)
        {
            try
            {
                if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                {
                    await _context.Database.OpenConnectionAsync();
                }

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SP_MarcarEnviado_Interconsultas";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@CodEmp", SqlDbType.Int) { Value = orden.CodEmp });
                    command.Parameters.Add(new SqlParameter("@CodSed", SqlDbType.Int) { Value = orden.CodSed });
                    command.Parameters.Add(new SqlParameter("@CodTcl", SqlDbType.Int) { Value = orden.CodTcl });
                    command.Parameters.Add(new SqlParameter("@NumOrd", SqlDbType.Int) { Value = orden.NumOrd });
                    command.Parameters.Add(new SqlParameter("@IndIWa", SqlDbType.Bit) { Value = isSuccess });
                    // ¡Aquí es donde capturamos la fecha y hora actual del sistema!
                    command.Parameters.Add(new SqlParameter("@FecIWa", SqlDbType.DateTime) { Value = DateTime.Now });
                    command.Parameters.Add(new SqlParameter("@MsjIRW", SqlDbType.NVarChar, -1) { Value = (object) errorMessage ?? DBNull.Value }); // Para cadenas que pueden ser NULL
                    command.Parameters.Add(new SqlParameter("@NroInt", SqlDbType.Int) { Value = nroInt });
                    await command.ExecuteNonQueryAsync(); // No se esperan resultados, solo ejecución
                }
            }
            catch (SqlException sqlEx)
            {

                _logger.LogError(sqlEx, "Error de SQL al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", sqlEx.Message);
                // Aquí puedes decidir qué hacer:
                // 1. Relanzar una excepción personalizada: throw new RepositorioException("No se pudo obtener la orden.", sqlEx);
                // 2. Devolver una lista vacía: return new List<ObservadorOrdenDto>();
                // 3. Manejar el error de alguna otra manera específica.
                throw; // Por ahora, relanzamos la excepción para que sea manejada por la capa superior
            }
            catch (Exception ex) // Captura cualquier otra excepción genérica
            {
                _logger.LogError(ex, "Error inesperado al ejecutar SP_OBSERVADOR_ORDEN. Mensaje: {Message}", ex.Message);
                throw; // Relanzamos para que se maneje en la capa superior
            }
            finally
            {
                // Opcional: Si quieres asegurar el cierre de la conexión, aunque EF Core lo gestiona
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    await _context.Database.CloseConnectionAsync();
                }
            }

        }


       
    }
}
