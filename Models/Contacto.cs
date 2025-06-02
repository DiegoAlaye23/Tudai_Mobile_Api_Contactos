namespace GestionContactosApi.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Contacto
    {
        [Column("ContactoId")]
        public int ContactoId { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Apellido")]
        public string Apellido { get; set; }

        [Column("Teléfono")]
        public long Telefono { get; set; }  // Nota el nombre de la propiedad sin tilde

        [Column("Email")]
        public string Email { get; set; }

        // [Column("UsuarioId")]//
        //public int UsuarioId { get; set; }

        [Column("Activo")]
        public bool Activo { get; set; }
    }


}
