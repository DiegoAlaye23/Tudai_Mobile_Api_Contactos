using GestionContactosApi.Models;

namespace GestionContactosApi.Services
{
    public class AuthService
    {
        private readonly List<Usuario> _usuarios = new()
        {
            new Usuario { UserName = "diego", Password = "tudaipam3", Rol = "Admin" },
            new Usuario { UserName = "user", Password = "123456", Rol = "User" }
        };

        public Usuario? Login(string username, string password)
        {
            return _usuarios.FirstOrDefault(u => u.UserName == username && u.Password == password);
        }
    }
}
