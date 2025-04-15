namespace ApiAudiencia.Models;

public class EmailRequest
{
    public int IdEmail { get; set; }
    public required string Destinatario { get; set; }
    public required string CodigoRecuperacion {get; set;}
    public DateTime CodigoRecuperacionExpira { get; set; }
}