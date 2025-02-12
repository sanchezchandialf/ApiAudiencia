namespace ApiAudiencia.Models.DTOs
{
    public class AudienciaDTO
    {
        public int IdAudiencia { get; set; }
        public string CorreoElectronico { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public string Dni { get; set; } = null!;
        public string NombreEmpresa { get; set; } = null!;
        public int IdCargo { get; set; }
        public int IdClasificacion { get; set; }
        public int IdEstado { get; set; }
        public int AtendidoPor { get; set; }
        public string? DerivadoA { get; set; }
        public string? Estado { get; set; }
        public string Asunto { get; set; } = null!;
    }
}
