namespace GestionContactosApi.Models
{
    public class Contacto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Domicilio { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
    }
}
