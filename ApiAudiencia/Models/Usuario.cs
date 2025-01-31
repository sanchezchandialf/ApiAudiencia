using System;
using System.Collections.Generic;

namespace ApiAudiencia.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }

    public bool EsAdmin { get; set; }
}
