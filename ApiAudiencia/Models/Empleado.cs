using System;
using System.Collections.Generic;

namespace ApiAudiencia.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public virtual ICollection<Audiencia> Audiencia { get; set; } = new List<Audiencia>();
}
