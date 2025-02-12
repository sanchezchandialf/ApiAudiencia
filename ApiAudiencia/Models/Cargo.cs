using System;
using System.Collections.Generic;

namespace ApiAudiencia.Models;

public partial class Cargo
{
    public int IdCargo { get; set; }

    public string NombreCargo { get; set; } = null!;

    public virtual ICollection<Audiencia> Audiencia { get; set; } = new List<Audiencia>();
}
