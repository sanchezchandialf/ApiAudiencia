using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiAudiencia.Models;
using ApiAudiencia.Custom;
using ApiAudiencia.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace ApiAudiencia.Controllers;
[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly AudienciasContext _context;
    private readonly Utilidades _utilidades;
    public UsuarioController(AudienciasContext context, Utilidades utilidades)
    {
        _context = context;
        _utilidades = utilidades;
    }
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> ActualizarContraseña([FromBody] PassDTO modelo)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Correo == modelo.Correo);
        if (usuario == null)
        {
            return Unauthorized("Usuario no encontrado");
        }
        if (User != null)
        {
            var pass = modelo.ClaveActual;
            if (usuario.Clave == pass)
            {
                usuario.Clave=modelo.ClaveNueva;
                _context.Usuarios.Update(usuario);
            }
            return Ok(usuario);
        }
        else
        {
            return Unauthorized("Usuario no encontrado");
        }
    }
}