namespace ApiAudiencia.Models;

public class EmailRequest
{
    public int IdEmail { get; set; }
    public string Destinatario { get; set; }
    public string CodigoRecuperacion {get; set;}
    public DateTime CodigoRecuperacionExpira { get; set; }
}