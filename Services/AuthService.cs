using ContApi.Models; // Para DbA358b2Pam3Context y Usuario
using Microsoft.EntityFrameworkCore;

namespace GestionContactosApi.Services
{
    public class AuthService
    {
        private readonly DbA358b2Pam3Context _context;

        public AuthService(DbA358b2Pam3Context context)
        {
            _context = context;
        }

        public async Task<Usuario?> LoginAsync(string username, string password)
        {
            // Buscar usuario activo con email o nombre que coincida y password igual
            return await _context.Usuario
                .Where(u => u.Activo == true)
                .FirstOrDefaultAsync(u =>
                    (u.Email == username || u.NombreApellido == username)
                    && u.Password == password);
        }
    }
}
