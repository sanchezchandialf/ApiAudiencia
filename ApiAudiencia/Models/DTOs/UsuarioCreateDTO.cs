namespace ApiAudiencia.Models.DTOs;

public class UsuarioCreateDTO:UsuarioUpdateDTO
{
    public required string Clave {get;set;}
}