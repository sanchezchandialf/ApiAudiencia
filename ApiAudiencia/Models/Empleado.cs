using System.ComponentModel.DataAnnotations;

namespace ApiAudiencia.Models;

public class Empleado
{
    [Key]
    public required int DNI {get; set;}
    public required string Nombre {get; set;}
    public ICollection<Solicitud>? Solicitudes { get; set; }
}
