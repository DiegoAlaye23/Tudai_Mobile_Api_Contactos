using System;
using System.Collections.Generic;
using GestionContactosApi.Models;

namespace ContApi.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string? NombreApellido { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Contacto> Contactos { get; set; } = new List<Contacto>();
}
