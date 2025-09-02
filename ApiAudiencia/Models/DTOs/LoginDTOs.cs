using System.ComponentModel.DataAnnotations;
namespace ApiAudiencia.Models.DTOs
{
    public class LoginDTOs
    {
        [EmailAddress]
        public required string correo { get; set; }
        public required string clave { get; set; }
    }
}
