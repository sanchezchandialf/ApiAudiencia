using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiAudiencia.Models;
using ApiAudiencia.Custom;
using ApiAudiencia.Models.DTOs;
using Microsoft.EntityFrameworkCore;
namespace ApiAudiencia.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AcessoController : ControllerBase
    {
        private readonly AudienciasContext _context;
        private readonly Utilidades _utilidades;
        public AcessoController(AudienciasContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTOs login)
        {
            if (string.IsNullOrEmpty(login.Correo) || string.IsNullOrEmpty(login.Clave))
            {
                return BadRequest("Correo y contraseña son requeridos");
            } 
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Correo == login.Correo);
    
            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas");
            }
            
            bool credencialesValidas = false;
    
            if (usuario.Clave.StartsWith("$2a$") || usuario.Clave.StartsWith("$2b$")) 
            {
                credencialesValidas = BCrypt.Net.BCrypt.Verify(login.Clave, usuario.Clave);
            }
            else
            {
                credencialesValidas = (usuario.Clave == login.Clave);
        
                if (credencialesValidas)
                {
                    usuario.Clave = BCrypt.Net.BCrypt.HashPassword(login.Clave);
                    await _context.SaveChangesAsync();
                }
            }

            if (!credencialesValidas)
            {
                return Unauthorized("Credenciales inválidas");
            }

            var token = _utilidades.generarToken(usuario);
    
            return Ok(new
            {
                token = token,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(60),
                usuario = new
                {
                    usuario.IdUsuario,
                    usuario.Nombre,
                    usuario.Correo,
                    usuario.EsAdmin
                }
            });
        }
    }
}
