using System.ComponentModel.DataAnnotations;
namespace ApiAudiencia.Models.DTOs
{
    public class LoginDTOs
    {
        [EmailAddress]
        public required string Correo { get; set; }
        public required string Clave { get; set; }
    }
}
