using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvioNotificacionesDomian.Entities;
using EnvioNotificacionesDomian.Repositories;
using EnvioNotificacionesInfrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnvioNotificacionesInfrastructure.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task AddAsync(Persona persona)
        //{
        //    await _context.Personas.AddAsync(persona);
        //}

        //public async Task DeleteAsync(int id)
        //{
        //    var persona = await _context.Personas.FindAsync(id);
        //    if (persona != null)
        //    {
        //        _context.Personas.Remove(persona);
        //    }
        //}

        //public async Task<IEnumerable<Persona>> GetAllAsync()
        //{
        //    return await _context.Personas.ToListAsync();
        //}

        //public async Task<Persona?> GetByDniAsync(string dni)
        //{
        //    return await _context.Personas.FirstOrDefaultAsync(p => p.DNI == dni);
        //}

        //public async Task<Persona?> GetByIdAsync(int id)
        //{
        //    return await _context.Personas.FindAsync(id);
        //}

        //public async Task<int> SaveChangesAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}

        //public async Task UpdateAsync(Persona persona)
        //{
        //    _context.Personas.Update(persona);
        //    // _context.Entry(persona).State = EntityState.Modified; // Alternativa si no estás seguro del estado
        //}
    }
}
