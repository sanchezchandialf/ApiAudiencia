using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiAudiencia.Models;
using ApiAudiencia.Custom;
using ApiAudiencia.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
    [Route("/api/Usuario/editpass")]
    [Authorize]
    public async Task<IActionResult> ActualizarContraseña([FromBody] PassDTO modelo)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
    
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized("Usuario no autenticado");
        
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == userEmail);
    
        if (usuario == null)
            return NotFound("Usuario no encontrado");
        bool passwordValid;
        if (usuario.Clave.StartsWith("$2a$") || usuario.Clave.StartsWith("$2b$"))
        {
            // Verificación con BCrypt
            passwordValid = BCrypt.Net.BCrypt.Verify(modelo.ClaveActual, usuario.Clave);
        }
        else
        {
            // Migración: comparación en texto plano (solo durante transición)
            passwordValid = (usuario.Clave == modelo.ClaveActual);
        
            // Auto-migrar a BCrypt si la contraseña es correcta
            if (passwordValid)
            {
                usuario.Clave = BCrypt.Net.BCrypt.HashPassword(modelo.ClaveActual);
                await _context.SaveChangesAsync();
            }
        }

        if (!passwordValid)
            return BadRequest("Contraseña actual incorrecta");
        // 4. Hashear y guardar la nueva contraseña
        usuario.Clave = BCrypt.Net.BCrypt.HashPassword(modelo.ClaveNueva);
        
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

    [HttpPut]
    [Route("/api/Usuario/newpass")]
    public async Task<IActionResult> NuevaContraseña ([FromBody] RecoveryDTO modelo)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo == modelo.Destinatario);
        var emailrequest = await _context.EmailRequests.FirstOrDefaultAsync(er => er.Destinatario == modelo.Destinatario);
        if (usuario == null)
            return NotFound("Usuario no encontrado");
        if (emailrequest == null)
            return NotFound("No se encontro ninguna solicitud de recuperacion");
        if ((emailrequest.CodigoRecuperacionExpira - DateTime.Now).Minutes > 15) //Si ya paso los 15 min.
        {
            _context.EmailRequests.Remove(emailrequest); //Borro la solicitud de recuperacion de la BD si expiro.
            await _context.SaveChangesAsync(); //Guardo cambios
            return StatusCode(500,"El tiempo de la solicitud ha expirado");
        }
        if (modelo.Codigo.Equals(emailrequest.CodigoRecuperacion))
        {
            usuario.Clave = BCrypt.Net.BCrypt.HashPassword(modelo.ClaveNueva); //Cambio la contraseña
            _context.EmailRequests.Remove(emailrequest); //Borro la solicitud de recuperacion de la BD.
            try
            {
                await _context.SaveChangesAsync(); //Guardo cambios
                return Ok(new { message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al actualizar la contraseña");
            }
        }
        else
        {
            return StatusCode(500, "Error al introducir el codigo de autenticacion");
        }
    }
}