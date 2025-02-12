using ApiAudiencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAudiencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : ControllerBase
    {
        private readonly AudienciasContext _context;

        public FormController(AudienciasContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CrearAudiencia([FromBody] Audiencia modelo)
        {
            if (modelo == null || !ModelState.IsValid)
            {
                return BadRequest("Datos inválidos.");
            }

            // ✅ Verificar si las claves foráneas existen antes de la inserción
            bool cargoExiste = await _context.Cargos.AnyAsync(c => c.IdCargo == modelo.IdCargo);
            bool clasificacionExiste = await _context.Clasificaciones.AnyAsync(cl => cl.Idclasificacion == modelo.IdClasificacion);
            bool empleadoExiste = await _context.Empleados.AnyAsync(e => e.IdEmpleado == modelo.AtendidoPor);
            bool estadoExiste = await _context.Estados.AnyAsync(est => est.Idestado == modelo.IdEstado);

            if (!cargoExiste)
                return BadRequest("El Cargo especificado no existe.");
            if (!clasificacionExiste)
                return BadRequest("La Clasificación especificada no existe.");
            if (!empleadoExiste)
                return BadRequest("El Empleado especificado no existe.");
            if (!estadoExiste)
                return BadRequest("El Estado especificado no existe.");

            // ✅ Crear la nueva audiencia con los valores correctos
            var nuevaAudiencia = new Audiencia
            {
                CorreoElectronico = modelo.CorreoElectronico,
                Fecha = modelo.Fecha,
                Dni = modelo.Dni,
                IdCargo = modelo.IdCargo,
                NombreEmpresa = modelo.NombreEmpresa,
                IdClasificacion = modelo.IdClasificacion,
                DerivadoA = modelo.DerivadoA,
                AtendidoPor = modelo.AtendidoPor,
                IdEstado = modelo.IdEstado,  // Ahora usamos el ID en vez de un string
                Asunto = modelo.Asunto
            };

            try
            {
                _context.Audiencias.Add(nuevaAudiencia);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(CrearAudiencia), new { id = nuevaAudiencia.IdAudiencia }, nuevaAudiencia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
