using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiAudiencia.Models;
using ApiAudiencia.Custom;
using ApiAudiencia.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
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
            if (login == null)
            {
                return BadRequest("Datos incorrectos");
            }
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Correo == login.Correo);
            if (usuario == null)
            {
                return Unauthorized("Usuario no encontrado");
            }
            if (User != null)
            {
                var token = _utilidades.generarToken(usuario);
                return Ok(new

                { token = token,
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
            else
            {
                return Unauthorized("Usuario no encontrado");
            }
        }
    }
}
