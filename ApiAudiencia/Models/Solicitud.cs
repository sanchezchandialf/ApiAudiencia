using System;
using System.ComponentModel.DataAnnotations;

namespace ApiAudiencia.Models;

public class Solicitud
{
    [Key]
    public int Nro {get; set;}
    public required string Asunto {get; set;}
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Fecha {get; set;}
    public required string Clasif {get; set;}
    public required string Derivado {get; set;}
    public required string Estado {get; set;}
    //Claves foraneas
    public required Empleado Empleado {get; set;}
    public required Proveedor Proveedor {get; set;}
}
