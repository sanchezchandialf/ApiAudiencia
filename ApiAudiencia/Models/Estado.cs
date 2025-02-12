using System;
using System.Collections.Generic;

namespace ApiAudiencia.Models;

public partial class Estado
{
    public int Idestado { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Audiencia> Audiencia { get; set; } = new List<Audiencia>();
}
