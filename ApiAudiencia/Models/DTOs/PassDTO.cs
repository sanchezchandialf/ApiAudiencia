namespace ApiAudiencia.Models.DTOs
{
    public class PassDTO
    {
        public string ClaveActual { get; set; }
        public string ClaveNueva {get; set;}
    }

    public class RecoveryDTO : EmailRequestDTO
    {
        public string ClaveNueva { get; set; }
        public string Codigo { get; set; }
    }
}