using System;
using System.Collections.Generic;

namespace ApiAudiencia.Models;

public partial class Clasificacione
{
    public int Idclasificacion { get; set; }

    public string Clasificacion { get; set; } = null!;

    public virtual ICollection<Audiencia> Audiencia { get; set; } = new List<Audiencia>();
}
