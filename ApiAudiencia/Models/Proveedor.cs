using System;
using System.ComponentModel.DataAnnotations;

namespace ApiAudiencia.Models;

public class Proveedor
{
    [Key]
    public required int DNI {get; set;}
    public required string Correo {get; set;}
    public required string Nombre {get; set;}
    public ICollection<Solicitud>? Solicitudes { get; set; }
}
