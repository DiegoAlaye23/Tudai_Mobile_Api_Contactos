namespace GestionContactosApi.Models
{
    public class Usuario
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = "User"; 
    }
}
