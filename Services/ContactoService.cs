using ContApi.Models;
using GestionContactosApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionContactosApi.Services
{
    public class ContactoService
    {
        private readonly DbA358b2Pam3Context _context;

        public ContactoService(DbA358b2Pam3Context context)
        {
            _context = context;
        }

        public async Task<List<Contacto>> ObtenerTodosAsync()
        {
            return await _context.Contacto.ToListAsync();
        }

        public async Task<Contacto?> ObtenerPorIdAsync(int id)
        {
            return await _context.Contacto.FindAsync(id);
        }

        public async Task<bool> AgregarAsync(Contacto contacto)
        {
            _context.Contacto.Add(contacto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditarAsync(Contacto contacto)
        {
            _context.Entry(contacto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Contacto.AnyAsync(e => e.ContactoId == contacto.ContactoId))
                    return false;
                throw;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var contacto = await _context.Contacto.FindAsync(id);
            if (contacto == null)
                return false;

            _context.Contacto.Remove(contacto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
