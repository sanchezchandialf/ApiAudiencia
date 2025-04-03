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
    [Route("/[controller]/editpass")]
    [Authorize]
    public async Task<IActionResult> ActualizarContraseña([FromBody] PassDTO modelo)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Correo == modelo.Correo);
        var esPropioUsuario = usuario.Correo.Equals(modelo.Correo, StringComparison.OrdinalIgnoreCase);

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
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Contraseña actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al actualizar la contraseña");
            }
        }
        else
        {
            return Unauthorized("Usuario no encontrado");
        }
    }

    [HttpPut]
    [Route("/[controller]/edituser")]
    [Authorize]
    public async Task<IActionResult> ActualizarContraseña([FromBody] UsuarioUpdateDTO modelo)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Correo == modelo.Correo);
        var esPropioUsuario = usuario.Correo.Equals(modelo.Correo, StringComparison.OrdinalIgnoreCase);

        if (usuario == null)
        {
            return Unauthorized("Usuario no encontrado");
        }
        if (esPropioUsuario)
        {
            usuario.Nombre = modelo.Nombre ?? usuario.Nombre;
            usuario.Correo = modelo.Correo ?? usuario.Correo;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al actualizar la contraseña");
            }
        }
        else
        {
            return Unauthorized("Usuario no encontrado");
        }
    }
    
}