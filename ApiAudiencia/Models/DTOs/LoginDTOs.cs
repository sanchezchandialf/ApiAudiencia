using System.ComponentModel.DataAnnotations;
namespace ApiAudiencia.Models.DTOs
{
    public class LoginDTOs
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
        [Required]
        public string Clave { get; set; }
    }
}
