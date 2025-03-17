using ApiAudiencia.Models;
using ApiAudiencia.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

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
        [Authorize]
        public async Task<IActionResult> CrearAudiencia([FromBody] AudienciaDTO modelo)
        {
            if (modelo == null || !ModelState.IsValid)
            {
                return BadRequest("Datos inválidos.");
            }

            // Verificar si las claves foráneas existen antes de la inserción
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

            //  crea la nueva audiencia 
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
                IdEstado = modelo.IdEstado,  
                Asunto = modelo.Asunto
            };

            try
            {
                _context.Audiencias.Add(nuevaAudiencia);
                await _context.SaveChangesAsync();
                //sirve para devolver un 201
                return CreatedAtAction(nameof(CrearAudiencia), new { id = nuevaAudiencia.IdAudiencia }, nuevaAudiencia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObtenerAudiencias()
        {
            var audiencias = await _context.Audiencias
                .Join(_context.Empleados,
                      audiencia => audiencia.AtendidoPor,
                      empleado => empleado.IdEmpleado,
                      (audiencia, empleado) => new { audiencia, empleado })
                .Join(_context.Estados,
                      temp => temp.audiencia.IdEstado,
                      estado => estado.Idestado,
                      (temp, estado) => new { temp.audiencia, temp.empleado, estado })
                .Join(_context.Cargos,
                      temp => temp.audiencia.IdCargo,
                      cargo => cargo.IdCargo,
                      (temp, cargo) => new { temp.audiencia, temp.empleado, temp.estado, cargo })
                .Join(_context.Clasificaciones,
                      temp => temp.audiencia.IdClasificacion,
                      clasificacion => clasificacion.Idclasificacion,
                      (temp, clasificacion) => new
                      {
                          temp.audiencia.IdAudiencia,
                          temp.audiencia.CorreoElectronico,
                          Fecha=temp.audiencia.Fecha.ToString("dd/MM/yyyy"),
                          temp.audiencia.Dni,
                          temp.audiencia.NombreEmpresa,
                          Cargo = temp.cargo.NombreCargo,  // Nombre del Cargo
                          Clasificacion = clasificacion.Clasificacion, // Nombre de Clasificación
                          Estado = temp.estado.Nombre, // Nombre del Estado
                          AtendidoPor = temp.empleado.Nombre + " " + temp.empleado.Apellido, // Nombre del Atendente
                          temp.audiencia.DerivadoA,
                          temp.audiencia.Asunto
                      })
                .ToListAsync();

            return Ok(audiencias);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ObtenerAudienciaPorId(int id)
        {
            var audiencia = await _context.Audiencias
                .Where(a => a.IdAudiencia == id)
                .Join(_context.Empleados,
                      audiencia => audiencia.AtendidoPor,
                      empleado => empleado.IdEmpleado,
                      (audiencia, empleado) => new { audiencia, empleado })
                .Join(_context.Estados,
                      temp => temp.audiencia.IdEstado,
                      estado => estado.Idestado,
                      (temp, estado) => new { temp.audiencia, temp.empleado, estado })
                .Join(_context.Cargos,
                      temp => temp.audiencia.IdCargo,
                      cargo => cargo.IdCargo,
                      (temp, cargo) => new { temp.audiencia, temp.empleado, temp.estado, cargo })
                .Join(_context.Clasificaciones,
                      temp => temp.audiencia.IdClasificacion,
                      clasificacion => clasificacion.Idclasificacion,
                      (temp, clasificacion) => new
                      {
                          temp.audiencia.IdAudiencia,
                          temp.audiencia.CorreoElectronico,
                          Fecha = temp.audiencia.Fecha.ToString("dd/MM/yyyy"),
                          temp.audiencia.Dni,
                          temp.audiencia.NombreEmpresa,
                          Cargo = temp.cargo.NombreCargo,  // Nombre del Cargo
                          Clasificacion = clasificacion.Clasificacion, // Nombre de Clasificación
                          Estado = temp.estado.Nombre, // Nombre del Estado
                          AtendidoPor = temp.empleado.Nombre + " " + temp.empleado.Apellido, // Nombre del Atendente
                          temp.audiencia.DerivadoA,
                          temp.audiencia.Asunto
                      })
                .FirstOrDefaultAsync();

            if (audiencia == null)
            {
                return NotFound("Audiencia no encontrada.");
            }

            return Ok(audiencia);
        }



        [HttpGet("{fecha}")]
        [Authorize]
        public async Task<IActionResult> ObtenerAudienciaPorFecha(DateTime fecha)
        {
            var audiencia = await _context.Audiencias
                .Where(a => a.Fecha == fecha)
                .Join(_context.Empleados,
                      audiencia => audiencia.AtendidoPor,
                      empleado => empleado.IdEmpleado,
                      (audiencia, empleado) => new { audiencia, empleado })
                .Join(_context.Estados,
                      temp => temp.audiencia.IdEstado,
                      estado => estado.Idestado,
                      (temp, estado) => new { temp.audiencia, temp.empleado, estado })
                .Join(_context.Cargos,
                      temp => temp.audiencia.IdCargo,
                      cargo => cargo.IdCargo,
                      (temp, cargo) => new { temp.audiencia, temp.empleado, temp.estado, cargo })
                .Join(_context.Clasificaciones,
                      temp => temp.audiencia.IdClasificacion,
                      clasificacion => clasificacion.Idclasificacion,
                      (temp, clasificacion) => new
                      {
                          temp.audiencia.IdAudiencia,
                          temp.audiencia.CorreoElectronico,
                          Fecha = temp.audiencia.Fecha.ToString("dd/MM/yyyy"),
                          temp.audiencia.Dni,
                          temp.audiencia.NombreEmpresa,
                          Cargo = temp.cargo.NombreCargo,  // Nombre del Cargo
                          Clasificacion = clasificacion.Clasificacion, // Nombre de Clasificación
                          Estado = temp.estado.Nombre, // Nombre del Estado
                          AtendidoPor = temp.empleado.Nombre + " " + temp.empleado.Apellido, // Nombre del Atendente
                          temp.audiencia.DerivadoA,
                          temp.audiencia.Asunto
                      })
                .ToListAsync();
            if (audiencia == null)
            {
                return NotFound("Audiencia no encontrada.");
            }
            return Ok(audiencia);
        }





        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> ActualizarAudiencia(int id, [FromBody] AudienciaDTO modelo)
        {
            if (modelo == null || !ModelState.IsValid)
            {
                return BadRequest("Datos inválidos.");
            }

            var audienciaExistente = await _context.Audiencias.FindAsync(id);
            if (audienciaExistente == null)
            {
                return NotFound("Audiencia no encontrada.");
            }

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

            audienciaExistente.CorreoElectronico = modelo.CorreoElectronico;
            audienciaExistente.Fecha = modelo.Fecha;
            audienciaExistente.Dni = modelo.Dni;
            audienciaExistente.IdCargo = modelo.IdCargo;
            audienciaExistente.NombreEmpresa = modelo.NombreEmpresa;
            audienciaExistente.IdClasificacion = modelo.IdClasificacion;
            audienciaExistente.DerivadoA = modelo.DerivadoA;
            audienciaExistente.AtendidoPor = modelo.AtendidoPor;
            audienciaExistente.IdEstado = modelo.IdEstado;
            audienciaExistente.Asunto = modelo.Asunto;

            try
            {
                _context.Audiencias.Update(audienciaExistente);
                await _context.SaveChangesAsync();
                return Ok(audienciaExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> borrarForm(int id)
        {
            var audienciaExistente = await _context.Audiencias.FindAsync(id);
            if (audienciaExistente == null)
            {
                return NotFound();
            }

            try
            {
                _context.Audiencias.Remove(audienciaExistente);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                // Log the exception (ex) here
                return Problem("Ocurrió un error al procesar la solicitud.");
            }
        } 
    
    }
    
}


