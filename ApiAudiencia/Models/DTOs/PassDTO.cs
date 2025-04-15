namespace ApiAudiencia.Models.DTOs
{
    public class PassDTO
    {
        public required string ClaveActual { get; set; }
        public required string ClaveNueva {get; set;}
    }

    public class RecoveryDTO : EmailRequestDTO
    {
        public required string ClaveNueva { get; set; }
        public required string Codigo { get; set; }
    }
}